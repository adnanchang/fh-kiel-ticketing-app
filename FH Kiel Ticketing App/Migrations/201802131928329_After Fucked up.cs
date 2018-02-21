namespace FH_Kiel_Ticketing_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AfterFuckedup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        RecID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        CommentDate = c.DateTime(nullable: false),
                        RepliedTo_RecID = c.Int(),
                        Ticket_recordID = c.Int(nullable: false),
                        User_recordID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RecID)
                .ForeignKey("dbo.Comments", t => t.RepliedTo_RecID)
                .ForeignKey("dbo.Tickets", t => t.Ticket_recordID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_recordID, cascadeDelete: true)
                .Index(t => t.RepliedTo_RecID)
                .Index(t => t.Ticket_recordID)
                .Index(t => t.User_recordID);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false),
                        status = c.String(),
                        timesRejected = c.Int(nullable: false),
                        tickettype = c.String(),
                        creationDate = c.DateTime(nullable: false),
                        idea_recordID = c.Int(),
                        ticketStatus_recordID = c.Int(),
                        User_recordID = c.Int(),
                    })
                .PrimaryKey(t => t.recordID)
                .ForeignKey("dbo.Ideas", t => t.idea_recordID)
                .ForeignKey("dbo.TicketStatus", t => t.ticketStatus_recordID)
                .ForeignKey("dbo.Users", t => t.User_recordID)
                .Index(t => t.idea_recordID)
                .Index(t => t.ticketStatus_recordID)
                .Index(t => t.User_recordID);
            
            CreateTable(
                "dbo.Ideas",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false),
                        description = c.String(nullable: false),
                        type = c.String(nullable: false),
                        field = c.String(nullable: false),
                        User_recordID = c.Int(),
                    })
                .PrimaryKey(t => t.recordID)
                .ForeignKey("dbo.Users", t => t.User_recordID)
                .Index(t => t.User_recordID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        firstName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        email = c.String(nullable: false),
                        password = c.String(nullable: false),
                        isEmailVerified = c.Boolean(nullable: false),
                        activationCode = c.Guid(nullable: false),
                        emailNotification = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.recordID);
            
            CreateTable(
                "dbo.Contributors",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        status = c.String(nullable: false),
                        Role = c.String(nullable: false),
                        FuckYou = c.Int(nullable: false),
                        Ticket_recordID = c.Int(),
                        User_recordID = c.Int(),
                    })
                .PrimaryKey(t => t.recordID)
                .ForeignKey("dbo.Tickets", t => t.Ticket_recordID)
                .ForeignKey("dbo.Users", t => t.User_recordID)
                .Index(t => t.Ticket_recordID)
                .Index(t => t.User_recordID);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        message = c.String(nullable: false),
                        url = c.String(nullable: false),
                        isRead = c.Boolean(nullable: false),
                        User_recordID = c.Int(),
                    })
                .PrimaryKey(t => t.recordID)
                .ForeignKey("dbo.Users", t => t.User_recordID)
                .Index(t => t.User_recordID);
            
            CreateTable(
                "dbo.TicketStatus",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        ticketStatus = c.String(),
                    })
                .PrimaryKey(t => t.recordID);
            
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        field = c.String(),
                    })
                .PrimaryKey(t => t.recordID);
            
            CreateTable(
                "dbo.Supervisors",
                c => new
                    {
                        recordID = c.Int(nullable: false),
                        userType = c.String(),
                    })
                .PrimaryKey(t => t.recordID)
                .ForeignKey("dbo.Users", t => t.recordID)
                .Index(t => t.recordID);
            
            CreateTable(
                "dbo.Proposals",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        nameOfProject = c.String(nullable: false),
                        abstrac = c.String(nullable: false),
                        introduction = c.String(nullable: false),
                        overallDescription = c.String(nullable: false),
                        functionalRequirements = c.String(nullable: false),
                        nonFunctionalRequirements = c.String(nullable: false),
                        projectTechnologies = c.String(nullable: false),
                        result = c.String(nullable: false),
                        Idea_recordID = c.Int(nullable: false),
                        User_recordID = c.Int(),
                    })
                .PrimaryKey(t => t.recordID)
                .ForeignKey("dbo.Ideas", t => t.Idea_recordID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_recordID)
                .Index(t => t.Idea_recordID)
                .Index(t => t.User_recordID);
            
            CreateTable(
                "dbo.RoleIdentifiers",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.recordID);
            
            CreateTable(
                "dbo.RoleIdentifierDetails",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        identifier = c.String(nullable: false),
                        RoleIdentifier_recordID = c.Int(),
                    })
                .PrimaryKey(t => t.recordID)
                .ForeignKey("dbo.RoleIdentifiers", t => t.RoleIdentifier_recordID)
                .Index(t => t.RoleIdentifier_recordID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        recordID = c.Int(nullable: false),
                        matrikelNumber = c.Int(nullable: false),
                        beginningSemesterSeason = c.String(),
                        beginningSemesterYear = c.Int(nullable: false),
                        userType = c.String(),
                    })
                .PrimaryKey(t => t.recordID)
                .ForeignKey("dbo.Users", t => t.recordID)
                .Index(t => t.recordID);
            
            CreateTable(
                "dbo.SupervisorFields",
                c => new
                    {
                        Supervisor_recordID = c.Int(nullable: false),
                        Fields_recordID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Supervisor_recordID, t.Fields_recordID })
                .ForeignKey("dbo.Supervisors", t => t.Supervisor_recordID, cascadeDelete: true)
                .ForeignKey("dbo.Fields", t => t.Fields_recordID, cascadeDelete: true)
                .Index(t => t.Supervisor_recordID)
                .Index(t => t.Fields_recordID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "recordID", "dbo.Users");
            DropForeignKey("dbo.RoleIdentifierDetails", "RoleIdentifier_recordID", "dbo.RoleIdentifiers");
            DropForeignKey("dbo.Proposals", "User_recordID", "dbo.Users");
            DropForeignKey("dbo.Proposals", "Idea_recordID", "dbo.Ideas");
            DropForeignKey("dbo.Supervisors", "recordID", "dbo.Users");
            DropForeignKey("dbo.SupervisorFields", "Fields_recordID", "dbo.Fields");
            DropForeignKey("dbo.SupervisorFields", "Supervisor_recordID", "dbo.Supervisors");
            DropForeignKey("dbo.Comments", "User_recordID", "dbo.Users");
            DropForeignKey("dbo.Comments", "Ticket_recordID", "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "User_recordID", "dbo.Users");
            DropForeignKey("dbo.Tickets", "ticketStatus_recordID", "dbo.TicketStatus");
            DropForeignKey("dbo.Tickets", "idea_recordID", "dbo.Ideas");
            DropForeignKey("dbo.Notifications", "User_recordID", "dbo.Users");
            DropForeignKey("dbo.Ideas", "User_recordID", "dbo.Users");
            DropForeignKey("dbo.Contributors", "User_recordID", "dbo.Users");
            DropForeignKey("dbo.Contributors", "Ticket_recordID", "dbo.Tickets");
            DropForeignKey("dbo.Comments", "RepliedTo_RecID", "dbo.Comments");
            DropIndex("dbo.SupervisorFields", new[] { "Fields_recordID" });
            DropIndex("dbo.SupervisorFields", new[] { "Supervisor_recordID" });
            DropIndex("dbo.Students", new[] { "recordID" });
            DropIndex("dbo.RoleIdentifierDetails", new[] { "RoleIdentifier_recordID" });
            DropIndex("dbo.Proposals", new[] { "User_recordID" });
            DropIndex("dbo.Proposals", new[] { "Idea_recordID" });
            DropIndex("dbo.Supervisors", new[] { "recordID" });
            DropIndex("dbo.Notifications", new[] { "User_recordID" });
            DropIndex("dbo.Contributors", new[] { "User_recordID" });
            DropIndex("dbo.Contributors", new[] { "Ticket_recordID" });
            DropIndex("dbo.Ideas", new[] { "User_recordID" });
            DropIndex("dbo.Tickets", new[] { "User_recordID" });
            DropIndex("dbo.Tickets", new[] { "ticketStatus_recordID" });
            DropIndex("dbo.Tickets", new[] { "idea_recordID" });
            DropIndex("dbo.Comments", new[] { "User_recordID" });
            DropIndex("dbo.Comments", new[] { "Ticket_recordID" });
            DropIndex("dbo.Comments", new[] { "RepliedTo_RecID" });
            DropTable("dbo.SupervisorFields");
            DropTable("dbo.Students");
            DropTable("dbo.RoleIdentifierDetails");
            DropTable("dbo.RoleIdentifiers");
            DropTable("dbo.Proposals");
            DropTable("dbo.Supervisors");
            DropTable("dbo.Fields");
            DropTable("dbo.TicketStatus");
            DropTable("dbo.Notifications");
            DropTable("dbo.Contributors");
            DropTable("dbo.Users");
            DropTable("dbo.Ideas");
            DropTable("dbo.Tickets");
            DropTable("dbo.Comments");
        }
    }
}
