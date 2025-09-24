using FastParts.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace FastParts.Controllers
{
    public class EncuestaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult AdministrarEncuestas()
        {
            var model = db.Encuestas.ToList();
            return View(model);
        }
 
        [HttpGet]
        public async Task<ActionResult> CrearEncuesta(int? id)
        {
            if (id == null)
            {
                var encuesta = new EncuestaModel
                {
                    Preguntas = new List<PreguntaModel>()
                };

                db.Encuestas.Add(encuesta);
                db.SaveChanges();
                return RedirectToAction("CrearEncuesta", new { id = encuesta.ID_Encuesta });
            }
            else
            {
                var encuesta = await db.Encuestas.FindAsync(id);
                encuesta.Preguntas = await db.Preguntas
                    .Where(p => p.ID_Encuesta == id)
                    .ToListAsync();
                //encuesta.TiposDePregunta = ;

                return View(encuesta);
            }
        }

        public ActionResult ActualizarEncuesta(EncuestaModel encuestaModel)
        {
            var encuesta = db.Encuestas.Find(encuestaModel.ID_Encuesta);
            if (encuesta != null)
            {
                encuesta.Nombre = encuestaModel.Nombre;
                encuesta.Descripcion = encuestaModel.Descripcion;
                db.SaveChanges();
                TempData["Ok"] = "Encuesta actualizada.";
                return RedirectToAction("CrearEncuesta", new { id = encuesta.ID_Encuesta });
            }
            else
            {
                TempData["ErrorEdicion"] = "No se encontró la Encuesta.";
                return RedirectToAction("CrearEncuesta");
            }
        }

        public ActionResult CrearPregunta(PreguntaModel preguntaModel)
        {
            if (preguntaModel != null && preguntaModel.ID_Pregunta != null)
            {
                db.Preguntas.Add(preguntaModel);
                db.SaveChanges();
                return RedirectToAction("CrearEncuesta", new { id = preguntaModel.ID_Encuesta });
            }
            else
            {
                return RedirectToAction("AdministrarEncuestas");
            }
        }



        // BORRAR

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
            var encuestas = new List<EncuestaViewModel>
    {
        new EncuestaViewModel
        {

            CalificacionServicio = 5,
            CalificacionTiempo = 4,
            CalificacionTrato = 5,
            ComentariosAdicionales = "Muy buen trato",
            Fecha = DateTime.Now.AddDays(-1)
        },
        new EncuestaViewModel
        {

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