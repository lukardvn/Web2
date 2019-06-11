namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BozeTiPomozi123 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "BoughtAt", c => c.DateTime());
            AlterColumn("dbo.Tickets", "Expires", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "Expires", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tickets", "BoughtAt", c => c.DateTime(nullable: false));
        }
    }
}
