using FastParts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastParts.Controllers
{
    // Permiso para acceder panel de administrador cuando solo si pertenece a rol "Admin".
    //Habilitar despues de pruebas.
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult AlertasInventario()
        {
            var vm = new AlertasInventarioVM
            {
                BajoMinimo = db.Repuestos
                    .AsNoTracking()
                    .Where(r => !r.IsDeleted && r.Stock <= r.StockMinimo)
                    .OrderBy(r => r.Nombre)
                    .ToList(),

                Alertas = db.Alertas
                    .AsNoTracking()
                    .Where(a => !a.Atendida)
                    .OrderByDescending(a => a.Fecha)
                    .ToList()
            };

            return View(vm);
        }


        [HttpPost]
        public ActionResult MarcarAlertaAtendida(int id)
        {
            var a = db.Alertas.Find(id);
            a.Atendida = true;
            db.SaveChanges();
            return RedirectToAction("AlertasInventario");
        }
    }
}