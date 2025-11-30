using FastParts.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastParts.Migrations
{
    public class SeedCitas
    {
        public void CrearCitasDemo(ApplicationDbContext context)
        {
            var cliente = context.Users.FirstOrDefault(u => u.UserName == "cliente@fastparts.com");

            if (context.CitaModels.Any())
                return;

            if (cliente != null)
            {
                var cita1 = new CitaModel
                {
                    UsuarioId = cliente.Id,
                    NombreCliente = cliente.NombreCompleto ?? "Usuario Cliente Por Defecto",
                    TelefonoCliente = "8888-8888",
                    Vehiculo = "Toyota Corolla 2018",
                    Placa = "ABC123",
                    FechaCita = DateTime.Now.AddDays(1),
                    Motivo = "Cambio de aceite y revisión general",
                    Estado = "Ingresada"
                };
                context.CitaModels.Add(cita1);
            }

            if (cliente != null)
            {

                var cita2 = new CitaModel
                {
                    UsuarioId = cliente.Id,
                    NombreCliente = cliente.NombreCompleto ?? "Usuario Cliente Por Defecto",
                    TelefonoCliente = "8888-8888",
                    Vehiculo = "BMW X6",
                    Placa = "FGH567",
                    FechaCita = DateTime.Now.AddDays(1),
                    Motivo = "Cambio de luces",
                    Estado = "Ingresada"
                };
                context.CitaModels.Add(cita2);
            }

            context.SaveChanges();
        }
    }

}
