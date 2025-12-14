using FastParts.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace FastParts.Controllers
{
    public class ClienteController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Perfil()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var model = new PerfilViewModel
            {
                NombreCompleto = user.NombreCompleto,
                Direccion = user.Direccion,
                Email = user.Email,
                Telefono = user.PhoneNumber,
            };

            return View(model);
        }

        public ActionResult EditarPerfil()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var model = new EditarPerfilViewModel
            {
                NombreCompleto = user.NombreCompleto,
                Direccion = user.Direccion,
                Email = user.Email,
                Telefono = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPerfil(EditarPerfilViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            user.NombreCompleto = model.NombreCompleto;
            user.Direccion = model.Direccion;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneNumber = model.Telefono;

            db.SaveChanges();

            return RedirectToAction("Perfil");
        }

    }
}