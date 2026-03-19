using FastParts.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastParts.Controllers
{
    public class CitaController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //Cita Cliente

        [Authorize(Roles = "Cliente")]
        public ActionResult CrearCliente()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var dt = DateTime.Now.AddHours(1);

            // Redondear hacia arriba al siguiente bloque de 30 minutos
            var minutos = dt.Minute;
            var siguienteBloque = (int)(Math.Ceiling(minutos / 30.0) * 30);
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0).AddMinutes(siguienteBloque);

            var model = new CrearCitaClienteViewModel
            {
                NombreCliente = user?.NombreCompleto,
                TelefonoCliente = "8888-8888",
                FechaCita = dt
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public ActionResult CrearCliente(CrearCitaClienteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.FechaCita = new DateTime(
                model.FechaCita.Year,
                model.FechaCita.Month,
                model.FechaCita.Day,
                model.FechaCita.Hour,
                model.FechaCita.Minute,
                0
            );

            if (!ValidarReglasCita(model.FechaCita, nameof(model.FechaCita)))
                return View(model);

            var userId = User.Identity.GetUserId();

            var cita = new CitaModel
            {
                UsuarioId = userId,
                NombreCliente = model.NombreCliente,
                TelefonoCliente = model.TelefonoCliente,
                Vehiculo = model.Vehiculo,
                Placa = model.Placa,
                FechaCita = model.FechaCita,
                Motivo = model.Motivo,
                Estado = "Ingresada"
            };

            db.CitaModels.Add(cita);
            db.SaveChanges();

            TempData["CitaCreada"] = "La cita fue registrada correctamente.";
            return RedirectToAction("MisCitas");
        }


        [Authorize(Roles = "Cliente")]
        public ActionResult MisCitas()
        {
            var userId = User.Identity.GetUserId();
            var ahora = DateTime.Now;

            var citas = db.CitaModels
                .Where(c => c.UsuarioId == userId)
                .OrderByDescending(c => c.FechaCita)
                .Select(c => new CitaListadoViewModel
                {
                    Id = c.Id,
                    NombreCliente = c.NombreCliente,
                    TelefonoCliente = c.TelefonoCliente,
                    Vehiculo = c.Vehiculo,
                    Placa = c.Placa,
                    FechaCita = c.FechaCita,
                    Estado = c.Estado,
                    NombreMecanico = c.Mecanico != null ? c.Mecanico.NombreCompleto : "",
                    PuedeCancelar = c.FechaCita > ahora
                            && c.MecanicoId == null
                            && c.HoraInicio == null
                            && c.Estado == "Ingresada"
                })
                .ToList();

            return View(citas);
        }

        //Agendar Cita Admin 

        [Authorize(Roles = "Admin")]
        public ActionResult CrearAdmin()
        {
            var dt = DateTime.Now.AddHours(1);

            // redondear hacia arriba a 00 o 30
            var minutos = dt.Minute;
            var siguienteBloque = (int)(Math.Ceiling(minutos / 30.0) * 30);
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0).AddMinutes(siguienteBloque);

            var model = new CrearCitaAdminViewModel
            {
                FechaCita = dt
            };

            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult CrearAdmin(CrearCitaAdminViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.FechaCita = new DateTime(
                model.FechaCita.Year,
                model.FechaCita.Month,
                model.FechaCita.Day,
                model.FechaCita.Hour,
                model.FechaCita.Minute,
                0
            );

            if (!ValidarReglasCita(model.FechaCita, nameof(model.FechaCita)))
                return View(model);

            var cita = new CitaModel
            {
                UsuarioId = null,
                NombreCliente = model.NombreCliente,
                TelefonoCliente = model.TelefonoCliente,
                Vehiculo = model.Vehiculo,
                Placa = model.Placa,
                FechaCita = model.FechaCita,
                Motivo = model.Motivo,
                Estado = "Ingresada"
            };

            db.CitaModels.Add(cita);
            db.SaveChanges();

            TempData["CitaCreada"] = "La cita fue registrada correctamente.";
            return RedirectToAction("ListaAdmin");
        }



        //Listado citas - Admin
        [Authorize(Roles = "Admin")]
        public ActionResult ListaAdmin()
        {
            var ahora = DateTime.Now;

            var citas = db.CitaModels
                .OrderByDescending(c => c.FechaCita)
                .Select(c => new CitaListadoViewModel
                {
                    Id = c.Id,
                    NombreCliente = c.NombreCliente,
                    TelefonoCliente = c.TelefonoCliente,
                    Vehiculo = c.Vehiculo,
                    Placa = c.Placa,
                    FechaCita = c.FechaCita,
                    Estado = c.Estado,
                    NombreMecanico = c.Mecanico != null ? c.Mecanico.NombreCompleto : "",
                    PuedeCancelar = c.FechaCita > ahora
                            && c.MecanicoId == null
                            && c.HoraInicio == null
                            && c.Estado == "Ingresada"
                })
                .ToList();

            return View(citas);
        }

        // Detaelles cita - Admin

        [Authorize(Roles = "Admin")]
        public ActionResult DetalleAdmin(int id)
        {
            var cita = db.CitaModels
                .Include(c => c.Mecanico)
                .Include(c => c.Usuario)
                .FirstOrDefault(c => c.Id == id);

            if (cita == null)
                return HttpNotFound();

            var model = new CitaDetalleViewModel
            {
                Id = cita.Id,
                NombreCliente = cita.NombreCliente,
                TelefonoCliente = cita.TelefonoCliente,
                Vehiculo = cita.Vehiculo,
                Placa = cita.Placa,
                FechaCita = cita.FechaCita,
                Estado = cita.Estado,
                NombreMecanico = cita.Mecanico != null ? cita.Mecanico.NombreCompleto : "(Sin mecánico)",
                NombreUsuario = cita.Usuario != null ? cita.Usuario.UserName : "(Cliente sin cuenta)",
                Motivo = cita.Motivo,
                HoraInicio = cita.HoraInicio,
                HoraFin = cita.HoraFin,
                TieneFotoAntes = cita.FotoAntes != null,
                TieneFotoDespues = cita.FotoDespues != null
            };

            return View(model);
        }

        //Detalle cita - Cliente
        [Authorize(Roles = "Cliente")]
        public ActionResult DetalleCliente(int id)
        {
            var userId = User.Identity.GetUserId();

            var cita = db.CitaModels
                .Include(c => c.Mecanico)
                .Include(c => c.Usuario)
                .FirstOrDefault(c => c.Id == id && c.UsuarioId == userId);

            if (cita == null)
                return HttpNotFound();

            var model = new CitaDetalleViewModel
            {
                Id = cita.Id,
                NombreCliente = cita.NombreCliente,
                TelefonoCliente = cita.TelefonoCliente,
                Vehiculo = cita.Vehiculo,
                Placa = cita.Placa,
                FechaCita = cita.FechaCita,
                Estado = cita.Estado,
                NombreMecanico = cita.Mecanico != null ? cita.Mecanico.NombreCompleto : "(Sin mecánico)",
                NombreUsuario = cita.Usuario != null ? cita.Usuario.UserName : "",
                Motivo = cita.Estado == "Terminada" ? cita.Motivo : "",
                HoraInicio = cita.HoraInicio,
                HoraFin = cita.HoraFin,
                TieneFotoAntes = cita.FotoAntes != null,
                TieneFotoDespues = cita.FotoDespues != null
            };

            return View(model);
        }



        // Cola citas sin mecanico asignado

        [Authorize(Roles = "Mecanico")]
        public ActionResult Cola()
        {
            var citas = db.CitaModels
                .Where(c => c.Estado == "Ingresada")
                .OrderBy(c => c.FechaCita)
                .Select(c => new CitaListadoViewModel
                {
                    Id = c.Id,
                    NombreCliente = c.NombreCliente,
                    TelefonoCliente = c.TelefonoCliente,
                    Vehiculo = c.Vehiculo,
                    Placa = c.Placa,
                    FechaCita = c.FechaCita,
                    Estado = c.Estado
                })
                .ToList();

            return View(citas);
        }

        // Asignar emcanico a la cita

        [HttpPost]
        [Authorize(Roles = "Mecanico")]
        public ActionResult TomarCita(int id)
        {
            var userId = User.Identity.GetUserId();

            var cita = db.CitaModels.Find(id);
            if (cita == null)
                return HttpNotFound();

            if (cita.MecanicoId == null && cita.Estado == "Ingresada")
            {
                cita.MecanicoId = userId;
                cita.Estado = "Asignada";
                db.SaveChanges();
            }

            return RedirectToAction("MisCitasMecanico");
        }

        // Lista de citas asignadas al mecánico
        [Authorize(Roles = "Mecanico")]
        public ActionResult MisCitasMecanico()
        {
            var userId = User.Identity.GetUserId();

            var citas = db.CitaModels
                .Where(c => c.MecanicoId == userId)
                .OrderByDescending(c => c.FechaCita)
                .Select(c => new CitaListadoViewModel
                {
                    Id = c.Id,
                    NombreCliente = c.NombreCliente,
                    TelefonoCliente = c.TelefonoCliente,
                    Vehiculo = c.Vehiculo,
                    Placa = c.Placa,
                    FechaCita = c.FechaCita,
                    Estado = c.Estado,
                    NombreMecanico = c.Mecanico.NombreCompleto
                })
                .ToList();

            return View(citas);
        }

        // Inicia el servicio mecanico
        [HttpPost]
        [Authorize(Roles = "Mecanico")]
        public ActionResult IniciarServicio(int id)
        {
            var userId = User.Identity.GetUserId();
            var cita = db.CitaModels.Find(id);

            if (cita == null || cita.MecanicoId != userId)
                return new HttpUnauthorizedResult();

            if (!cita.HoraInicio.HasValue)
            {
                cita.HoraInicio = DateTime.Now;
                cita.Estado = "En Proceso";
                db.SaveChanges();
            }

            return RedirectToAction("MisCitasMecanico");
        }

        [Authorize(Roles = "Mecanico")]
        public ActionResult FinalizarServicio(int id)
        {
            var userId = User.Identity.GetUserId();
            var cita = db.CitaModels.Find(id);

            if (cita == null || cita.MecanicoId != userId)
                return new HttpUnauthorizedResult();

            var model = new CitaFinalizarViewModel
            {
                CitaId = cita.Id,
                Motivo = cita.Motivo
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Mecanico")]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizarServicio(CitaFinalizarViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var cita = db.CitaModels.Find(model.CitaId);

            if (cita == null || cita.MecanicoId != userId)
                return new HttpUnauthorizedResult();

            if (!ModelState.IsValid)
                return View(model);

            cita.Motivo = model.Motivo;
            cita.Estado = "Terminada";
            cita.HoraFin = DateTime.Now;

            GuardarImagen(model.FotoAntes, out var antes, out var antesType);
            if (antes != null)
            {
                cita.FotoAntes = antes;
                cita.FotoAntesContentType = antesType;
            }

            GuardarImagen(model.FotoDespues, out var despues, out var despuesType);
            if (despues != null)
            {
                cita.FotoDespues = despues;
                cita.FotoDespuesContentType = despuesType;
            }

            db.SaveChanges();

            return RedirectToAction("MisCitasMecanico");
        }



        // Guardar fotos del vehiculo
        private string GuardarFoto(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var relativePath = "~/Content/FotosCitas/" + fileName;
            var absolutePath = Server.MapPath(relativePath);

            var directory = Path.GetDirectoryName(absolutePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            file.SaveAs(absolutePath);

            return VirtualPathUtility.ToAbsolute(relativePath);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Editar Citas Mecanico

        [HttpPost]
        [Authorize(Roles = "Mecanico")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarServicio(int id)
        {
            var userId = User.Identity.GetUserId();
            var cita = db.CitaModels.Find(id);

            if (cita == null || cita.MecanicoId != userId)
                return new HttpUnauthorizedResult();

            if (cita.Estado != "Terminada")
                return new HttpStatusCodeResult(400, "Solo se pueden editar citas terminadas.");

            var model = new CitaEditarMecanicoViewModel
            {
                CitaId = cita.Id,
                Motivo = cita.Motivo,
                TieneFotoAntes = cita.FotoAntes != null,
                TieneFotoDespues = cita.FotoDespues != null
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Mecanico")]
        public ActionResult EditarServicio(CitaEditarMecanicoViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var cita = db.CitaModels.Find(model.CitaId);

            if (cita == null || cita.MecanicoId != userId)
                return new HttpUnauthorizedResult();

            if (cita.Estado != "Terminada")
                return new HttpStatusCodeResult(400, "Solo se pueden editar citas terminadas.");

            if (!ModelState.IsValid)
                return View(model);

            cita.Motivo = model.Motivo;

            GuardarImagen(model.FotoAntes, out var antes, out var antesType);
            if (antes != null)
            {
                cita.FotoAntes = antes;
                cita.FotoAntesContentType = antesType;
            }

            GuardarImagen(model.FotoDespues, out var despues, out var despuesType);
            if (despues != null)
            {
                cita.FotoDespues = despues;
                cita.FotoDespuesContentType = despuesType;
            }

            db.SaveChanges();

            TempData["CitaEditadaMecanico"] = "La cita fue actualizada correctamente.";
            return RedirectToAction("MisCitasMecanico");
        }



        private void GuardarImagen(HttpPostedFileBase archivo, out byte[] binario, out string contentType)
        {
            binario = null;
            contentType = null;

            if (archivo != null && archivo.ContentLength > 0)
            {
                using (var ms = new MemoryStream())
                {
                    archivo.InputStream.CopyTo(ms);
                    binario = ms.ToArray();
                    contentType = archivo.ContentType;
                }
            }
        }

        public FileContentResult ObtenerFoto(int id, string tipo)
        {
            var cita = db.CitaModels.Find(id);
            if (cita == null)
                return null;

            if (tipo == "antes" && cita.FotoAntes != null)
                return File(cita.FotoAntes, cita.FotoAntesContentType);

            if (tipo == "despues" && cita.FotoDespues != null)
                return File(cita.FotoDespues, cita.FotoDespuesContentType);

            return null;
        }

        //Metodo para cancelar citas desd vista client

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public ActionResult CancelarCliente(int id)
        {
            var userId = User.Identity.GetUserId();

            var cita = db.CitaModels
                .Include(c => c.Servicios)
                .Include(c => c.Repuestos)
                .FirstOrDefault(c => c.Id == id && c.UsuarioId == userId);

            if (cita == null)
                return HttpNotFound();

            bool puedeCancelar = cita.FechaCita > DateTime.Now
                                 && cita.MecanicoId == null
                                 && !cita.HoraInicio.HasValue
                                 && cita.Estado == "Ingresada";

            if (!puedeCancelar)
            {
                TempData["ErrorCancelarCita"] = "Esta cita ya no se puede cancelar porque ya fue tomada por un mecánico, ya inició o ya no es futura.";
                return RedirectToAction("MisCitas");
            }

            if (cita.Servicios != null && cita.Servicios.Any())
                db.CitaServicioModels.RemoveRange(cita.Servicios);

            if (cita.Repuestos != null && cita.Repuestos.Any())
                db.CitaRepuestoModels.RemoveRange(cita.Repuestos);

            db.CitaModels.Remove(cita);
            db.SaveChanges();

            TempData["CitaCancelada"] = "La cita fue cancelada correctamente.";
            return RedirectToAction("MisCitas");
        }


        // Cancelar cita vista Admin

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult CancelarAdmin(int id)
        {
            var cita = db.CitaModels
                .Include(c => c.Servicios)
                .Include(c => c.Repuestos)
                .FirstOrDefault(c => c.Id == id);

            if (cita == null)
                return HttpNotFound();

            bool puedeCancelar = cita.FechaCita > DateTime.Now
                                 && cita.MecanicoId == null
                                 && !cita.HoraInicio.HasValue
                                 && cita.Estado == "Ingresada";

            if (!puedeCancelar)
            {
                TempData["ErrorCancelarCita"] = "La cita no se puede cancelar porque ya fue tomada por un mecánico, ya inició o ya no es futura.";
                return RedirectToAction("ListaAdmin");
            }
                        
            if (cita.Servicios != null && cita.Servicios.Any())
                db.CitaServicioModels.RemoveRange(cita.Servicios);

            if (cita.Repuestos != null && cita.Repuestos.Any())
                db.CitaRepuestoModels.RemoveRange(cita.Repuestos);

            db.CitaModels.Remove(cita);
            db.SaveChanges();

            TempData["CitaCancelada"] = "La cita fue cancelada correctamente.";
            return RedirectToAction("ListaAdmin");
        }


        private bool ValidarReglasCita(DateTime fechaCita, string campoFecha = "FechaCita")
        {
            // Normalizar sin segundos
            fechaCita = new DateTime(
                fechaCita.Year,
                fechaCita.Month,
                fechaCita.Day,
                fechaCita.Hour,
                fechaCita.Minute,
                0
            );

            //  Mínimo 1 hora en el futuro
            var minimoPermitido = DateTime.Now.AddHours(1);
            if (fechaCita < minimoPermitido)
            {
                ModelState.AddModelError(campoFecha,
                    "La fecha/hora de la cita debe ser al menos 1 hora en el futuro.");
                return false;
            }

            //  No domingos
            var dia = fechaCita.DayOfWeek;
            if (dia == DayOfWeek.Sunday)
            {
                ModelState.AddModelError(campoFecha,
                    "No se pueden agendar citas los domingos (taller cerrado).");
                return false;
            }

            // Bloques de 30 min
            if ((fechaCita.Minute % 30) != 0 || fechaCita.Second != 0)
            {
                ModelState.AddModelError(campoFecha,
                    "La hora debe seleccionarse en intervalos de 30 minutos (por ejemplo 10:00 o 10:30).");
                return false;
            }

            // Horario laboral
            int minutosDia = fechaCita.Hour * 60 + fechaCita.Minute;
            int openMin;
            int closeMin;

            if (dia == DayOfWeek.Saturday)
            {
                openMin = 8 * 60;
                closeMin = 12 * 60;

                if (minutosDia < openMin || minutosDia >= closeMin)
                {
                    ModelState.AddModelError(campoFecha,
                        "Sábados el horario es de 8:00 AM a 12:00 PM.");
                    return false;
                }
            }
            else
            {
                openMin = 8 * 60;
                closeMin = 18 * 60;

                if (minutosDia < openMin || minutosDia >= closeMin)
                {
                    ModelState.AddModelError(campoFecha,
                        "El horario es de lunes a viernes de 8:00 AM a 6:00 PM.");
                    return false;
                }
            }

            // Máximo 2 citas por slot
            int citasEnSlot = db.CitaModels.Count(c =>
                c.FechaCita == fechaCita &&
                c.Estado != "Cancelada"
            );

            if (citasEnSlot >= 2)
            {
                ModelState.AddModelError(campoFecha,
                    "Este horario ya alcanzó el máximo de citas. Por favor selecciona otra hora.");
                return false;
            }

            return true;
        }


        [HttpGet]
        [Authorize(Roles = "Cliente,Admin")]
        public JsonResult ObtenerHorasDisponibles(DateTime fecha)
        {
            var horas = new List<string>();

            if (fecha.Date < DateTime.Today || fecha.DayOfWeek == DayOfWeek.Sunday)
                return Json(horas, JsonRequestBehavior.AllowGet);

            int horaInicio = 8;
            int horaFin = fecha.DayOfWeek == DayOfWeek.Saturday ? 12 : 18;

            var minimo = DateTime.Now.AddHours(1);

            for (int h = horaInicio; h < horaFin; h++)
            {
                foreach (var minuto in new[] { 0, 30 })
                {
                    var slot = new DateTime(fecha.Year, fecha.Month, fecha.Day, h, minuto, 0);

                    if (slot < minimo)
                        continue;

                    int citasEnSlot = db.CitaModels.Count(c => c.FechaCita == slot);

                    if (citasEnSlot < 2)
                    {
                        horas.Add(slot.ToString("HH:mm"));
                    }
                }
            }

            return Json(horas, JsonRequestBehavior.AllowGet);
        }




    }

}