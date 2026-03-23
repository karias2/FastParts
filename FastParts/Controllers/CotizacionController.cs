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
using System.Data.Entity;

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

        // Ver el carrito de cotización del cliente
        [Authorize(Roles = "Cliente")]
        public ActionResult MiCarrito()
        {
            var userId = User.Identity.GetUserId();

            var cotizacion = db.Cotizaciones
                .Where(c => c.IdCliente == userId && c.Estado == "ACTIVA")
                .FirstOrDefault();

            var vm = new MiCarritoViewModel();

            if (cotizacion == null)
            {
                return View(vm);
            }

            vm.IdCotizacion = cotizacion.IdCotizacion;

            vm.Repuestos = db.RepuestosCotizados
                .Where(rc => rc.IdCotizacion == cotizacion.IdCotizacion)
                .Join(db.Repuestos, rc => rc.IdRepuesto, r => r.Id, (rc, r) => new { rc, r })
                .GroupBy(x => new { x.r.Id, x.r.Nombre, x.r.Marca, x.r.Precio })
                .Select(g => new CarritoItemRepuestoVM
                {
                    IdRepuesto = g.Key.Id,
                    Nombre = g.Key.Nombre,
                    Marca = g.Key.Marca,
                    PrecioUnitario = g.Key.Precio,
                    Cantidad = g.Count()
                })
                .OrderBy(x => x.Nombre)
                .ToList();

            vm.Servicios = db.ServiciosCotizados
                .Where(sc => sc.IdCotizacion == cotizacion.IdCotizacion)
                .Join(db.ServicioModels, sc => sc.IdServicio, s => s.IdServicio, (sc, s) => new { sc, s })
                .GroupBy(x => new { x.s.IdServicio, x.s.Nombre, x.s.PrecioServicio })
                .Select(g => new CarritoItemServicioVM
                {
                    IdServicio = g.Key.IdServicio,
                    Nombre = g.Key.Nombre,
                    PrecioUnitario = g.Key.PrecioServicio,
                    Cantidad = g.Count()
                })
                .OrderBy(x => x.Nombre)
                .ToList();

            vm.TotalRepuestos = vm.Repuestos.Sum(x => x.Subtotal);
            vm.TotalServicios = vm.Servicios.Sum(x => x.Subtotal);
            cotizacion.MontoTotal = vm.Total;
            db.SaveChanges();

            return View(vm);
        }

        // Agregar repuesto al carrito
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public ActionResult AgregarRepuesto(int idRepuesto)
        {
            var userId = User.Identity.GetUserId();

            // Verificar que el repuesto existe y está disponible
            var repuesto = db.Repuestos.Find(idRepuesto);
            if (repuesto == null || repuesto.IsDeleted || repuesto.OcultarClientes || repuesto.SinStockForzado || repuesto.Stock <= 0)
            {
                TempData["Error"] = "El repuesto no está disponible.";
                return RedirectToAction("CatalogoRepuestos");
            }

            // Obtener o crear cotización activa
            var cotizacion = db.Cotizaciones
              .FirstOrDefault(c => c.IdCliente == userId && c.Estado == "ACTIVA");

            if (cotizacion == null)
            {
                cotizacion = new CotizacionesModel
                {
                    IdCliente = userId,
                    Estado = "ACTIVA",
                    FechaCreacion = DateTime.Now,
                    FechaCita = DateTime.Now.AddDays(1),
                    MontoTotal = 0
                };
                db.Cotizaciones.Add(cotizacion);
                db.SaveChanges();
            }

            var cantidadActual = db.RepuestosCotizados
                .Count(rc => rc.IdCotizacion == cotizacion.IdCotizacion && rc.IdRepuesto == idRepuesto);

            if (cantidadActual >= repuesto.Stock)
            {
                TempData["Error"] = $"No puedes agregar más unidades de {repuesto.Nombre}. Stock disponible: {repuesto.Stock}.";
            }
            else
            {
                var repuestoCotizado = new RepuestosCotizadosModel
                {
                    IdCotizacion = cotizacion.IdCotizacion,
                    IdRepuesto = idRepuesto
                };
                db.RepuestosCotizados.Add(repuestoCotizado);
                db.SaveChanges();
                TempData["Success"] = $"Se agregó una unidad de {repuesto.Nombre} a tu cotización.";
            }

            string referer = Request.UrlReferrer?.ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("CatalogoRepuestos");
        }

        // Agregar servicio al carrito
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public ActionResult AgregarServicio(int idServicio)
        {
            var userId = User.Identity.GetUserId();

            // Verificar que el servicio existe y está activo
            var servicio = db.ServicioModels.Find(idServicio);
            if (servicio == null || !servicio.Activo)
            {
                TempData["Error"] = "El servicio no está disponible.";
                return RedirectToAction("CatalogoServicios");
            }

            // Obtener o crear cotización activa
            var cotizacion = db.Cotizaciones
                   .FirstOrDefault(c => c.IdCliente == userId && c.Estado == "ACTIVA");

            if (cotizacion == null)
            {
                cotizacion = new CotizacionesModel
                {
                    IdCliente = userId,
                    Estado = "ACTIVA",
                    FechaCreacion = DateTime.Now,
                    FechaCita = DateTime.Now.AddDays(1),
                    MontoTotal = 0
                };
                db.Cotizaciones.Add(cotizacion);
                db.SaveChanges();
            }

            // Verificar si ya existe en la cotización
            var yaExiste = db.ServiciosCotizados
              .Any(sc => sc.IdCotizacion == cotizacion.IdCotizacion && sc.IdServicio == idServicio);

            if (!yaExiste)
            {
                var servicioCotizado = new ServiciosCotizadosModel
                {
                    IdCotizacion = cotizacion.IdCotizacion,
                    IdServicio = idServicio,
                    IdCliente = 0
                };
                db.ServiciosCotizados.Add(servicioCotizado);
                db.SaveChanges();

                TempData["Success"] = $"Se agregó {servicio.Nombre} a tu cotización.";
            }
            else
            {
                TempData["Info"] = "Este servicio ya está en tu cotización.";
            }

            string referer = Request.UrlReferrer?.ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("CatalogoServicios");
        }

        // Eliminar repuesto del carrito
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public ActionResult EliminarRepuesto(int idRepuesto)
        {
            var userId = User.Identity.GetUserId();

            var cotizacion = db.Cotizaciones
                   .FirstOrDefault(c => c.IdCliente == userId && c.Estado == "ACTIVA");

            if (cotizacion != null)
            {
                var repuestoCotizado = db.RepuestosCotizados
                    .FirstOrDefault(rc => rc.IdCotizacion == cotizacion.IdCotizacion && rc.IdRepuesto == idRepuesto);

                if (repuestoCotizado != null)
                {
                    db.RepuestosCotizados.Remove(repuestoCotizado);
                    db.SaveChanges();
                    TempData["Success"] = "Repuesto eliminado de la cotización.";
                }
            }

            return RedirectToAction("MiCarrito");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public ActionResult IncrementarRepuesto(int idRepuesto)
        {
            return AgregarRepuesto(idRepuesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public ActionResult DisminuirRepuesto(int idRepuesto)
        {
            return EliminarRepuesto(idRepuesto);
        }

        // Eliminar servicio del carrito
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public ActionResult EliminarServicio(int idServicio)
        {
            var userId = User.Identity.GetUserId();

            var cotizacion = db.Cotizaciones
          .FirstOrDefault(c => c.IdCliente == userId && c.Estado == "ACTIVA");

            if (cotizacion != null)
            {
                var servicioCotizado = db.ServiciosCotizados
       .FirstOrDefault(sc => sc.IdCotizacion == cotizacion.IdCotizacion && sc.IdServicio == idServicio);

                if (servicioCotizado != null)
                {
                    db.ServiciosCotizados.Remove(servicioCotizado);
                    db.SaveChanges();
                    TempData["Success"] = "Servicio eliminado de la cotización.";
                }
            }

            return RedirectToAction("MiCarrito");
        }

        // Catálogo de repuestos para clientes
        [Authorize(Roles = "Cliente")]
        public ActionResult CatalogoRepuestos(string q, string sort = "nombre")
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
                            (r.NumeroParte ?? "").Contains(q));
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

            var list = query.ToList();
            return View(list);
        }

        // Catálogo de servicios para clientes
        [Authorize(Roles = "Cliente")]
        public ActionResult CatalogoServicios(string q)
        {
            q = (q ?? string.Empty).Trim();

            var query = db.ServicioModels
                   .AsNoTracking()
                 .Where(s => s.Activo);

            if (q.Length > 0)
            {
                query = query.Where(s =>
            s.Nombre.Contains(q) ||
                    (s.Descripcion ?? "").Contains(q));
            }

            var list = query.OrderBy(s => s.Nombre).ToList();
            return View(list);
        }

        // Solicitar cotización (cambiar estado a REVISION)
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public ActionResult SolicitarCotizacion()
        {
            var userId = User.Identity.GetUserId();

            var cotizacion = db.Cotizaciones
                     .FirstOrDefault(c => c.IdCliente == userId && c.Estado == "ACTIVA");

            if (cotizacion == null)
            {
                TempData["Error"] = "No tienes una cotización activa.";
                return RedirectToAction("MiCarrito");
            }

            var tieneRepuestos = db.RepuestosCotizados.Any(rc => rc.IdCotizacion == cotizacion.IdCotizacion);
            var tieneServicios = db.ServiciosCotizados.Any(sc => sc.IdCotizacion == cotizacion.IdCotizacion);

            if (!tieneRepuestos && !tieneServicios)
            {
                TempData["Error"] = "Debes agregar al menos un repuesto o servicio.";
                return RedirectToAction("MiCarrito");
            }


            var repuestosAgrupados = db.RepuestosCotizados
                .Where(rc => rc.IdCotizacion == cotizacion.IdCotizacion)
                .GroupBy(rc => rc.IdRepuesto)
                .Select(g => new { IdRepuesto = g.Key, Cantidad = g.Count() })
                .ToList();

            var serviciosAgrupados = db.ServiciosCotizados
                .Where(sc => sc.IdCotizacion == cotizacion.IdCotizacion)
                .GroupBy(sc => sc.IdServicio)
                .Select(g => new { IdServicio = g.Key, Cantidad = g.Count() })
                .ToList();

            decimal totalRepuestos = 0;
            foreach (var item in repuestosAgrupados)
            {
                var rep = db.Repuestos.Find(item.IdRepuesto);
                if (rep == null || rep.IsDeleted || rep.OcultarClientes || rep.SinStockForzado || rep.Stock < item.Cantidad)
                {
                    TempData["Error"] = "Hay repuestos no disponibles o sin stock suficiente en tu carrito. Actualiza tu carrito e intenta de nuevo.";
                    return RedirectToAction("MiCarrito");
                }
                totalRepuestos += rep.Precio * item.Cantidad;
            }

            decimal totalServicios = 0;
            foreach (var item in serviciosAgrupados)
            {
                var serv = db.ServicioModels.Find(item.IdServicio);
                if (serv == null || !serv.Activo)
                {
                    TempData["Error"] = "Hay servicios no disponibles en tu carrito. Actualiza tu carrito e intenta de nuevo.";
                    return RedirectToAction("MiCarrito");
                }
                totalServicios += serv.PrecioServicio * item.Cantidad;
            }

            cotizacion.MontoTotal = totalRepuestos + totalServicios;

            cotizacion.Estado = "REVICION";
            db.SaveChanges();

            TempData["Success"] = "Tu cotización ha sido enviada. Un administrador la revisará pronto.";
            return RedirectToAction("MisCotizaciones");
        }

        // Ver historial de cotizaciones del cliente
        [Authorize(Roles = "Cliente")]
        public ActionResult MisCotizaciones()
        {
            var userId = User.Identity.GetUserId();

            var cotizaciones = db.Cotizaciones
          .Where(c => c.IdCliente == userId && c.Estado != "ACTIVA")
     .OrderByDescending(c => c.FechaCreacion)
                .ToList();

            return View(cotizaciones);
        }

        // Lista de cotizaciones - Admin
        [Authorize(Roles = "Admin")]
        public ActionResult GestionarCotizaciones(string estado = "TODAS", int page = 1)
        {
            const int pageSize = 20;
            var query = db.Cotizaciones.AsQueryable();

            var estadoNormalizado = (estado ?? "TODAS").Trim().ToUpperInvariant();
            if (estadoNormalizado != "TODAS")
            {
                query = query.Where(c => c.Estado == estadoNormalizado);
            }

            var safePage = page < 1 ? 1 : page;
            var total = query.Count();
            var totalPages = Math.Max(1, (int)Math.Ceiling((double)total / pageSize));
            if (safePage > totalPages) safePage = totalPages;

            var cotizaciones = query
                .OrderByDescending(c => c.FechaCreacion)
                .Skip((safePage - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            var viewModel = new List<CotizacionListadoViewModel>();
            foreach (var cot in cotizaciones)
            {
                var cliente = db.Users.Find(cot.IdCliente);
                var cantidadRepuestos = db.RepuestosCotizados.Count(rc => rc.IdCotizacion == cot.IdCotizacion);
                var cantidadServicios = db.ServiciosCotizados.Count(sc => sc.IdCotizacion == cot.IdCotizacion);

                viewModel.Add(new CotizacionListadoViewModel
                {
                    IdCotizacion = cot.IdCotizacion,
                    IdCliente = cot.IdCliente,
                    NombreCliente = cliente?.NombreCompleto ?? "Cliente no encontrado",
                    EmailCliente = cliente?.Email ?? "",
                    FechaCreacion = cot.FechaCreacion,
                    FechaCita = cot.FechaCita,
                    Estado = cot.Estado,
                    CantidadRepuestos = cantidadRepuestos,
                    CantidadServicios = cantidadServicios,
                    MontoTotal = cot.MontoTotal
                });
            }

            ViewBag.EstadoFiltro = estadoNormalizado;
            ViewBag.Page = safePage;
            ViewBag.TotalPages = totalPages;
            return View(viewModel);
        }

        // Ver detalles de una cotización - Admin
        [Authorize(Roles = "Admin")]
        public ActionResult DetalleCotizacion(int id)
        {
            var cotizacion = db.Cotizaciones.Find(id);
            if (cotizacion == null)
            {
                return HttpNotFound();
            }

            // Cargar repuestos completos
            var repuestoIds = db.RepuestosCotizados
           .Where(rc => rc.IdCotizacion == id)
            .Select(rc => rc.IdRepuesto)
                 .ToList();
            var repuestoCantidades = db.RepuestosCotizados
                .Where(rc => rc.IdCotizacion == id)
                .GroupBy(rc => rc.IdRepuesto)
                .ToDictionary(g => g.Key, g => g.Count());

            cotizacion.RepuestosCotizados = db.Repuestos
      .Where(r => repuestoIds.Contains(r.Id))
      .ToList();

            // Cargar servicios completos
            var servicioIds = db.ServiciosCotizados
           .Where(sc => sc.IdCotizacion == id)
       .Select(sc => sc.IdServicio)
     .ToList();
            var servicioCantidades = db.ServiciosCotizados
                .Where(sc => sc.IdCotizacion == id)
                .GroupBy(sc => sc.IdServicio)
                .ToDictionary(g => g.Key, g => g.Count());

            cotizacion.ServiciosCotizados = db.ServicioModels
     .Where(s => servicioIds.Contains(s.IdServicio))
        .ToList();

            // Cargar información del cliente
            ViewBag.Cliente = db.Users.Find(cotizacion.IdCliente);
            ViewBag.RepuestoCantidades = repuestoCantidades;
            ViewBag.ServicioCantidades = servicioCantidades;

            return View(cotizacion);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AprobarCotizacion(int id)
        {
            TempData["Info"] = "La aprobación/rechazo de cotizaciones fue deshabilitada. Este flujo ya no modifica inventario.";
            return RedirectToAction("DetalleCotizacion", new { id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult RechazarCotizacion(int id, string motivo)
        {
            TempData["Info"] = "La aprobación/rechazo de cotizaciones fue deshabilitada. Este flujo ya no está disponible.";
            return RedirectToAction("DetalleCotizacion", new { id });
        }


        [ChildActionOnly]
        public ActionResult ContadorCarrito()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Cliente"))
            {
                return Content("0");
            }

            var userId = User.Identity.GetUserId();

            var cotizacion = db.Cotizaciones
         .FirstOrDefault(c => c.IdCliente == userId && c.Estado == "ACTIVA");

            if (cotizacion == null)
            {
                return Content("0");
            }

            var totalRepuestos = db.RepuestosCotizados
            .Count(rc => rc.IdCotizacion == cotizacion.IdCotizacion);

            var totalServicios = db.ServiciosCotizados
       .Count(sc => sc.IdCotizacion == cotizacion.IdCotizacion);

            var totalItems = totalRepuestos + totalServicios;

            return Content(totalItems.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}