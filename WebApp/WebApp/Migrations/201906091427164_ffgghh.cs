namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ffgghh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DayInWeek = c.Int(nullable: false),
                        TimeTable = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TransportLines",
                c => new
                    {
                        TransportLineID = c.String(nullable: false, maxLength: 128),
                        FromTo = c.String(),
                        Departures_ID = c.Int(),
                    })
                .PrimaryKey(t => t.TransportLineID)
                .ForeignKey("dbo.Departures", t => t.Departures_ID)
                .Index(t => t.Departures_ID);
            
            CreateTable(
                "dbo.LinePoints",
                c => new
                    {
                        LinePointID = c.Int(nullable: false, identity: true),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        TransportLineID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.LinePointID)
                .ForeignKey("dbo.TransportLines", t => t.TransportLineID, cascadeDelete: true)
                .Index(t => t.TransportLineID);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        StationID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Address = c.String(),
                        TransportLine_TransportLineID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StationID)
                .ForeignKey("dbo.TransportLines", t => t.TransportLine_TransportLineID)
                .Index(t => t.TransportLine_TransportLineID);
            
            CreateTable(
                "dbo.StationsOnLines",
                c => new
                    {
                        StationsOnLineID = c.Int(nullable: false, identity: true),
                        StationID = c.Int(nullable: false),
                        TransportLineID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.StationsOnLineID)
                .ForeignKey("dbo.Stations", t => t.StationID, cascadeDelete: true)
                .ForeignKey("dbo.TransportLines", t => t.TransportLineID, cascadeDelete: true)
                .Index(t => t.StationID)
                .Index(t => t.TransportLineID);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        VehicleID = c.Int(nullable: false, identity: true),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        TransportLineID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.VehicleID)
                .ForeignKey("dbo.TransportLines", t => t.TransportLineID, cascadeDelete: true)
                .Index(t => t.TransportLineID);
            
            CreateTable(
                "dbo.PriceFinals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        PricelistID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pricelists", t => t.PricelistID, cascadeDelete: true)
                .Index(t => t.PricelistID);
            
            CreateTable(
                "dbo.Pricelists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketID = c.Int(nullable: false, identity: true),
                        TicketType = c.String(nullable: false),
                        BoughtAt = c.DateTime(nullable: false),
                        UserID = c.Int(nullable: false),
                        Expires = c.DateTime(nullable: false),
                        PriceFinalID = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TicketID)
                .ForeignKey("dbo.PriceFinals", t => t.PriceFinalID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.PriceFinalID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        UserTypeID = c.Int(nullable: false, identity: true),
                        TypeOfUser = c.Int(nullable: false),
                        Coefficient = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.UserTypeID);
            
            AddColumn("dbo.AspNetUsers", "UserTypeID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "UserTypeID");
            AddForeignKey("dbo.AspNetUsers", "UserTypeID", "dbo.UserTypes", "UserTypeID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserTypeID", "dbo.UserTypes");
            DropForeignKey("dbo.Tickets", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "PriceFinalID", "dbo.PriceFinals");
            DropForeignKey("dbo.PriceFinals", "PricelistID", "dbo.Pricelists");
            DropForeignKey("dbo.TransportLines", "Departures_ID", "dbo.Departures");
            DropForeignKey("dbo.Vehicles", "TransportLineID", "dbo.TransportLines");
            DropForeignKey("dbo.Stations", "TransportLine_TransportLineID", "dbo.TransportLines");
            DropForeignKey("dbo.StationsOnLines", "TransportLineID", "dbo.TransportLines");
            DropForeignKey("dbo.StationsOnLines", "StationID", "dbo.Stations");
            DropForeignKey("dbo.LinePoints", "TransportLineID", "dbo.TransportLines");
            DropIndex("dbo.AspNetUsers", new[] { "UserTypeID" });
            DropIndex("dbo.Tickets", new[] { "User_Id" });
            DropIndex("dbo.Tickets", new[] { "PriceFinalID" });
            DropIndex("dbo.PriceFinals", new[] { "PricelistID" });
            DropIndex("dbo.Vehicles", new[] { "TransportLineID" });
            DropIndex("dbo.StationsOnLines", new[] { "TransportLineID" });
            DropIndex("dbo.StationsOnLines", new[] { "StationID" });
            DropIndex("dbo.Stations", new[] { "TransportLine_TransportLineID" });
            DropIndex("dbo.LinePoints", new[] { "TransportLineID" });
            DropIndex("dbo.TransportLines", new[] { "Departures_ID" });
            DropColumn("dbo.AspNetUsers", "UserTypeID");
            DropTable("dbo.UserTypes");
            DropTable("dbo.Tickets");
            DropTable("dbo.Pricelists");
            DropTable("dbo.PriceFinals");
            DropTable("dbo.Vehicles");
            DropTable("dbo.StationsOnLines");
            DropTable("dbo.Stations");
            DropTable("dbo.LinePoints");
            DropTable("dbo.TransportLines");
            DropTable("dbo.Departures");
        }
    }
}
