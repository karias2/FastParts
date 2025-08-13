using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FastParts.Models;

namespace FastParts.Controllers
{
    public class CotizacionsController : Controller
    {
        public static List<Cotizacion> cotizaciones = new List<Cotizacion>
    {
        new Cotizacion
        {
            Id = 1,
            Cliente = new Cliente { Nombre="Juan", Apellido="Pérez", Identificacion="12345678", Correo="juan@email.com"},
            Fecha = DateTime.Now,
            Servicios = new List<Servicio>
            {
                new Servicio { Descripcion="Cambio de aceite", Cantidad=1, PrecioUnitario=20 },
                new Servicio { Descripcion="Filtro de aire", Cantidad=1, PrecioUnitario=15 }
            },
            Enviada = false
        },
        new Cotizacion
        {
            Id = 2,
            Cliente = new Cliente { Nombre="Ana", Apellido="Gómez", Identificacion="87654321", Correo="ana@email.com"},
            Fecha = DateTime.Now,
            Servicios = new List<Servicio>
            {
                new Servicio { Descripcion="Revisión frenos", Cantidad=2, PrecioUnitario=30 }
            },
            Enviada = true
        }
    };

        public ActionResult Index()
        {
            return View(cotizaciones);
        }

        public ActionResult Details(int id)
        {
            var cot = cotizaciones.FirstOrDefault(c => c.Id == id);
            if (cot == null) return HttpNotFound();
            return View(cot);
        }
    }
}
