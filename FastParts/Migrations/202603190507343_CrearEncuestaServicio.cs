namespace FastParts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrearEncuestaServicio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EncuestaServicioModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CitaId = c.Int(nullable: false),
                        ClienteId = c.String(nullable: false, maxLength: 128),
                        MecanicoId = c.String(maxLength: 128),
                        CalificacionGeneral = c.Int(nullable: false),
                        CalificacionMecanico = c.Int(nullable: false),
                        RecomendariaTaller = c.Boolean(nullable: false),
                        Comentario = c.String(maxLength: 500),
                        FechaRespuesta = c.DateTime(),
                        Respondida = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CitaModels", t => t.CitaId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ClienteId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.MecanicoId)
                .Index(t => t.CitaId)
                .Index(t => t.ClienteId)
                .Index(t => t.MecanicoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EncuestaServicioModels", "MecanicoId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EncuestaServicioModels", "ClienteId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EncuestaServicioModels", "CitaId", "dbo.CitaModels");
            DropIndex("dbo.EncuestaServicioModels", new[] { "MecanicoId" });
            DropIndex("dbo.EncuestaServicioModels", new[] { "ClienteId" });
            DropIndex("dbo.EncuestaServicioModels", new[] { "CitaId" });
            DropTable("dbo.EncuestaServicioModels");
        }
    }
}
