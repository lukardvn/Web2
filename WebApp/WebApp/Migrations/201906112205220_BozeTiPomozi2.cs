namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BozeTiPomozi2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PriceFinals", "TicketID");
            DropColumn("dbo.Tickets", "PriceFinalID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "PriceFinalID", c => c.Int());
            AddColumn("dbo.PriceFinals", "TicketID", c => c.Int(nullable: false));
        }
    }
}
