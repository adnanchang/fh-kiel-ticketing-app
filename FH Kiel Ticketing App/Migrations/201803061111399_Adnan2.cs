namespace FH_Kiel_Ticketing_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adnan2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleIdentifiers",
                c => new
                    {
                        roleIdentifierID = c.Int(nullable: false, identity: true),
                        role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.roleIdentifierID);
            
            CreateTable(
                "dbo.RoleIdentifierDetails",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        identifier = c.String(nullable: false),
                        roleIdentifierID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.recordID)
                .ForeignKey("dbo.RoleIdentifiers", t => t.roleIdentifierID, cascadeDelete: true)
                .Index(t => t.roleIdentifierID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleIdentifierDetails", "roleIdentifierID", "dbo.RoleIdentifiers");
            DropIndex("dbo.RoleIdentifierDetails", new[] { "roleIdentifierID" });
            DropTable("dbo.RoleIdentifierDetails");
            DropTable("dbo.RoleIdentifiers");
        }
    }
}
