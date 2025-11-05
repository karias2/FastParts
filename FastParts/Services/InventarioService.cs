using FastParts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastParts.Services
{

    public interface IInventarioService
    {
        void RegistrarConsumo(int servicioId, IEnumerable<(int repuestoId, int cantidad)> items, string usuarioId = null);
    }

    public class InventarioService : IInventarioService
    {
        private readonly ApplicationDbContext _db;
        public InventarioService(ApplicationDbContext db) => _db = db;

        public void RegistrarConsumo(int servicioId, IEnumerable<(int repuestoId, int cantidad)> items, string usuarioId = null)
        {
            using (var tx = _db.Database.BeginTransaction())
            {
                foreach (var (repuestoId, cantidad) in items)
                {
                    var rep = _db.Repuestos.Single(r => r.Id == repuestoId);

                    if (rep.SinStockForzado) throw new InvalidOperationException($"Repuesto {rep.Nombre} está marcado sin stock.");
                    if (cantidad <= 0) throw new ArgumentException("Cantidad inválida.");
                    if (rep.Stock < cantidad) throw new InvalidOperationException($"Stock insuficiente de {rep.Nombre}.");

                    // Registrar detalle de servicio
                    var sr = new ServicioRepuestoModel
                    {
                        ServicioId = servicioId,
                        RepuestoId = repuestoId,
                        Cantidad = cantidad,
                        PrecioUnitario = rep.Precio
                    };
                    _db.ServicioRepuestos.Add(sr);

                    // Descontar stock y movimiento
                    rep.Stock -= cantidad;
                    _db.Movimientos.Add(new MovimientoInventarioModel
                    {
                        RepuestoId = repuestoId,
                        Tipo = TipoMovimiento.Salida,
                        Cantidad = cantidad,
                        UsuarioId = usuarioId,
                        ServicioRepuesto = sr
                    });

                    // Generar alerta si cayó al mínimo
                    if (rep.Stock <= rep.StockMinimo)
                    {
                        _db.Alertas.Add(new AlertaInventarioModel
                        {
                            RepuestoId = rep.Id,
                            Mensaje = $"Repuesto \"{rep.Nombre}\" alcanzó el stock mínimo ({rep.Stock}/{rep.StockMinimo})."
                        });
                    }
                }

                _db.SaveChanges();
                tx.Commit();
            }
        }
    }
}