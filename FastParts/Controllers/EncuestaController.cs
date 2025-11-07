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


        [HttpGet]
        public async Task<ActionResult> LlenarEncuesta(int? IdEncuesta, String SessionId)
        {
            if (IdEncuesta != null)
            {
                var viewModel = new EncuestaViewModel();
                var encuesta = await db.Encuestas.FindAsync(IdEncuesta);
                encuesta.Preguntas = await db.Preguntas
                        .Where(p => p.ID_Encuesta == IdEncuesta)
                        //.Include(p => p.Respuestas)
                        .ToListAsync();

                if (SessionId != null)
                {
                    var respuestasPrevias = db.Respuestas
                        .Where(r => r.ID_Encuesta == IdEncuesta && r.Session_Id == SessionId)
                        .ToList();

                    foreach (var pregunta in encuesta.Preguntas)
                    {
                        var respuestaEncontrada = respuestasPrevias
                            .OrderByDescending(r => r.ID_Respuesta)
                            .FirstOrDefault(r => r.ID_Pregunta == pregunta.ID_Pregunta);

                        if (respuestaEncontrada != null && respuestaEncontrada.ValorRespuesta != null)
                        {
                            pregunta.ValorRespuesta = respuestaEncontrada.ValorRespuesta;
                        }

                        if (respuestaEncontrada != null && respuestaEncontrada.TextoRespuesta != null)
                        {
                            pregunta.TextoRespuesta = respuestaEncontrada.TextoRespuesta;
                        }
                    }
                }

                viewModel.ID_Encuesta = encuesta.ID_Encuesta;
                viewModel.Encuesta = encuesta;
                viewModel.Pregunta = new PreguntaModel();
                viewModel.Session_Id = SessionId != null ? SessionId : DateTime.Now.Ticks.ToString();

                //return PartialView("_LlenarEncuesta", viewModel);
                return View("Formulario", viewModel);
            } else
            {
                return RedirectToAction("Index");
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

        public ActionResult EliminarEncuesta(EncuestaViewModel viewModel)
        {
            var encuesta = db.Encuestas.Find(viewModel.ID_Encuesta);
            if (encuesta != null)
            {
                //db.Preguntas.Remove(pregunta);
                encuesta.Activa = false;
                db.SaveChanges();
                TempData["Ok"] = "Se ha eliminado la Encuesta correctamente.";
                return RedirectToAction("AdministrarEncuestas");
            }
            else
            {
                TempData["Error"] = "No se encontró la Pregunta a eliminar.";
                return RedirectToAction("AdministrarEncuestas", new { IdEncuesta = viewModel.ID_Encuesta });
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
                //db.Preguntas.Remove(pregunta);
                pregunta.Activa = false;
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

        [HttpPost]
        public ActionResult RespoderPregunta(EncuestaViewModel viewModel)
        {
            try
            {
                var encuestaModel = viewModel.Encuesta;
                if (viewModel.ID_Encuesta != null && viewModel.ID_Pregunta != null)
                {
                    var respuesta = new RespuestasModel();
                    respuesta.ID_Encuesta = viewModel.ID_Encuesta;
                    respuesta.ID_Pregunta = viewModel.ID_Pregunta;
                    respuesta.Session_Id = viewModel.Session_Id;
                    respuesta.Tipo = viewModel.Tipo;

                    if (viewModel.ValorRespuesta != null)
                    {
                        respuesta.ValorRespuesta = viewModel.ValorRespuesta;
                    }

                    if (viewModel.Pregunta != null && viewModel.Pregunta.ValorRespuesta != null)
                    {
                        respuesta.ValorRespuesta = viewModel.Pregunta.ValorRespuesta;
                    }

                    if (viewModel.TextoRespuesta != null)
                    {
                        respuesta.TextoRespuesta = viewModel.TextoRespuesta;
                    }

                    if (viewModel.Pregunta != null && viewModel.Pregunta.TextoRespuesta != null)
                    {
                        respuesta.TextoRespuesta = viewModel.Pregunta.TextoRespuesta;
                    }

                    db.Respuestas.Add(respuesta);
                    db.SaveChanges();
                    return RedirectToAction("LlenarEncuesta", new { 
                        IdEncuesta = viewModel.ID_Encuesta, 
                        IdPregunta = viewModel.ID_Pregunta, 
                        SessionId = viewModel.Session_Id
                    });
                }
                else
                {
                    TempData["Error"] = "No se encontró la Encuesta.";
                    return Json(new { success = false, message = $"error: No se encontró Encuesta." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"error: {ex.Message}" });
            }
        }

    }
}