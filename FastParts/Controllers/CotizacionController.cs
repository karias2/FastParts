using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FastParts.Models;

namespace FastParts.Controllers
{
    public class CotizacionController : Controller
    {
        //// GET: Cotizacion
        //public ActionResult Index()
        //{
        //    return View();
        //}
        private static List<Cotizacion> _cotizaciones = new List<Cotizacion>();
        private static int _nextId = 1;

        public ActionResult Lista()
        {
            return View(_cotizaciones);
        }

        public ActionResult Crear()
        {
            return View(new Cotizacion
            {
                Fecha = DateTime.Today
            });
        }

        [HttpPost]
        public ActionResult Crear(Cotizacion model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _nextId++;
                _cotizaciones.Add(model);
                return RedirectToAction("Lista");
            }
            return View(model);
        }

        public ActionResult Editar(int id)
        {
            var cot = _cotizaciones.FirstOrDefault(c => c.Id == id);
            if (cot == null) return HttpNotFound();
            return View(cot);
        }

        [HttpPost]
        public ActionResult Editar(Cotizacion model)
        {
            var cot = _cotizaciones.FirstOrDefault(c => c.Id == model.Id);
            if (cot == null) return HttpNotFound();

            cot.Cliente = model.Cliente;
            cot.Fecha = model.Fecha;
            cot.ServicioSolicitado = model.ServicioSolicitado;
            cot.Observaciones = model.Observaciones;
            cot.MontoEstimado = model.MontoEstimado;
            cot.Aprobada = model.Aprobada;

            return RedirectToAction("Lista");
        }

        public ActionResult Eliminar(int id)
        {
            var cot = _cotizaciones.FirstOrDefault(c => c.Id == id);
            if (cot == null) return HttpNotFound();
            return View(cot);
        }

        [HttpPost, ActionName("Eliminar")]
        public ActionResult EliminarConfirmado(int id)
        {
            var cot = _cotizaciones.FirstOrDefault(c => c.Id == id);
            if (cot != null) _cotizaciones.Remove(cot);
            return RedirectToAction("Lista");
        }


    }
}