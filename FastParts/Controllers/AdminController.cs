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

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}