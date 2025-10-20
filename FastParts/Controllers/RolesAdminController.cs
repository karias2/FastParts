using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace FastParts.Models
{
    //[Authorize(Roles = "Admin")]
    public class RolesAdminController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        public RoleManager<IdentityRole> RoleManager
            => _roleManager ?? HttpContext.GetOwinContext().Get<RoleManager<IdentityRole>>();

        // LISTAR
        public ActionResult Index()
        {
            var roles = RoleManager.Roles.ToList();
            return View(roles);
        }

        // CREAR
        public ActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("", "El nombre del rol es requerido.");
                return View();
            }

            if (await RoleManager.RoleExistsAsync(name))
            {
                ModelState.AddModelError("", "El rol ya existe.");
                return View();
            }

            var result = await RoleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded) return RedirectToAction("Index");

            ModelState.AddModelError("", string.Join("; ", result.Errors));
            return View();
        }

        // EDITAR (renombrar)
        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null) return HttpNotFound();
            return View(role);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, string name)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null) return HttpNotFound();

            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("", "El nombre del rol es requerido.");
                return View(role);
            }

            role.Name = name;
            var result = await RoleManager.UpdateAsync(role);
            if (result.Succeeded) return RedirectToAction("Index");

            ModelState.AddModelError("", string.Join("; ", result.Errors));
            return View(role);
        }

        // ELIMINAR
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null) return HttpNotFound();
            return View(role);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null) return HttpNotFound();

            var result = await RoleManager.DeleteAsync(role);
            if (result.Succeeded) return RedirectToAction("Index");

            ModelState.AddModelError("", string.Join("; ", result.Errors));
            return View(role);
        }
    }
}
