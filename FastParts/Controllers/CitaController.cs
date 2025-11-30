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

            var model = new CrearCitaClienteViewModel
            {
                NombreCliente = user?.NombreCompleto,
                TelefonoCliente = "8888-8888",
                FechaCita = DateTime.Now.AddHours(1)
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

            return RedirectToAction("MisCitas");
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult MisCitas()
        {
            var userId = User.Identity.GetUserId();

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
                    NombreMecanico = c.Mecanico.NombreCompleto
                })
                .ToList();

            return View(citas);
        }

        //Agendar Cita Admin 


        [Authorize(Roles = "Admin")]
        public ActionResult CrearAdmin()
        {
            var model = new CrearCitaAdminViewModel
            {
                FechaCita = DateTime.Now.AddHours(1)
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

            return RedirectToAction("ListaAdmin");
        }

        //Listado citas - Admin
        [Authorize(Roles = "Admin")]
        public ActionResult ListaAdmin()
        {
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
                    NombreMecanico = c.Mecanico.NombreCompleto
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



    }

}