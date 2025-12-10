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
                         .Include(c => c.RepuestosCotizados)
             .Include(c => c.ServiciosCotizados)
                .Where(c => c.IdCliente == userId && c.Estado == "ACTIVA")
                .FirstOrDefault();

            if (cotizacion == null)
            {
                cotizacion = new CotizacionesModel
                {
                    IdCliente = userId,
                    Estado = "ACTIVA",
                    FechaCreacion = DateTime.Now,
                    MontoTotal = 0
                };
            }
            else
            {
                // Cargar los repuestos completos
                var repuestoIds = db.RepuestosCotizados
                  .Where(rc => rc.IdCotizacion == cotizacion.IdCotizacion)
                .Select(rc => rc.IdRepuesto)
                          .ToList();

                cotizacion.RepuestosCotizados = db.Repuestos
                      .Where(r => repuestoIds.Contains(r.Id))
                       .ToList();

                // Cargar los servicios completos
                var servicioIds = db.ServiciosCotizados
                     .Where(sc => sc.IdCotizacion == cotizacion.IdCotizacion)
                         .Select(sc => sc.IdServicio)
                     .ToList();

                cotizacion.ServiciosCotizados = db.ServicioModels
                 .Where(s => servicioIds.Contains(s.IdServicio))
                  .ToList();

                // Calcular el monto total
                decimal totalRepuestos = cotizacion.RepuestosCotizados.Sum(r => r.Precio);
                decimal totalServicios = cotizacion.ServiciosCotizados.Sum(s => s.PrecioServicio);
                cotizacion.MontoTotal = totalRepuestos + totalServicios;
            }

            return View(cotizacion);
        }

        // Agregar repuesto al carrito
        [HttpPost]
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

            // Verificar si ya existe en la cotización
            var yaExiste = db.RepuestosCotizados
                     .Any(rc => rc.IdCotizacion == cotizacion.IdCotizacion && rc.IdRepuesto == idRepuesto);

            if (!yaExiste)
            {
                var repuestoCotizado = new RepuestosCotizadosModel
                {
                    IdCotizacion = cotizacion.IdCotizacion,
                    IdRepuesto = idRepuesto
                };
                db.RepuestosCotizados.Add(repuestoCotizado);
                db.SaveChanges();

                TempData["Success"] = $"Se agregó {repuesto.Nombre} a tu cotización.";
            }
            else
            {
                TempData["Info"] = "Este repuesto ya está en tu cotización.";
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

        // Eliminar servicio del carrito
        [HttpPost]
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


            var repuestoIds = db.RepuestosCotizados
                 .Where(rc => rc.IdCotizacion == cotizacion.IdCotizacion)
            .Select(rc => rc.IdRepuesto)
                .ToList();

            var servicioIds = db.ServiciosCotizados
             .Where(sc => sc.IdCotizacion == cotizacion.IdCotizacion)
               .Select(sc => sc.IdServicio)
                  .ToList();

            decimal totalRepuestos = db.Repuestos
                  .Where(r => repuestoIds.Contains(r.Id))
         .Sum(r => (decimal?)r.Precio) ?? 0;

            decimal totalServicios = db.ServicioModels
                    .Where(s => servicioIds.Contains(s.IdServicio))
              .Sum(s => (decimal?)s.PrecioServicio) ?? 0;

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

        // Lista de cotizaciones en revisión - Admin
        [Authorize(Roles = "Admin")]
        public ActionResult GestionarCotizaciones(string estado = "REVICION")
        {
            var query = db.Cotizaciones.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(c => c.Estado == estado);
            }

            var cotizaciones = query
              .OrderByDescending(c => c.FechaCreacion)
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

            ViewBag.EstadoFiltro = estado;
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

            cotizacion.RepuestosCotizados = db.Repuestos
      .Where(r => repuestoIds.Contains(r.Id))
      .ToList();

            // Cargar servicios completos
            var servicioIds = db.ServiciosCotizados
           .Where(sc => sc.IdCotizacion == id)
       .Select(sc => sc.IdServicio)
     .ToList();

            cotizacion.ServiciosCotizados = db.ServicioModels
     .Where(s => servicioIds.Contains(s.IdServicio))
        .ToList();

            // Cargar información del cliente
            ViewBag.Cliente = db.Users.Find(cotizacion.IdCliente);

            return View(cotizacion);
        }

        // Aprobar cotización y descontar stock - Admin
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AprobarCotizacion(int id)
        {
            var cotizacion = db.Cotizaciones.Find(id);
            if (cotizacion == null)
            {
                TempData["Error"] = "Cotización no encontrada.";
                return RedirectToAction("GestionarCotizaciones");
            }

            if (cotizacion.Estado != "REVICION")
            {
                TempData["Error"] = "Solo se pueden aprobar cotizaciones en revisión.";
                return RedirectToAction("DetalleCotizacion", new { id });
            }

            // Obtener repuestos de la cotización
            var repuestosIds = db.RepuestosCotizados
         .Where(rc => rc.IdCotizacion == id)
                .Select(rc => rc.IdRepuesto)
                .ToList();

            var repuestos = db.Repuestos
             .Where(r => repuestosIds.Contains(r.Id))
        .ToList();

            // Verificar que hay suficiente stock
            var stockInsuficiente = new List<string>();
            foreach (var repuesto in repuestos)
            {
                if (repuesto.Stock < 1)
                {
                    stockInsuficiente.Add($"{repuesto.Nombre} (Stock: {repuesto.Stock})");
                }
            }

            if (stockInsuficiente.Any())
            {
                TempData["Error"] = $"Stock insuficiente para: {string.Join(", ", stockInsuficiente)}";
                return RedirectToAction("DetalleCotizacion", new { id });
            }

            // Descontar stock de repuestos
            foreach (var repuesto in repuestos)
            {
                repuesto.Stock -= 1; // Descontar 1 unidad

                // Registrar movimiento de inventario
                var movimiento = new MovimientoInventarioModel
                {
                    RepuestoId = repuesto.Id,
                    Tipo = TipoMovimiento.Salida,
                    Cantidad = 1,
                    Fecha = DateTime.Now,
                    UsuarioId = User.Identity.GetUserId()
                };
                db.Movimientos.Add(movimiento);

                // Crear alerta si el stock está bajo mínimo
                if (repuesto.Stock <= repuesto.StockMinimo)
                {
                    var alertaExistente = db.Alertas
                             .Any(a => a.RepuestoId == repuesto.Id && !a.Atendida);

                    if (!alertaExistente)
                    {
                        var alerta = new AlertaInventarioModel
                        {
                            RepuestoId = repuesto.Id,
                            Fecha = DateTime.Now,
                            Mensaje = $"Stock bajo: {repuesto.Nombre} (Stock actual: {repuesto.Stock}, Mínimo: {repuesto.StockMinimo})",
                            Atendida = false
                        };
                        db.Alertas.Add(alerta);
                    }
                }
            }

            // Cambiar estado de la cotización
            cotizacion.Estado = "COMPLETADA";
            cotizacion.IdResponsable = User.Identity.GetUserId();

            db.SaveChanges();

            TempData["Success"] = $"Cotización #{id} aprobada exitosamente. Stock actualizado.";
            return RedirectToAction("GestionarCotizaciones");
        }

        // Rechazar cotización - Admin
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult RechazarCotizacion(int id, string motivo)
        {
            var cotizacion = db.Cotizaciones.Find(id);
            if (cotizacion == null)
            {
                TempData["Error"] = "Cotización no encontrada.";
                return RedirectToAction("GestionarCotizaciones");
            }

            if (cotizacion.Estado != "REVICION")
            {
                TempData["Error"] = "Solo se pueden rechazar cotizaciones en revisión.";
                return RedirectToAction("DetalleCotizacion", new { id });
            }

            cotizacion.Estado = "INACTIVA";
            cotizacion.IdResponsable = User.Identity.GetUserId();

            db.SaveChanges();

            TempData["Success"] = $"Cotización #{id} rechazada.";
            return RedirectToAction("GestionarCotizaciones");
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