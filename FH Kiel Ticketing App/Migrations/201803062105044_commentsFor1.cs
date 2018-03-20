namespace FH_Kiel_Ticketing_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentsFor1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artifacts",
                c => new
                    {
                        RecID = c.Int(nullable: false, identity: true),
                        content = c.String(),
                        creationDate = c.DateTime(nullable: false),
                        ticket_recordID = c.Int(),
                        user_recordID = c.Int(),
                        ArtifactTemplate_RecID = c.Int(),
                    })
                .PrimaryKey(t => t.RecID)
                .ForeignKey("dbo.Tickets", t => t.ticket_recordID)
                .ForeignKey("dbo.Users", t => t.user_recordID)
                .ForeignKey("dbo.ArtifactTemplates", t => t.ArtifactTemplate_RecID)
                .Index(t => t.ticket_recordID)
                .Index(t => t.user_recordID)
                .Index(t => t.ArtifactTemplate_RecID);
            
            CreateTable(
                "dbo.ArtifactTemplates",
                c => new
                    {
                        RecID = c.Int(nullable: false, identity: true),
                        Field_recordID = c.Int(),
                        user_recordID = c.Int(),
                    })
                .PrimaryKey(t => t.RecID)
                .ForeignKey("dbo.Fields", t => t.Field_recordID)
                .ForeignKey("dbo.Users", t => t.user_recordID)
                .Index(t => t.Field_recordID)
                .Index(t => t.user_recordID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArtifactTemplates", "user_recordID", "dbo.Users");
            DropForeignKey("dbo.ArtifactTemplates", "Field_recordID", "dbo.Fields");
            DropForeignKey("dbo.Artifacts", "ArtifactTemplate_RecID", "dbo.ArtifactTemplates");
            DropForeignKey("dbo.Artifacts", "user_recordID", "dbo.Users");
            DropForeignKey("dbo.Artifacts", "ticket_recordID", "dbo.Tickets");
            DropIndex("dbo.ArtifactTemplates", new[] { "user_recordID" });
            DropIndex("dbo.ArtifactTemplates", new[] { "Field_recordID" });
            DropIndex("dbo.Artifacts", new[] { "ArtifactTemplate_RecID" });
            DropIndex("dbo.Artifacts", new[] { "user_recordID" });
            DropIndex("dbo.Artifacts", new[] { "ticket_recordID" });
            DropTable("dbo.ArtifactTemplates");
            DropTable("dbo.Artifacts");
        }
    }
}
