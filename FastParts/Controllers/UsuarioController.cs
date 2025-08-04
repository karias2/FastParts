using FastParts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastParts.Controllers
{
    public class UsuarioController : Controller
    {
        //// GET: Usuario
        //public ActionResult Index()
        //{
        //    return View();
        //}

        private static List<UsuarioViewModel> _usuarios = new List<UsuarioViewModel>
        {
            new UsuarioViewModel { Id = 1, NombreUsuario = "cliente1", Correo = "cliente1@correo.com", Roles = new List<string>{ "Cliente" } },
            new UsuarioViewModel { Id = 2, NombreUsuario = "mecanico1", Correo = "mecanico1@correo.com", Roles = new List<string>{ "Mecanico" } },
            new UsuarioViewModel { Id = 3, NombreUsuario = "admin", Correo = "admin@correo.com", Roles = new List<string>{ "Administrador" } }
        };

        public ActionResult Lista()
        {
            return View(_usuarios);
        }

        public ActionResult Crear()
        {
            ViewBag.Roles = RolesDisponibles.Todos;
            return View(new UsuarioViewModel());
        }

        [HttpPost]
        public ActionResult Crear(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _usuarios.Max(u => u.Id) + 1;
                _usuarios.Add(model);
                return RedirectToAction("Lista");
            }

            ViewBag.Roles = RolesDisponibles.Todos;
            return View(model);
        }

        public ActionResult Editar(int id)
        {
            var user = _usuarios.FirstOrDefault(u => u.Id == id);
            if (user == null) return HttpNotFound();

            ViewBag.Roles = RolesDisponibles.Todos;
            return View(user);
        }

        [HttpPost]
        public ActionResult Editar(UsuarioViewModel model)
        {
            var user = _usuarios.FirstOrDefault(u => u.Id == model.Id);
            if (user != null && ModelState.IsValid)
            {
                user.NombreUsuario = model.NombreUsuario;
                user.Correo = model.Correo;
                user.Roles = model.Roles ?? new List<string>();
                return RedirectToAction("Lista");
            }

            ViewBag.Roles = RolesDisponibles.Todos;
            return View(model);
        }

        public ActionResult Eliminar(int id)
        {
            var user = _usuarios.FirstOrDefault(u => u.Id == id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        [HttpPost, ActionName("Eliminar")]
        public ActionResult EliminarConfirmado(int id)
        {
            var user = _usuarios.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _usuarios.Remove(user);
            }
            return RedirectToAction("Lista");
        }
    }
}