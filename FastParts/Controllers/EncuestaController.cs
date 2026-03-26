using FastParts.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace FastParts.Controllers
{
    public class EncuestaController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // Get responder encuesta
        [Authorize(Roles = "Cliente")]
        public ActionResult Responder(int id)
        {
            var userId = User.Identity.GetUserId();

            var encuesta = db.EncuestaServicios
                .Include(e => e.Cita)
                .Include(e => e.Mecanico)
                .FirstOrDefault(e => e.Id == id && e.ClienteId == userId);

            if (encuesta == null)
                return HttpNotFound();

            if (encuesta.Respondida)
            {
                TempData["EncuestaRespondida"] = "Esta encuesta ya fue respondida.";
                return RedirectToAction("MisCitas", "Cita");
            }

            if (encuesta.Cita == null || encuesta.Cita.Estado != "Terminada")
                return new HttpStatusCodeResult(400, "Solo se pueden responder encuestas de citas terminadas.");

            var model = new ResponderEncuestaViewModel
            {
                EncuestaId = encuesta.Id,
                CitaId = encuesta.CitaId,
                FechaCita = encuesta.Cita.FechaCita,
                Vehiculo = encuesta.Cita.Vehiculo,
                Placa = encuesta.Cita.Placa,
                NombreMecanico = encuesta.Mecanico != null ? encuesta.Mecanico.NombreCompleto : "(Sin mecánico)"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public ActionResult Responder(ResponderEncuestaViewModel model)
        {
            var userId = User.Identity.GetUserId();

            var encuesta = db.EncuestaServicios
                .Include(e => e.Cita)
                .Include(e => e.Mecanico)
                .FirstOrDefault(e => e.Id == model.EncuestaId && e.ClienteId == userId);

            if (encuesta == null)
                return HttpNotFound();

            if (encuesta.Respondida)
            {
                TempData["EncuestaRespondida"] = "Esta encuesta ya fue respondida.";
                return RedirectToAction("MisCitas", "Cita");
            }

            if (encuesta.Cita == null || encuesta.Cita.Estado != "Terminada")
                return new HttpStatusCodeResult(400, "Solo se pueden responder encuestas de citas terminadas.");

            if (!ModelState.IsValid)
            {
                model.CitaId = encuesta.CitaId;
                model.FechaCita = encuesta.Cita.FechaCita;
                model.Vehiculo = encuesta.Cita.Vehiculo;
                model.Placa = encuesta.Cita.Placa;
                model.NombreMecanico = encuesta.Mecanico != null ? encuesta.Mecanico.NombreCompleto : "(Sin mecánico)";
                return View(model);
            }

            encuesta.CalificacionGeneral = model.CalificacionGeneral;
            encuesta.CalificacionMecanico = model.CalificacionMecanico;
            encuesta.RecomendariaTaller = model.RecomendariaTaller;
            encuesta.Comentario = model.Comentario;
            encuesta.FechaRespuesta = DateTime.Now;
            encuesta.Respondida = true;

            db.SaveChanges();

            TempData["EncuestaRespondida"] = "Gracias por completar la encuesta.";
            return RedirectToAction("MisCitas", "Cita");
        }

        //Get resultados encuestas Admin
        [Authorize(Roles = "Admin")]
        public ActionResult ResultadosAdmin()
        {
            var resultados = db.EncuestaServicios
                .AsNoTracking()
                .Include(e => e.Cita)
                .Include(e => e.Cliente)
                .Include(e => e.Mecanico)
                .Where(e => e.Respondida)
                .OrderByDescending(e => e.FechaRespuesta)
                .Select(e => new ResultadoEncuestaAdminViewModel
                {
                    Id = e.Id,
                    CitaId = e.CitaId,
                    Cliente = e.Cliente != null ? e.Cliente.NombreCompleto : e.Cita.NombreCliente,
                    Mecanico = e.Mecanico != null ? e.Mecanico.NombreCompleto : "(Sin mecánico)",
                    Vehiculo = e.Cita.Vehiculo,
                    FechaCita = e.Cita.FechaCita,
                    CalificacionGeneral = e.CalificacionGeneral,
                    CalificacionMecanico = e.CalificacionMecanico,
                    RecomendariaTaller = e.RecomendariaTaller,
                    Comentario = e.Comentario,
                    FechaRespuesta = e.FechaRespuesta
                })
                .ToList();

            ViewBag.TotalRespuestas = resultados.Count;

            var calificacionesGenerales = resultados
                .Where(r => r.CalificacionGeneral.HasValue)
                .Select(r => r.CalificacionGeneral.Value)
                .ToList();

            var calificacionesMecanico = resultados
                .Where(r => r.CalificacionMecanico.HasValue)
                .Select(r => r.CalificacionMecanico.Value)
                .ToList();

            ViewBag.PromedioGeneral = calificacionesGenerales.Any()
                ? calificacionesGenerales.Average().ToString("0.0")
                : "0.0";

            ViewBag.PromedioMecanico = calificacionesMecanico.Any()
                ? calificacionesMecanico.Average().ToString("0.0")
                : "0.0";

            ViewBag.Recomendacion = resultados.Any()
                ? ((double)resultados.Count(r => r.RecomendariaTaller == true) / resultados.Count * 100).ToString("0") + "%"
                : "0%";

            return View(resultados);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}