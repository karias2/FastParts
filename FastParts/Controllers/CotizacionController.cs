using FastParts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity; // Required for GetUserManager


namespace FastParts.Controllers
{
    public class CotizacionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public async Task<ActionResult> CotizarRepuesto(RepuestosCotizadosModel repuesto)
        {
            string refererUrl = Request.Headers["Referer"].ToString();
            var userId = User.Identity.GetUserId();
            var cotizacion = db.Cotizaciones
                .Where(c => c.IdCliente == userId)
                .FirstOrDefault();
            ApplicationUser loggedInUser = await UserManager.FindByIdAsync(userId);

            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (cotizacion != null)
            {
                repuesto.IdCotizacion = cotizacion.IdCotizacion;
                db.RepuestosCotizados.Add(repuesto);
                db.SaveChanges();
            }
            else
            {
                var cotizacionesModel = new CotizacionesModel();
                cotizacionesModel.IdCliente = userId;
                db.Cotizaciones.Add(cotizacionesModel);
                db.SaveChanges();
                repuesto.IdCotizacion = cotizacionesModel.IdCotizacion;
                db.RepuestosCotizados.Add(repuesto);
                db.SaveChanges();
            }

            return Redirect(refererUrl);
        }

        [HttpGet]
        public async Task<ActionResult> CotizarServicio(ServiciosCotizadosModel servicio)
        {
            string refererUrl = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(refererUrl))
            {
                // Redirect the user back to the URL specified in the Referer header.
                return Redirect(refererUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}