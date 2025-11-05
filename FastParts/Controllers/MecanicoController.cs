using FastParts.Models;
using FastParts.Services;
using System.Linq;
using System;
using System.Web.Mvc;
using System.Net;

namespace FastParts.Controllers
{
    public class MecanicoController : Controller
    {

        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly IInventarioService inv;

        public MecanicoController()
        {
            inv = new InventarioService(new ApplicationDbContext()); 
        }

        public ActionResult IngresoVehiculo()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DetallesServicio(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var servicioId = id.Value;

            var consumos = db.ServicioRepuestos
                             .Include("Repuesto")
                             .Where(x => x.ServicioId == servicioId)
                             .ToList();

            var vm = new ConsumosServicioVM
            {
                ServicioId = servicioId,
                Consumos = consumos
            };

            ViewBag.Repuestos = new SelectList(
                db.Repuestos
                  .Where(r => !r.IsDeleted && !r.OcultarClientes && !r.SinStockForzado && r.Stock > 0)
                  .OrderBy(r => r.Nombre)
                  .ToList(),
                "Id", "Nombre"
            );

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarConsumo(int servicioId, int[] repuestoId, int[] cantidad)
        {
            var items = repuestoId.Zip(cantidad, (rid, cant) => (rid, cant));
            try
            {
                inv.RegistrarConsumo(servicioId, items, User?.Identity?.Name);
                TempData["ok"] = "Consumo registrado y stock actualizado.";
            }
            catch (Exception ex)
            {
                TempData["err"] = ex.Message;
            }
            return RedirectToAction("DetallesServicio", new { id = servicioId });
        }
    }
}
