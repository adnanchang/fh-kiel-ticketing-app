namespace FH_Kiel_Ticketing_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentsFor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "CommentsFor", c => c.String());
            AddColumn("dbo.Supervisors", "nothing", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Supervisors", "nothing");
            DropColumn("dbo.Comments", "CommentsFor");
        }
    }
}
