using FastParts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastParts.Controllers
{
    public class EncuestaController : Controller
    {
        // GET: Encuesta
        [HttpGet]
        public ActionResult Crear()
        {
            return View(new EncuestaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(EncuestaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Simula guardar en base de datos (o mostrar confirmación)
                TempData["Mensaje"] = "Gracias por tu opinión.";
                return RedirectToAction("Gracias");
            }

            return View(model);
        }

        public ActionResult Gracias()
        {
            return View();
        }

        public ActionResult VerTodas()
        {
            // Simulación de encuestas obtenidas de base de datos o memoria
            var encuestas = new List<EncuestaResumenViewModel>
    {
        new EncuestaResumenViewModel
        {
            Id = 1,
            CalificacionServicio = 5,
            CalificacionTiempo = 4,
            CalificacionTrato = 5,
            ComentariosAdicionales = "Muy buen trato",
            Fecha = DateTime.Now.AddDays(-1)
        },
        new EncuestaResumenViewModel
        {
            Id = 2,
            CalificacionServicio = 3,
            CalificacionTiempo = 3,
            CalificacionTrato = 4,
            ComentariosAdicionales = "Servicio aceptable",
            Fecha = DateTime.Now.AddDays(-2)
        }
    };

            return View(encuestas);
        }

    }
}