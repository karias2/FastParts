using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastParts.Controllers
{
    public class ReporteController : Controller
    {
        //// GET: Reporte
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // Simulación de datos en memoria
        private static List<dynamic> servicios = new List<dynamic>
        {
            new { Fecha = new DateTime(2025, 8, 1), Cliente = "Juan Pérez", Servicio = "Cambio de aceite" },
            new { Fecha = new DateTime(2025, 8, 2), Cliente = "Ana Torres", Servicio = "Alineamiento" },
            new { Fecha = new DateTime(2025, 7, 31), Cliente = "Carlos Rojas", Servicio = "Frenos" },
        };

        private static List<dynamic> encuestas = new List<dynamic>
        {
            new { Fecha = new DateTime(2025, 8, 1), Cliente = "Juan Pérez", Puntuacion = 5, Comentario = "Excelente" },
            new { Fecha = new DateTime(2025, 8, 2), Cliente = "Ana Torres", Puntuacion = 4, Comentario = "Muy bien" }
        };

        private static List<dynamic> recordatorios = new List<dynamic>
        {
            new { Cliente = "Carlos Rojas", Servicio = "Cambio de batería", FechaServicio = new DateTime(2025, 8, 15), Metodo = "Correo" },
            new { Cliente = "Lucía Vargas", Servicio = "Revisión general", FechaServicio = new DateTime(2025, 8, 20), Metodo = "WhatsApp" }
        };

        // Vista principal de reportes
        public ActionResult Index()
        {
            return View();
        }

        // Formulario de filtro
        public ActionResult Filtro(string tipo)
        {
            ViewBag.Tipo = tipo;
            return View();
        }

        // Resultado del reporte
        public ActionResult Resultado(string tipo, DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<dynamic> resultado = new List<dynamic>();
            List<string> columnas = new List<string>();

            switch (tipo)
            {
                case "ServiciosPorFecha":
                    resultado = servicios
                        .Where(s => fechaInicio == null || s.Fecha >= fechaInicio)
                        .Where(s => fechaFin == null || s.Fecha <= fechaFin)
                        .ToList<dynamic>();

                    columnas = new List<string> { "Fecha", "Cliente", "Servicio" };
                    break;

                case "Encuestas":
                    resultado = encuestas
                        .Where(e => fechaInicio == null || e.Fecha >= fechaInicio)
                        .Where(e => fechaFin == null || e.Fecha <= fechaFin)
                        .ToList<dynamic>();

                    columnas = new List<string> { "Fecha", "Cliente", "Puntuacion", "Comentario" };
                    break;

                case "Recordatorios":
                    resultado = recordatorios.ToList<dynamic>();
                    columnas = new List<string> { "Cliente", "Servicio", "FechaServicio", "Metodo" };
                    break;
            }

            ViewBag.Tipo = tipo;
            ViewBag.Columnas = columnas;

            // Convertir objetos anónimos a IDictionary<string, object> para usar en la vista genérica
            var resultadoConvertido = resultado.Select(item =>
            {
                IDictionary<string, object> dict = new ExpandoObject();
                foreach (var prop in item.GetType().GetProperties())
                {
                    dict[prop.Name] = prop.GetValue(item);
                }
                return dict;
            }).ToList();

            return View(resultadoConvertido);
        }


    }
}