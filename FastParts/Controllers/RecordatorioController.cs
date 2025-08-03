using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FastParts.Models;

namespace FastParts.Controllers
{
    public class RecordatorioController : Controller
    {
        private static List<Recordatorio> _recordatorios = new List<Recordatorio>(); // simulación
        // GET: Recordatorio
        public ActionResult ListaRecordatorios()
        {
            return View(_recordatorios);
        }

        public ActionResult Crear()
        {
            var modelo = new Recordatorio
            {
                FechaServicio = DateTime.Today
            };
            return View(modelo);
        }

        [HttpPost]
        public ActionResult Crear(Recordatorio model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _recordatorios.Count + 1;
                _recordatorios.Add(model);
                return RedirectToAction("ListaRecordatorios");
            }
            return View(model);
        }

        public ActionResult Editar(int id)
        {
            var recordatorio = _recordatorios.FirstOrDefault(r => r.Id == id);
            if (recordatorio == null) return HttpNotFound();
            return View(recordatorio);
        }

        [HttpPost]
        public ActionResult Editar(Recordatorio model)
        {
            var original = _recordatorios.FirstOrDefault(r => r.Id == model.Id);
            if (original != null && ModelState.IsValid)
            {
                original.Descripcion = model.Descripcion;
                original.FechaServicio = model.FechaServicio;
                original.Activo = model.Activo;
                return RedirectToAction("ListaRecordatorios");
            }
            return View(model);
        }
    }
}
