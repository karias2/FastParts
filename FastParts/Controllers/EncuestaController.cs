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
                var viewModel = new EncuestaViewModel();
                var encuesta = await db.Encuestas.FindAsync(id);
                encuesta.Preguntas = await db.Preguntas
                    .Where(p => p.ID_Encuesta == id)
                    .ToListAsync();

                var TiposDePregunta = new List<System.Web.Mvc.SelectListItem>();
                TiposDePregunta.Add(new System.Web.Mvc.SelectListItem { Value = "Rango", Text = "Rango" });
                TiposDePregunta.Add(new System.Web.Mvc.SelectListItem { Value = "Texto", Text = "Párrafo" });
                TiposDePregunta.Add(new System.Web.Mvc.SelectListItem { Value = "OpcionMultiple", Text = "Opcion multiple" });
                TiposDePregunta.Add(new System.Web.Mvc.SelectListItem { Value = "CasillasVerificacion", Text = "Casillas de merificacion" });

                viewModel.ID_Encuesta = encuesta.ID_Encuesta;
                viewModel.Encuesta = encuesta;
                viewModel.Pregunta = new PreguntaModel();
                viewModel.TiposDePregunta = TiposDePregunta;

                return View(viewModel);
            }
        }

        public ActionResult ActualizarEncuesta(EncuestaViewModel viewModel)
        {
            var encuestaModel = viewModel.Encuesta;
            var encuesta = db.Encuestas.Find(viewModel.ID_Encuesta);
            if (encuesta != null)
            {
                encuesta.Nombre = encuestaModel.Nombre;
                encuesta.Descripcion = encuestaModel.Descripcion;
                db.SaveChanges();
                TempData["Ok"] = "Encuesta actualizada.";
                return RedirectToAction("CrearEncuesta", new { id = viewModel.ID_Encuesta });
            }
            else
            {
                TempData["ErrorEdicion"] = "No se encontró la Encuesta.";
                return RedirectToAction("CrearEncuesta");
            }
        }

        public ActionResult CrearPregunta(EncuestaViewModel viewModel)
        {
            var preguntaModel = viewModel.Pregunta;
            if (preguntaModel != null && viewModel.ID_Encuesta != null)
            {
                preguntaModel.ID_Encuesta = viewModel.ID_Encuesta;
                db.Preguntas.Add(preguntaModel);
                db.SaveChanges();
                return RedirectToAction("CrearEncuesta", new { id = viewModel.ID_Encuesta });
            }
            else
            {
                return RedirectToAction("CrearEncuesta", new { id = viewModel.ID_Encuesta });
            }
        }
    }
}