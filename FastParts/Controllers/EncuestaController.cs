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
        public async Task<ActionResult> CrearEncuesta(int? IdEncuesta, int? IdPregunta)
        {
            if (IdEncuesta == null)
            {
                var encuesta = new EncuestaModel
                {
                    Preguntas = new List<PreguntaModel>()
                };

                db.Encuestas.Add(encuesta);
                db.SaveChanges();
                return RedirectToAction("CrearEncuesta", new { IdEncuesta = encuesta.ID_Encuesta });
            }
            else
            {
                var viewModel = new EncuestaViewModel();
                var encuesta = await db.Encuestas.FindAsync(IdEncuesta);
                encuesta.Preguntas = await db.Preguntas
                    .Where(p => p.ID_Encuesta == IdEncuesta)
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

                if (IdPregunta != null)
                {
                    var pregunta = db.Preguntas.Find(IdPregunta);
                    viewModel.ID_Pregunta = pregunta.ID_Pregunta;
                    viewModel.Pregunta = pregunta;
                }

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
                return RedirectToAction("CrearEncuesta", new { IdEncuesta = viewModel.ID_Encuesta });
            }
            else
            {
                TempData["Error"] = "No se encontró la Encuesta.";
                return RedirectToAction("CrearEncuesta");
            }
        }

        public ActionResult CrearPregunta(EncuestaViewModel viewModel)
        {
            var preguntaModel = viewModel.Pregunta;
            if (preguntaModel != null && viewModel.ID_Encuesta != null)
            {
                if (viewModel.ID_Pregunta != null)
                {
                    var pregunta = db.Preguntas.Find(viewModel.ID_Pregunta);
                    pregunta.Descripcion = viewModel.Pregunta.Descripcion;
                    pregunta.Tipo = viewModel.Pregunta.Tipo;
                    pregunta.Minimo = viewModel.Pregunta.Minimo;
                    pregunta.Maximo = viewModel.Pregunta.Maximo;
                    pregunta.Opciones = viewModel.Pregunta.Opciones;
                    db.SaveChanges();
                    return RedirectToAction("CrearEncuesta", new { IdEncuesta = viewModel.ID_Encuesta });
                } 
                else
                {
                    preguntaModel.ID_Encuesta = viewModel.ID_Encuesta;
                    db.Preguntas.Add(preguntaModel);
                    db.SaveChanges();
                    return RedirectToAction("CrearEncuesta", new { IdEncuesta = viewModel.ID_Encuesta });
                }
            }
            else
            {
                return RedirectToAction("CrearEncuesta", new { IdEncuesta = viewModel.ID_Encuesta });
            }
        }

        public ActionResult EditarPregunta(EncuestaViewModel viewModel)
        {
            var pregunta = db.Preguntas.Find(viewModel.ID_Pregunta);
            if (pregunta != null)
            {
                TempData["Ok"] = "Encuesta actualizada.";
                return RedirectToAction("CrearEncuesta", new { IdEncuesta = pregunta.ID_Encuesta, IdPregunta = pregunta.ID_Pregunta });
            }
            else
            {
                TempData["Error"] = "No se encontró la Encuesta.";
                return RedirectToAction("CrearEncuesta");
            }
        }


        public ActionResult EliminarPregunta(EncuestaViewModel viewModel)
        {
            var pregunta = db.Preguntas.Find(viewModel.ID_Pregunta);
            if (pregunta != null)
            {
                db.Preguntas.Remove(pregunta);
                db.SaveChanges();
                TempData["Ok"] = "Se ha eliminado la pregunta correctamente.";
                return RedirectToAction("CrearEncuesta", new { IdEncuesta = pregunta.ID_Encuesta, IdPregunta = pregunta.ID_Pregunta });
            }
            else
            {
                TempData["Error"] = "No se encontró la Pregunta a eliminar.";
                return RedirectToAction("CrearEncuesta");
            }
        }
    }
}