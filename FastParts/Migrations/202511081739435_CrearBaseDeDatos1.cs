namespace FastParts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrearBaseDeDatos1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AlertaInventarioModels", "RepuestoId", "dbo.RepuestoModels");
            DropForeignKey("dbo.RespuestasModels", "ID_Pregunta", "dbo.PreguntaModels");
            DropForeignKey("dbo.MovimientoInventarioModels", "RepuestoId", "dbo.RepuestoModels");
            DropForeignKey("dbo.ServicioRepuestoModels", "RepuestoId", "dbo.RepuestoModels");
            DropForeignKey("dbo.ServicioRepuestoModels", "Servicio_IdServicio", "dbo.ServicioModels");
            DropForeignKey("dbo.MovimientoInventarioModels", "ServicioRepuestoId", "dbo.ServicioRepuestoModels");
            DropIndex("dbo.AlertaInventarioModels", new[] { "RepuestoId" });
            DropIndex("dbo.RespuestasModels", new[] { "ID_Pregunta" });
            DropIndex("dbo.MovimientoInventarioModels", new[] { "RepuestoId" });
            DropIndex("dbo.MovimientoInventarioModels", new[] { "ServicioRepuestoId" });
            DropIndex("dbo.ServicioRepuestoModels", new[] { "RepuestoId" });
            DropIndex("dbo.ServicioRepuestoModels", new[] { "Servicio_IdServicio" });
            DropColumn("dbo.RepuestoModels", "StockMinimo");
            DropColumn("dbo.RepuestoModels", "OcultarClientes");
            DropColumn("dbo.RepuestoModels", "SinStockForzado");
            DropColumn("dbo.RepuestoModels", "IsDeleted");
            DropColumn("dbo.RepuestoModels", "DeletedAt");
            DropColumn("dbo.EncuestaModels", "Activa");
            DropColumn("dbo.PreguntaModels", "Requerido");
            DropColumn("dbo.PreguntaModels", "Activa");
            DropColumn("dbo.PreguntaModels", "ValorRespuesta");
            DropColumn("dbo.PreguntaModels", "TextoRespuesta");
            DropColumn("dbo.AspNetUsers", "NombreCompleto");
            DropColumn("dbo.AspNetUsers", "Direccion");
            DropColumn("dbo.AspNetUsers", "Estado");
            DropTable("dbo.AlertaInventarioModels");
            DropTable("dbo.RespuestasModels");
            DropTable("dbo.MovimientoInventarioModels");
            DropTable("dbo.ServicioRepuestoModels");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RespuestasModels",
                c => new
                    {
                        ID_Respuesta = c.Int(nullable: false, identity: true),
                        ID_Encuesta = c.Int(nullable: false),
                        ID_Pregunta = c.Int(nullable: false),
                        Session_Id = c.String(),
                        Tipo = c.String(nullable: false),
                        ValorRespuesta = c.Int(),
                        TextoRespuesta = c.String(),
                    })
                .PrimaryKey(t => t.ID_Respuesta);
            
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Estado", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Direccion", c => c.String());
            AddColumn("dbo.AspNetUsers", "NombreCompleto", c => c.String());
            AddColumn("dbo.PreguntaModels", "TextoRespuesta", c => c.String());
            AddColumn("dbo.PreguntaModels", "ValorRespuesta", c => c.Int());
            AddColumn("dbo.PreguntaModels", "Activa", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreguntaModels", "Requerido", c => c.Boolean(nullable: false));
            AddColumn("dbo.EncuestaModels", "Activa", c => c.Boolean(nullable: false));
            AddColumn("dbo.RepuestoModels", "DeletedAt", c => c.DateTime());
            AddColumn("dbo.RepuestoModels", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.RepuestoModels", "SinStockForzado", c => c.Boolean(nullable: false));
            AddColumn("dbo.RepuestoModels", "OcultarClientes", c => c.Boolean(nullable: false));
            AddColumn("dbo.RepuestoModels", "StockMinimo", c => c.Int(nullable: false));
            CreateIndex("dbo.ServicioRepuestoModels", "Servicio_IdServicio");
            CreateIndex("dbo.ServicioRepuestoModels", "RepuestoId");
            CreateIndex("dbo.MovimientoInventarioModels", "ServicioRepuestoId");
            CreateIndex("dbo.MovimientoInventarioModels", "RepuestoId");
            CreateIndex("dbo.RespuestasModels", "ID_Pregunta");
            CreateIndex("dbo.AlertaInventarioModels", "RepuestoId");
            AddForeignKey("dbo.MovimientoInventarioModels", "ServicioRepuestoId", "dbo.ServicioRepuestoModels", "Id");
            AddForeignKey("dbo.ServicioRepuestoModels", "Servicio_IdServicio", "dbo.ServicioModels", "IdServicio");
            AddForeignKey("dbo.ServicioRepuestoModels", "RepuestoId", "dbo.RepuestoModels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MovimientoInventarioModels", "RepuestoId", "dbo.RepuestoModels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RespuestasModels", "ID_Pregunta", "dbo.PreguntaModels", "ID_Pregunta", cascadeDelete: true);
            AddForeignKey("dbo.AlertaInventarioModels", "RepuestoId", "dbo.RepuestoModels", "Id", cascadeDelete: true);
        }
    }
}
