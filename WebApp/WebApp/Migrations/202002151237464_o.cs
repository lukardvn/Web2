namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class o : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Status", c => c.String());
            AddColumn("dbo.AspNetUsers", "HasVerified", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Files", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Files");
            DropColumn("dbo.AspNetUsers", "HasVerified");
            DropColumn("dbo.AspNetUsers", "Status");
        }
    }
}
