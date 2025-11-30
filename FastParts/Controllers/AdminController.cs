using FastParts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastParts.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            int totalUsuarios = db.Users.Count();
            ViewBag.TotalUsuarios = totalUsuarios;

            int totalUsuariosActivos = db.Users.Count(u => u.Estado == true);
            ViewBag.TotalUsuariosActivos = totalUsuariosActivos;

            int totalRepuestos = db.Repuestos.Count();
            ViewBag.TotalRepuestos = totalRepuestos;

            int totalCitasIngresadas = db.CitaModels.Count(c => c.Estado == "Ingresada");
            ViewBag.TotalCitasIngresadas = totalCitasIngresadas;
            return View();

        }
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