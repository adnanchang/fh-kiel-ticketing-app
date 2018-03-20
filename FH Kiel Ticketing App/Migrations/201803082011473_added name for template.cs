namespace FH_Kiel_Ticketing_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addednamefortemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArtifactTemplates", "name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArtifactTemplates", "name");
        }
    }
}
