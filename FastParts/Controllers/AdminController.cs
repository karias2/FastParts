using FastParts.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace FastParts.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.TotalUsuarios = db.Users.Count();
            ViewBag.TotalUsuariosActivos = db.Users.Count(u => u.Estado == true);

            ViewBag.TotalRepuestos = db.Repuestos.Count();
            ViewBag.TotalServicios = db.ServicioModels.Count();

            ViewBag.TotalCitasIngresadas = db.CitaModels.Count(c => c.Estado == "Ingresada");

            ViewBag.AlertasPendientes = db.Alertas.Count(a => !a.Atendida);
            ViewBag.RepuestosBajoMinimo = db.Repuestos.Count(r => !r.IsDeleted && r.Stock <= r.StockMinimo);

            ViewBag.TotalEncuestas = db.EncuestaServicios.Count(e => e.Respondida);
            ViewBag.EncuestasPendientes = db.EncuestaServicios.Count(e => !e.Respondida);

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
        [ValidateAntiForgeryToken]
        public ActionResult MarcarAlertaAtendida(int id)
        {
            var alerta = db.Alertas.Find(id);
            if (alerta == null)
                return HttpNotFound();

            alerta.Atendida = true;
            db.SaveChanges();

            return RedirectToAction("AlertasInventario");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}