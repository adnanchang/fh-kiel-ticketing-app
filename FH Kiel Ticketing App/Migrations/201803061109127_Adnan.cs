namespace FH_Kiel_Ticketing_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adnan : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RoleIdentifierDetails", "roleIdentifierID", "dbo.RoleIdentifiers");
            DropIndex("dbo.RoleIdentifierDetails", new[] { "roleIdentifierID" });
            
            DropTable("dbo.RoleIdentifierDetails");
            DropTable("dbo.RoleIdentifiers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RoleIdentifierDetails",
                c => new
                    {
                        recordID = c.Int(nullable: false, identity: true),
                        identifier = c.String(nullable: false),
                        roleIdentifierID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.recordID);
            
            CreateTable(
                "dbo.RoleIdentifiers",
                c => new
                    {
                        roleIdentifierID = c.Int(nullable: false, identity: true),
                        role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.roleIdentifierID);
            
            CreateIndex("dbo.RoleIdentifierDetails", "roleIdentifierID");
            AddForeignKey("dbo.RoleIdentifierDetails", "roleIdentifierID", "dbo.RoleIdentifiers", "roleIdentifierID", cascadeDelete: true);
        }
    }
}
