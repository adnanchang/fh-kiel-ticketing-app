namespace FH_Kiel_Ticketing_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sumission : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Submissions",
                c => new
                    {
                        RecID = c.Int(nullable: false, identity: true),
                        submissionDate = c.DateTime(nullable: false),
                        Ticket_recordID = c.Int(),
                        User_recordID = c.Int(),
                    })
                .PrimaryKey(t => t.RecID)
                .ForeignKey("dbo.Tickets", t => t.Ticket_recordID)
                .ForeignKey("dbo.Users", t => t.User_recordID)
                .Index(t => t.Ticket_recordID)
                .Index(t => t.User_recordID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Submissions", "User_recordID", "dbo.Users");
            DropForeignKey("dbo.Submissions", "Ticket_recordID", "dbo.Tickets");
            DropIndex("dbo.Submissions", new[] { "User_recordID" });
            DropIndex("dbo.Submissions", new[] { "Ticket_recordID" });
            DropTable("dbo.Submissions");
        }
    }
}
