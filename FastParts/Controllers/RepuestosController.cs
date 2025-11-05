using FastParts.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FastParts.Controllers
{
    //[Authorize]
    public class RepuestosController : Controller
    {

        // TODO: ALLOW ONLY ADMIN USERS TO CREATE, MODIFY AND DELETE THIS RESOURCE

        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Repuesto
        public async Task<ActionResult> Index(string q, string sort = "nombre")
        {
            q = (q ?? string.Empty).Trim();

            var query = db.Repuestos
                .AsNoTracking()
                .Where(r => !r.IsDeleted
                            && !r.OcultarClientes
                            && !r.SinStockForzado
                            && r.Stock > 0);

            if (q.Length > 0)
            {
                query = query.Where(r =>
                    r.Nombre.Contains(q) ||
                    (r.Marca ?? "").Contains(q) ||
                    (r.NumeroParte ?? "").Contains(q) ||
                    (r.Proveedor ?? "").Contains(q));
            }

            switch ((sort ?? "nombre").ToLowerInvariant())
            {
                case "precio":
                    query = query.OrderBy(r => r.Precio);
                    break;
                case "stock":
                    query = query.OrderByDescending(r => r.Stock);
                    break;
                default:
                    query = query.OrderBy(r => r.Nombre);
                    break;
            }

            var list = await query.ToListAsync();
            return View(list);
        }


        // GET: Repuestos/Create
        //[Authorize]
        public ActionResult Create() => View();

        // POST: Repuestos/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RepuestoModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Guardar imagen si viene
            if (model.ImagenFile != null && model.ImagenFile.ContentLength > 0)
            {
                var okExt = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var ext = Path.GetExtension(model.ImagenFile.FileName)?.ToLowerInvariant();
                if (!okExt.Contains(ext ?? ""))
                {
                    ModelState.AddModelError("", "Formato de imagen no permitido.");
                    return View(model);
                }

                var dir = Server.MapPath("~/Content/uploads/repuestos");
                Directory.CreateDirectory(dir);
                var fileName = $"{System.Guid.NewGuid()}{ext}";
                var fullPath = Path.Combine(dir, fileName);
                model.ImagenFile.SaveAs(fullPath);

                model.ImagenUrl = $"/Content/uploads/repuestos/{fileName}";
            }

            db.Repuestos.Add(model);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Repuestos/Edit/5
        //[Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var repuesto = await db.Repuestos.FindAsync(id);
            if (repuesto == null) return HttpNotFound();
            return View(repuesto);
        }

        // POST: Repuestos/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, RepuestoModel form)
        {
            var entity = await db.Repuestos.FindAsync(id);
            if (entity == null) return HttpNotFound();

            if (TryUpdateModel(entity, new[] { "Nombre", "Marca", "NumeroParte", "Precio", "Stock", "Proveedor", "Descripcion" }))
            {
                if (form.ImagenFile != null && form.ImagenFile.ContentLength > 0)
                {
                    var okExt = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var ext = Path.GetExtension(form.ImagenFile.FileName)?.ToLowerInvariant();
                    if (!okExt.Contains(ext ?? ""))
                    {
                        ModelState.AddModelError("", "Formato de imagen no permitido.");
                        return View(entity);
                    }

                    var dir = Server.MapPath("~/Content/uploads/repuestos");
                    Directory.CreateDirectory(dir);
                    var fileName = $"{System.Guid.NewGuid()}{ext}";
                    var fullPath = Path.Combine(dir, fileName);
                    form.ImagenFile.SaveAs(fullPath);

                    if (!string.IsNullOrWhiteSpace(entity.ImagenUrl))
                    {
                        var old = Server.MapPath(entity.ImagenUrl);
                        if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                    }

                    entity.ImagenUrl = $"/Content/uploads/repuestos/{fileName}";
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(entity);
        }

        //[Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            // Find only if it's NOT deleted
            var repuesto = await db.Repuestos
                                   .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            if (repuesto == null) return HttpNotFound();

            return View(repuesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var repuesto = await db.Repuestos.FindAsync(id);
            if (repuesto == null) return HttpNotFound();

            if (repuesto.IsDeleted)
            {
                TempData["OkMsg"] = "El repuesto ya estaba eliminado.";
                return RedirectToAction("Index");
            }

            repuesto.IsDeleted = true;
            repuesto.DeletedAt = DateTime.UtcNow; 

            try
            {
                await db.SaveChangesAsync();
                TempData["OkMsg"] = "Repuesto eliminado (soft delete).";
            }
            catch (DbUpdateException)
            {
                TempData["ErrMsg"] = "No se puede marcar como eliminado.";
                return RedirectToAction("Delete", new { id });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleOcultar(int id)
        {
            var r = db.Repuestos.Find(id);
            r.OcultarClientes = !r.OcultarClientes;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleForzarSinStock(int id)
        {
            var r = db.Repuestos.Find(id);
            r.SinStockForzado = !r.SinStockForzado;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}