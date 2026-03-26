namespace FastParts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustarEncuestaServicioNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EncuestaServicioModels", "CalificacionGeneral", c => c.Int());
            AlterColumn("dbo.EncuestaServicioModels", "CalificacionMecanico", c => c.Int());
            AlterColumn("dbo.EncuestaServicioModels", "RecomendariaTaller", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EncuestaServicioModels", "RecomendariaTaller", c => c.Boolean(nullable: false));
            AlterColumn("dbo.EncuestaServicioModels", "CalificacionMecanico", c => c.Int(nullable: false));
            AlterColumn("dbo.EncuestaServicioModels", "CalificacionGeneral", c => c.Int(nullable: false));
        }
    }
}
