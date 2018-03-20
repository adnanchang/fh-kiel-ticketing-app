namespace FH_Kiel_Ticketing_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sumission3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        RecID = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        File = c.String(),
                        User_recordID = c.Int(nullable: false),
                        Submission_RecID = c.Int(),
                    })
                .PrimaryKey(t => t.RecID)
                .ForeignKey("dbo.Users", t => t.User_recordID, cascadeDelete: true)
                .ForeignKey("dbo.Submissions", t => t.Submission_RecID)
                .Index(t => t.User_recordID)
                .Index(t => t.Submission_RecID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "Submission_RecID", "dbo.Submissions");
            DropForeignKey("dbo.Files", "User_recordID", "dbo.Users");
            DropIndex("dbo.Files", new[] { "Submission_RecID" });
            DropIndex("dbo.Files", new[] { "User_recordID" });
            DropTable("dbo.Files");
        }
    }
}
