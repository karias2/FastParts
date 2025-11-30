using Microsoft.AspNet.Identity;
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
    public class UserRolesController : Controller
    {
    //    private ApplicationUserManager _userManager;
    //    private RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole> _roleManager;

    //    public ApplicationUserManager UserManager
    //        => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
    //    public RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole> RoleManager
    //        => _roleManager ?? HttpContext.GetOwinContext().Get<RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>>();

    //    // LISTA DE USUARIOS
    //    public ActionResult Index()
    //    {
    //        var users = UserManager.Users.ToList();
    //        return View(users);
    //    }

    //    // EDITAR ROLES DE UN USUARIO
    //    public async Task<ActionResult> Edit(string id)
    //    {
    //        var user = await UserManager.FindByIdAsync(id);
    //        if (user == null) return HttpNotFound();

    //        var allRoles = RoleManager.Roles.Select(r => r.Name).ToList();
    //        var userRoles = await UserManager.GetRolesAsync(user.Id);

    //        var vm = new EditUserRolesViewModel
    //        {
    //            UserId = user.Id,
    //            Email = user.Email,
    //            Roles = allRoles.Select(r => new RoleCheck { Name = r, Assigned = userRoles.Contains(r) }).ToList()
    //        };
    //        return View(vm);
    //    }

    //    [HttpPost, ValidateAntiForgeryToken]
    //    public async Task<ActionResult> Edit(EditUserRolesViewModel model)
    //    {
    //        var user = await UserManager.FindByIdAsync(model.UserId);
    //        if (user == null) return HttpNotFound();

    //        var currentRoles = await UserManager.GetRolesAsync(user.Id);
    //        var selected = model.Roles?.Where(x => x.Assigned).Select(x => x.Name).ToArray() ?? new string[] { };

    //        // Roles a agregar y a quitar
    //        var toAdd = selected.Except(currentRoles).ToArray();
    //        var toRemove = currentRoles.Except(selected).ToArray();

    //        if (toAdd.Any())
    //        {
    //            var addRes = await UserManager.AddToRolesAsync(user.Id, toAdd);
    //            if (!addRes.Succeeded)
    //            {
    //                ModelState.AddModelError("", string.Join("; ", addRes.Errors));
    //                return await Edit(model.UserId); // recargar
    //            }
    //        }
    //        if (toRemove.Any())
    //        {
    //            var remRes = await UserManager.RemoveFromRolesAsync(user.Id, toRemove);
    //            if (!remRes.Succeeded)
    //            {
    //                ModelState.AddModelError("", string.Join("; ", remRes.Errors));
    //                return await Edit(model.UserId);
    //            }
    //        }

    //        TempData["Msg"] = "Roles actualizados.";
    //        return RedirectToAction("Index");
    //    }
    //}

    //public class EditUserRolesViewModel
    //{
    //    public string UserId { get; set; }
    //    public string Email { get; set; }
    //    public List<RoleCheck> Roles { get; set; } = new List<RoleCheck>();
    //}
    //public class RoleCheck
    //{
    //    public string Name { get; set; }
    //    public bool Assigned { get; set; }
    }
}