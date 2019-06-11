namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class glavobolja4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TimeTable = c.String(),
                        ValidFrom = c.DateTime(nullable: false),
                        TransportLineID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TransportLines", t => t.TransportLineID)
                .Index(t => t.TransportLineID);
            
            CreateTable(
                "dbo.TransportLines",
                c => new
                    {
                        TransportLineID = c.String(nullable: false, maxLength: 128),
                        FromTo = c.String(),
                    })
                .PrimaryKey(t => t.TransportLineID);
            
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
                        ID = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        PricelistID = c.Int(nullable: false),
                        TicketID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pricelists", t => t.PricelistID, cascadeDelete: true)
                .ForeignKey("dbo.Tickets", t => t.ID)
                .Index(t => t.ID)
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
                        PriceFinalID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TicketID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
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
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Adress", c => c.String());
            AddColumn("dbo.AspNetUsers", "Password", c => c.String());
            AddColumn("dbo.AspNetUsers", "ConfirmedPassword", c => c.String());
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.DateTime());
            CreateIndex("dbo.AspNetUsers", "UserTypeID");
            AddForeignKey("dbo.AspNetUsers", "UserTypeID", "dbo.UserTypes", "UserTypeID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceFinals", "ID", "dbo.Tickets");
            DropForeignKey("dbo.AspNetUsers", "UserTypeID", "dbo.UserTypes");
            DropForeignKey("dbo.Tickets", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PriceFinals", "PricelistID", "dbo.Pricelists");
            DropForeignKey("dbo.Departures", "TransportLineID", "dbo.TransportLines");
            DropForeignKey("dbo.Vehicles", "TransportLineID", "dbo.TransportLines");
            DropForeignKey("dbo.Stations", "TransportLine_TransportLineID", "dbo.TransportLines");
            DropForeignKey("dbo.StationsOnLines", "TransportLineID", "dbo.TransportLines");
            DropForeignKey("dbo.StationsOnLines", "StationID", "dbo.Stations");
            DropForeignKey("dbo.LinePoints", "TransportLineID", "dbo.TransportLines");
            DropIndex("dbo.AspNetUsers", new[] { "UserTypeID" });
            DropIndex("dbo.Tickets", new[] { "User_Id" });
            DropIndex("dbo.PriceFinals", new[] { "PricelistID" });
            DropIndex("dbo.PriceFinals", new[] { "ID" });
            DropIndex("dbo.Vehicles", new[] { "TransportLineID" });
            DropIndex("dbo.StationsOnLines", new[] { "TransportLineID" });
            DropIndex("dbo.StationsOnLines", new[] { "StationID" });
            DropIndex("dbo.Stations", new[] { "TransportLine_TransportLineID" });
            DropIndex("dbo.LinePoints", new[] { "TransportLineID" });
            DropIndex("dbo.Departures", new[] { "TransportLineID" });
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
            DropColumn("dbo.AspNetUsers", "ConfirmedPassword");
            DropColumn("dbo.AspNetUsers", "Password");
            DropColumn("dbo.AspNetUsers", "Adress");
            DropColumn("dbo.AspNetUsers", "Surname");
            DropColumn("dbo.AspNetUsers", "Name");
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
