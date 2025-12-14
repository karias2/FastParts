namespace FastParts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrearBaseDeDatos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlertaInventarioModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RepuestoId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Mensaje = c.String(),
                        Atendida = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RepuestoModels", t => t.RepuestoId, cascadeDelete: true)
                .Index(t => t.RepuestoId);
            
            CreateTable(
                "dbo.RepuestoModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 120),
                        StockMinimo = c.Int(nullable: false),
                        OcultarClientes = c.Boolean(nullable: false),
                        SinStockForzado = c.Boolean(nullable: false),
                        Marca = c.String(maxLength: 80),
                        NumeroParte = c.String(maxLength: 80),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Int(nullable: false),
                        Proveedor = c.String(maxLength: 200),
                        Descripcion = c.String(maxLength: 500),
                        CreatedAt = c.DateTime(nullable: false),
                        ImagenUrl = c.String(maxLength: 300),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedAt = c.DateTime(),
                        CotizacionesModel_IdCotizacion = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CotizacionesModels", t => t.CotizacionesModel_IdCotizacion)
                .Index(t => t.CotizacionesModel_IdCotizacion);
            
            CreateTable(
                "dbo.CitaModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(maxLength: 128),
                        NombreCliente = c.String(),
                        TelefonoCliente = c.String(),
                        Vehiculo = c.String(),
                        Placa = c.String(),
                        FechaCita = c.DateTime(nullable: false),
                        Motivo = c.String(),
                        MecanicoId = c.String(maxLength: 128),
                        HoraInicio = c.DateTime(),
                        HoraFin = c.DateTime(),
                        FotoAntes = c.Binary(),
                        FotoDespues = c.Binary(),
                        FotoAntesContentType = c.String(),
                        FotoDespuesContentType = c.String(),
                        Estado = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.MecanicoId)
                .ForeignKey("dbo.AspNetUsers", t => t.UsuarioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.MecanicoId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        NombreCompleto = c.String(),
                        Direccion = c.String(),
                        Estado = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.CitaRepuestoModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CitaId = c.Int(nullable: false),
                        RepuestoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CitaModels", t => t.CitaId, cascadeDelete: true)
                .ForeignKey("dbo.RepuestoModels", t => t.RepuestoId, cascadeDelete: true)
                .Index(t => t.CitaId)
                .Index(t => t.RepuestoId);
            
            CreateTable(
                "dbo.CitaServicioModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CitaId = c.Int(nullable: false),
                        ServicioId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CitaModels", t => t.CitaId, cascadeDelete: true)
                .ForeignKey("dbo.ServicioModels", t => t.ServicioId, cascadeDelete: true)
                .Index(t => t.CitaId)
                .Index(t => t.ServicioId);
            
            CreateTable(
                "dbo.ServicioModels",
                c => new
                    {
                        IdServicio = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Descripcion = c.String(nullable: false),
                        PrecioServicio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Activo = c.Boolean(nullable: false),
                        CotizacionesModel_IdCotizacion = c.Int(),
                    })
                .PrimaryKey(t => t.IdServicio)
                .ForeignKey("dbo.CotizacionesModels", t => t.CotizacionesModel_IdCotizacion)
                .Index(t => t.CotizacionesModel_IdCotizacion);
            
            CreateTable(
                "dbo.CotizacionesModels",
                c => new
                    {
                        IdCotizacion = c.Int(nullable: false, identity: true),
                        IdCliente = c.String(),
                        IdResponsable = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaCita = c.DateTime(nullable: false),
                        Estado = c.String(),
                        MontoTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IdCotizacion);
            
            CreateTable(
                "dbo.EncuestaModels",
                c => new
                    {
                        ID_Encuesta = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                        Activa = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID_Encuesta);
            
            CreateTable(
                "dbo.PreguntaModels",
                c => new
                    {
                        ID_Pregunta = c.Int(nullable: false, identity: true),
                        ID_Encuesta = c.Int(nullable: false),
                        Descripcion = c.String(nullable: false, maxLength: 400),
                        Tipo = c.String(nullable: false),
                        Requerido = c.Boolean(nullable: false),
                        Activa = c.Boolean(nullable: false),
                        Opciones = c.String(),
                        Minimo = c.Int(),
                        Maximo = c.Int(),
                        ValorRespuesta = c.Int(),
                        TextoRespuesta = c.String(),
                    })
                .PrimaryKey(t => t.ID_Pregunta)
                .ForeignKey("dbo.EncuestaModels", t => t.ID_Encuesta, cascadeDelete: true)
                .Index(t => t.ID_Encuesta);
            
            CreateTable(
                "dbo.RespuestasModels",
                c => new
                    {
                        ID_Respuesta = c.Int(nullable: false, identity: true),
                        ID_Encuesta = c.Int(nullable: false),
                        ID_Pregunta = c.Int(nullable: false),
                        Session_Id = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                        Tipo = c.String(nullable: false),
                        ValorRespuesta = c.Int(),
                        TextoRespuesta = c.String(),
                    })
                .PrimaryKey(t => t.ID_Respuesta)
                .ForeignKey("dbo.PreguntaModels", t => t.ID_Pregunta, cascadeDelete: true)
                .Index(t => t.ID_Pregunta);
            
            CreateTable(
                "dbo.MovimientoInventarioModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RepuestoId = c.Int(nullable: false),
                        Tipo = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        UsuarioId = c.String(),
                        ServicioRepuestoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RepuestoModels", t => t.RepuestoId, cascadeDelete: true)
                .ForeignKey("dbo.ServicioRepuestoModels", t => t.ServicioRepuestoId)
                .Index(t => t.RepuestoId)
                .Index(t => t.ServicioRepuestoId);
            
            CreateTable(
                "dbo.ServicioRepuestoModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServicioId = c.Int(nullable: false),
                        RepuestoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Servicio_IdServicio = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RepuestoModels", t => t.RepuestoId, cascadeDelete: true)
                .ForeignKey("dbo.ServicioModels", t => t.Servicio_IdServicio)
                .Index(t => t.RepuestoId)
                .Index(t => t.Servicio_IdServicio);
            
            CreateTable(
                "dbo.RepuestosCotizadosModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IdCotizacion = c.Int(nullable: false),
                        IdRepuesto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CotizacionesModels", t => t.IdCotizacion, cascadeDelete: true)
                .ForeignKey("dbo.RepuestoModels", t => t.IdRepuesto, cascadeDelete: true)
                .Index(t => t.IdCotizacion)
                .Index(t => t.IdRepuesto);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ServiciosCotizadosModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IdCliente = c.Int(nullable: false),
                        IdCotizacion = c.Int(nullable: false),
                        IdServicio = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CotizacionesModels", t => t.IdCotizacion, cascadeDelete: true)
                .ForeignKey("dbo.ServicioModels", t => t.IdServicio, cascadeDelete: true)
                .Index(t => t.IdCotizacion)
                .Index(t => t.IdServicio);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiciosCotizadosModels", "IdServicio", "dbo.ServicioModels");
            DropForeignKey("dbo.ServiciosCotizadosModels", "IdCotizacion", "dbo.CotizacionesModels");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RepuestosCotizadosModels", "IdRepuesto", "dbo.RepuestoModels");
            DropForeignKey("dbo.RepuestosCotizadosModels", "IdCotizacion", "dbo.CotizacionesModels");
            DropForeignKey("dbo.MovimientoInventarioModels", "ServicioRepuestoId", "dbo.ServicioRepuestoModels");
            DropForeignKey("dbo.ServicioRepuestoModels", "Servicio_IdServicio", "dbo.ServicioModels");
            DropForeignKey("dbo.ServicioRepuestoModels", "RepuestoId", "dbo.RepuestoModels");
            DropForeignKey("dbo.MovimientoInventarioModels", "RepuestoId", "dbo.RepuestoModels");
            DropForeignKey("dbo.RespuestasModels", "ID_Pregunta", "dbo.PreguntaModels");
            DropForeignKey("dbo.PreguntaModels", "ID_Encuesta", "dbo.EncuestaModels");
            DropForeignKey("dbo.ServicioModels", "CotizacionesModel_IdCotizacion", "dbo.CotizacionesModels");
            DropForeignKey("dbo.RepuestoModels", "CotizacionesModel_IdCotizacion", "dbo.CotizacionesModels");
            DropForeignKey("dbo.CitaModels", "UsuarioId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CitaServicioModels", "ServicioId", "dbo.ServicioModels");
            DropForeignKey("dbo.CitaServicioModels", "CitaId", "dbo.CitaModels");
            DropForeignKey("dbo.CitaRepuestoModels", "RepuestoId", "dbo.RepuestoModels");
            DropForeignKey("dbo.CitaRepuestoModels", "CitaId", "dbo.CitaModels");
            DropForeignKey("dbo.CitaModels", "MecanicoId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AlertaInventarioModels", "RepuestoId", "dbo.RepuestoModels");
            DropIndex("dbo.ServiciosCotizadosModels", new[] { "IdServicio" });
            DropIndex("dbo.ServiciosCotizadosModels", new[] { "IdCotizacion" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RepuestosCotizadosModels", new[] { "IdRepuesto" });
            DropIndex("dbo.RepuestosCotizadosModels", new[] { "IdCotizacion" });
            DropIndex("dbo.ServicioRepuestoModels", new[] { "Servicio_IdServicio" });
            DropIndex("dbo.ServicioRepuestoModels", new[] { "RepuestoId" });
            DropIndex("dbo.MovimientoInventarioModels", new[] { "ServicioRepuestoId" });
            DropIndex("dbo.MovimientoInventarioModels", new[] { "RepuestoId" });
            DropIndex("dbo.RespuestasModels", new[] { "ID_Pregunta" });
            DropIndex("dbo.PreguntaModels", new[] { "ID_Encuesta" });
            DropIndex("dbo.ServicioModels", new[] { "CotizacionesModel_IdCotizacion" });
            DropIndex("dbo.CitaServicioModels", new[] { "ServicioId" });
            DropIndex("dbo.CitaServicioModels", new[] { "CitaId" });
            DropIndex("dbo.CitaRepuestoModels", new[] { "RepuestoId" });
            DropIndex("dbo.CitaRepuestoModels", new[] { "CitaId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.CitaModels", new[] { "MecanicoId" });
            DropIndex("dbo.CitaModels", new[] { "UsuarioId" });
            DropIndex("dbo.RepuestoModels", new[] { "CotizacionesModel_IdCotizacion" });
            DropIndex("dbo.AlertaInventarioModels", new[] { "RepuestoId" });
            DropTable("dbo.ServiciosCotizadosModels");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RepuestosCotizadosModels");
            DropTable("dbo.ServicioRepuestoModels");
            DropTable("dbo.MovimientoInventarioModels");
            DropTable("dbo.RespuestasModels");
            DropTable("dbo.PreguntaModels");
            DropTable("dbo.EncuestaModels");
            DropTable("dbo.CotizacionesModels");
            DropTable("dbo.ServicioModels");
            DropTable("dbo.CitaServicioModels");
            DropTable("dbo.CitaRepuestoModels");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CitaModels");
            DropTable("dbo.RepuestoModels");
            DropTable("dbo.AlertaInventarioModels");
        }
    }
}
