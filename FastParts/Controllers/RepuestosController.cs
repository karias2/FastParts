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
    [Authorize(Roles = "Admin")]
    public class RepuestosController : Controller
    {
        private const int PageSize = 20;
        private const int MaxImageBytes = 10 * 1024 * 1024; // 10 MB
        private const string LegacyUploadsPrefix = "/Content/uploads/repuestos/";

        // TODO: ALLOW ONLY ADMIN USERS TO CREATE, MODIFY AND DELETE THIS RESOURCE

        private readonly ApplicationDbContext db = new ApplicationDbContext();

        private string GetRepuestosStorageDirectory()
        {
            // In Azure App Service, D:\home is persistent across deployments.
            var basePath = Environment.GetEnvironmentVariable("HOME");
            if (!string.IsNullOrWhiteSpace(basePath))
            {
                return Path.Combine(basePath, "data", "FastParts", "uploads", "repuestos");
            }

            // Local dev fallback.
            return Server.MapPath("~/App_Data/uploads/repuestos");
        }

        private string BuildImageUrl(string fileName)
        {
            return Url.Action("Imagen", "Repuestos", new { fileName = fileName });
        }

        private string GetImageFileNameFromUrl(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return null;
            }

            if (imageUrl.StartsWith(LegacyUploadsPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return Path.GetFileName(imageUrl);
            }

            var uriPart = imageUrl;
            var queryIdx = uriPart.IndexOf("?", StringComparison.Ordinal);
            if (queryIdx >= 0)
            {
                uriPart = uriPart.Substring(0, queryIdx);
            }

            var segment = uriPart.Split('/').LastOrDefault();
            if (string.IsNullOrWhiteSpace(segment))
            {
                return null;
            }

            return segment;
        }

        private string NormalizeImageUrl(string imageUrl)
        {
            var fileName = GetImageFileNameFromUrl(imageUrl);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return imageUrl;
            }

            return BuildImageUrl(fileName);
        }

        private string SaveUploadedImage(System.Web.HttpPostedFileBase imageFile, string extension)
        {
            var storageDir = GetRepuestosStorageDirectory();
            Directory.CreateDirectory(storageDir);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(storageDir, fileName);
            imageFile.SaveAs(fullPath);
            return fileName;
        }

        private void TryDeleteImageByUrl(string imageUrl)
        {
            var fileName = GetImageFileNameFromUrl(imageUrl);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            var storageFile = Path.Combine(GetRepuestosStorageDirectory(), fileName);
            if (System.IO.File.Exists(storageFile))
            {
                System.IO.File.Delete(storageFile);
                return;
            }

            // Legacy fallback path.
            var legacyVirtualPath = $"{LegacyUploadsPrefix}{fileName}";
            var legacyFile = Server.MapPath(legacyVirtualPath);
            if (System.IO.File.Exists(legacyFile))
            {
                System.IO.File.Delete(legacyFile);
            }
        }

        // GET: /Repuesto
        public async Task<ActionResult> Index(string q, string sort = "nombre", int page = 1)
        {
            q = (q ?? string.Empty).Trim();

            var query = db.Repuestos
                .AsNoTracking()
                .Where(r => !r.IsDeleted);

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

            var safePage = page < 1 ? 1 : page;
            var total = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling((double)total / PageSize));
            if (safePage > totalPages) safePage = totalPages;

            var list = await query
                .Skip((safePage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            foreach (var repuesto in list)
            {
                repuesto.ImagenUrl = NormalizeImageUrl(repuesto.ImagenUrl);
            }

            ViewBag.Page = safePage;
            ViewBag.TotalPages = totalPages;
            ViewBag.Q = q;
            ViewBag.Sort = sort;
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

            model.Nombre = (model.Nombre ?? string.Empty).Trim();
            model.NumeroParte = (model.NumeroParte ?? string.Empty).Trim();

            if (model.Precio <= 0)
            {
                ModelState.AddModelError("Precio", "El precio debe ser mayor que 0.");
            }

            if (model.Stock <= 0)
            {
                ModelState.AddModelError("Stock", "El stock debe ser mayor que 0.");
            }

            if (!string.IsNullOrWhiteSpace(model.NumeroParte))
            {
                var existsCode = await db.Repuestos.AnyAsync(r =>
                    !r.IsDeleted &&
                    (r.NumeroParte ?? "").ToLower() == model.NumeroParte.ToLower());
                if (existsCode)
                {
                    ModelState.AddModelError("NumeroParte", "El código ya existe. Ingrese un código único.");
                }
            }

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

                if (model.ImagenFile.ContentLength > MaxImageBytes)
                {
                    ModelState.AddModelError("ImagenFile", "La imagen excede el tamaño máximo permitido (10 MB).");
                    return View(model);
                }

                try
                {
                    var fileName = SaveUploadedImage(model.ImagenFile, ext);
                    model.ImagenUrl = BuildImageUrl(fileName);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("ImagenFile", "No fue posible guardar la imagen. Intente con otro archivo.");
                    return View(model);
                }
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
            repuesto.ImagenUrl = NormalizeImageUrl(repuesto.ImagenUrl);
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
                entity.Nombre = (entity.Nombre ?? string.Empty).Trim();
                entity.NumeroParte = (entity.NumeroParte ?? string.Empty).Trim();

                if (entity.Precio <= 0)
                {
                    ModelState.AddModelError("Precio", "El precio debe ser mayor que 0.");
                }

                if (entity.Stock <= 0)
                {
                    ModelState.AddModelError("Stock", "El stock debe ser mayor que 0.");
                }

                if (!string.IsNullOrWhiteSpace(entity.NumeroParte))
                {
                    var existsCode = await db.Repuestos.AnyAsync(r =>
                        r.Id != entity.Id &&
                        !r.IsDeleted &&
                        (r.NumeroParte ?? "").ToLower() == entity.NumeroParte.ToLower());
                    if (existsCode)
                    {
                        ModelState.AddModelError("NumeroParte", "El código ya existe. Ingrese un código único.");
                    }
                }

                if (!ModelState.IsValid)
                {
                    return View(entity);
                }

                if (form.ImagenFile != null && form.ImagenFile.ContentLength > 0)
                {
                    var okExt = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var ext = Path.GetExtension(form.ImagenFile.FileName)?.ToLowerInvariant();
                    if (!okExt.Contains(ext ?? ""))
                    {
                        ModelState.AddModelError("", "Formato de imagen no permitido.");
                        return View(entity);
                    }

                    if (form.ImagenFile.ContentLength > MaxImageBytes)
                    {
                        ModelState.AddModelError("ImagenFile", "La imagen excede el tamaño máximo permitido (10 MB).");
                        return View(entity);
                    }

                    try
                    {
                        var fileName = SaveUploadedImage(form.ImagenFile, ext);
                        TryDeleteImageByUrl(entity.ImagenUrl);
                        entity.ImagenUrl = BuildImageUrl(fileName);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("ImagenFile", "No fue posible guardar la imagen. Intente con otro archivo.");
                        return View(entity);
                    }
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
            repuesto.ImagenUrl = NormalizeImageUrl(repuesto.ImagenUrl);

            return View(repuesto);
        }

        [AllowAnonymous]
        public ActionResult Imagen(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return HttpNotFound();
            }

            fileName = Path.GetFileName(fileName);
            var storageFile = Path.Combine(GetRepuestosStorageDirectory(), fileName);
            if (!System.IO.File.Exists(storageFile))
            {
                // Legacy fallback.
                var legacyFile = Server.MapPath($"{LegacyUploadsPrefix}{fileName}");
                if (!System.IO.File.Exists(legacyFile))
                {
                    return HttpNotFound();
                }

                storageFile = legacyFile;
            }

            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
            var contentType = "application/octet-stream";
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".webp":
                    contentType = "image/webp";
                    break;
            }

            return File(storageFile, contentType);
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
        [ValidateAntiForgeryToken]
        public ActionResult ToggleOcultar(int id)
        {
            var r = db.Repuestos.Find(id);
            if (r == null) return HttpNotFound();
            r.OcultarClientes = !r.OcultarClientes;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleForzarSinStock(int id)
        {
            var r = db.Repuestos.Find(id);
            if (r == null) return HttpNotFound();
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