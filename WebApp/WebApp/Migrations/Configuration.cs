﻿namespace WebApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using WebApp.Models;
    using WebApp.Models.DomainEntities;
    using WebApp.Persistence;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApp.Persistence.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }


        protected override void Seed(WebApp.Persistence.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Controller"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Controller" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "AppUser"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppUser" };

                manager.Create(role);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            ////////////
            //regularan korisnik
            UserType regularanKorisnik = new UserType() { TypeOfUser = 0, Coefficient = 1 };

            context.UserTypes.Add(regularanKorisnik);
            //student
            UserType studentKorisnik = new UserType() { TypeOfUser = 1, Coefficient = 0.8 };

            context.UserTypes.Add(studentKorisnik);
            //penzioner
            UserType penzionerKorisnik = new UserType() { TypeOfUser = 2, Coefficient = 0.6 };
            context.UserTypes.Add(penzionerKorisnik);

            context.SaveChanges();
            ////////////


            if (!context.Users.Any(u => u.UserName == "admin@yahoo.com"))
            {
                var user = new ApplicationUser() { Id = "admin", UserName = "admin@yahoo", Email = "admin@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Admin123!"), DateOfBirth = new DateTime(1996, 9, 6, 10, 02, 01)/*, UserTypeID = 1*/ };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "appu@yahoo.com"))
            {
                var user = new ApplicationUser() { Id = "appu", UserName = "appu@yahoo", Email = "appu@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Appu123!"), DateOfBirth = new DateTime(1996, 9, 6, 10, 02, 01)/*,UserTypeID = 2*/ };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");
            }
            context.SaveChanges();



            if (!context.Users.Any(u => u.UserName == "anonymus@anonymus.com"))
            {
                var user = new ApplicationUser() { Id = "anonymus", UserName = "anonymus@anonymus", Email = "anonymus@anonymus.com", PasswordHash = ApplicationUser.HashPassword("password"), DateOfBirth = new DateTime(1996, 9, 6, 10, 02, 01), UserTypeID = 1 };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");
            }
            if (!context.Users.Any(u => u.UserName == "anonymus1@anonymus.com"))
            {
                var user = new ApplicationUser() { Id = "anonymus1", UserName = "anonymus1@anonymus", Email = "anonymus1@anonymus.com", PasswordHash = ApplicationUser.HashPassword("password"), DateOfBirth = new DateTime(1996, 9, 6, 10, 02, 01), UserTypeID = 2 };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");
            }
            if (!context.Users.Any(u => u.UserName == "anonymus2@anonymus.com"))
            {
                var user = new ApplicationUser() { Id = "anonymus2", UserName = "anonymus2@anonymus", Email = "anonymus2@anonymus.com", PasswordHash = ApplicationUser.HashPassword("password"), DateOfBirth = new DateTime(1996, 9, 6, 10, 02, 01), UserTypeID = 3 };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");
            }
            //ovde ovo jer se koristi kasnije userID,a mora da postoji da se ne bi bunilo zbog referencijalno int
            context.SaveChanges();



            ///////////
            DateTime priceValidFrom = new DateTime(2019, 05, 25);
            Pricelist pricelist = new Pricelist() { From = priceValidFrom, To = priceValidFrom.AddDays(100) };

            context.Pricelists.Add(pricelist);
            context.SaveChanges();
            /////////

            //za neregistrovanog korisnika
            //jako ruzno ovo za userid sto je hardkodovano ali sta bi sada...
            Ticket ticketRegular = new Ticket() { TicketType = "regularna", UserID = "anonymus" };
            context.Tickets.Add(ticketRegular);
            context.SaveChanges();
            ///
            //za sada ovde pravi problem neki ?
            PriceFinal priceFinal = new PriceFinal()
            {
                //u kontroleru cu da ovo uradim,makar malo nekog finog koda...
                //Price = 65*regularanKorisnik.Coefficient,
                Price = 65,
                Pricelist = pricelist,
                PricelistID = pricelist.ID,
                Ticket = ticketRegular
            };

            context.PriceFinals.Add(priceFinal);

            context.SaveChanges();



            //zameniti userid
            Ticket ticketDaily = new Ticket() { TicketType = "dnevna", UserID = "anonymus" };
            context.Tickets.Add(ticketDaily);
            context.SaveChanges();

            PriceFinal priceFinalDaily = new PriceFinal()
            {
                Price = 150,
                Pricelist = pricelist,
                PricelistID = pricelist.ID,
                Ticket = ticketDaily
            };

            context.PriceFinals.Add(priceFinalDaily);
            context.SaveChanges();



            Ticket ticketMonthly = new Ticket() { TicketType = "mesecna", UserID = "anonymus" };
            context.Tickets.Add(ticketMonthly);
            context.SaveChanges();


            PriceFinal priceFinalMonthly = new PriceFinal()
            {
                Price = 935,
                Pricelist = pricelist,
                PricelistID = pricelist.ID,
                Ticket = ticketMonthly
            };

            context.PriceFinals.Add(priceFinalMonthly);
            context.SaveChanges();


            Ticket ticketYearly = new Ticket() { TicketType = "godisnja", UserID = "anonymus" };
            context.Tickets.Add(ticketYearly);
            context.SaveChanges();

            PriceFinal priceFinalYearly = new PriceFinal()
            {
                Price = 16000,
                Pricelist = pricelist,
                PricelistID = pricelist.ID,
                Ticket = ticketYearly
            };

            context.PriceFinals.Add(priceFinalYearly);
            context.SaveChanges();



            //gradski za ns
            //context.TransportLines.Add(new TransportLine() { FromTo = "KLISA - CENTAR - LIMAN I", TransportLineID = "1A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "LIMAN I - CENTAR - KLISA", TransportLineID = "1B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - NOVO NASELJE", TransportLineID = "2A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "NOVO NASELJE - CENTAR", TransportLineID = "2B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "PETROVARADIN - CENTAR - DETELINARA", TransportLineID = "3A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "DETELINARA - CENTAR - PETROVARADIN", TransportLineID = "3B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ZEL.STANICA - POBEDA", TransportLineID = "3AA" });
            context.TransportLines.Add(new TransportLine() { FromTo = "POBEDA - ZEL.STANICA", TransportLineID = "3AB" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "LIMAN IV - CENTAR - Z.STANICA", TransportLineID = "4A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Z.STANICA - CENTAR - LIMAN IV", TransportLineID = "4B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "TEMERINSKI PUT - CENTAR - AVIJATIČAR.NASELJE", TransportLineID = "5A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "AVIJATIČAR.NASELJE - CENTAR - TEMERINSKI PUT", TransportLineID = "5B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "PODBARA - CENTAR - ADICE", TransportLineID = "6A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ADICE - CENTAR - PODBARA", TransportLineID = "6B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ADICE - VETERNIK (O.S.MARIJA TRANDAFIL)", TransportLineID = "6AA" });
            context.TransportLines.Add(new TransportLine() { FromTo = "VETERNIK (O.S.MARIJA TRANDAFIL) - ADICE", TransportLineID = "6AB" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - Z.STAN - F.PIJA - LIMAN4 - N.NASELJE", TransportLineID = "7A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - LIMAN4 - F.PIJA - Z.STAN - N.NASELJE", TransportLineID = "7B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "NOVO NASELJE - CENTAR - LIMAN I", TransportLineID = "8A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "LIMAN I - CENTAR - NOVO NASELJE", TransportLineID = "8B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "NOVO NASELJE - LIMAN - PETROVARADIN", TransportLineID = "9A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "PETROVARADIN - LIMAN - NOVO NASELJE", TransportLineID = "9B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - INDUST.ZONA JUG", TransportLineID = "10A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "INDUST.ZONA JUG - CENTAR", TransportLineID = "10B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Z.STANICA - BOLNICA - LIMAN - Z.STANICA", TransportLineID = "11A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Z.STANICA - LIMAN - BOLNICA - Z.STANICA", TransportLineID = "11B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - TELEP", TransportLineID = "12A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "TELEP - CENTAR", TransportLineID = "12B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "DETELINARA - GRBAVICA - UNIVERZITET", TransportLineID = "13A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "UNIVERZITET - GRBAVICA - DETELINARA", TransportLineID = "13B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - SAJLOVO", TransportLineID = "14A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "SAJLOVO - CENTAR", TransportLineID = "14B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - INDUSTR.ZONA SEVER", TransportLineID = "15A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "INDUSTR.ZONA SEVER - CENTAR", TransportLineID = "15B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "ZELE.STANICA - PRISTANISNA ZONA", TransportLineID = "16A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "PRISTANISNA ZONA - ZELE.STANICA", TransportLineID = "16B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "BIG TC - CENTAR", TransportLineID = "17A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - BIG TC", TransportLineID = "17B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - DETELINARA - CENTAR - LIMAN - N.NASELJE", TransportLineID = "18A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - LIMAN - CENTAR - DETELINARA - N.NASELJE", TransportLineID = "18B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "ZEL.STANICA - LESNINA", TransportLineID = "20A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "LESNINA - ZEL.STANICA", TransportLineID = "20B" });

            //prigradski
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za ŠANGAJ", TransportLineID = "21A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz ŠANGAJ", TransportLineID = "21B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za KAĆ", TransportLineID = "22A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz KAĆ", TransportLineID = "22B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BUDISAVA", TransportLineID = "23A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BUDISAVA", TransportLineID = "23B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za KOVILJ", TransportLineID = "24A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz KOVILJ", TransportLineID = "24B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za PEJIĆEVI SALAŠI", TransportLineID = "30A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz PEJIĆEVI SALAŠI", TransportLineID = "30B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BAČKI JARAK", TransportLineID = "31A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BAČKI JARAK", TransportLineID = "31B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za TEMERIN", TransportLineID = "32A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz TEMERIN", TransportLineID = "32B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za GOSPOĐINCI", TransportLineID = "33A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz GOSPOĐINCI", TransportLineID = "33B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za ČENEJ", TransportLineID = "35A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz ČENEJ", TransportLineID = "35B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za RUMENKA", TransportLineID = "41A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz RUMENKA", TransportLineID = "41B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za KISAČ", TransportLineID = "42A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz KISAČ", TransportLineID = "42B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za STEPANOVIĆEVO", TransportLineID = "43A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz STEPANOVIĆEVO", TransportLineID = "43B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za VETERNIK A", TransportLineID = "51AA" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz VETERNIK A", TransportLineID = "51AB" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za VETERNIK B", TransportLineID = "51BA" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz VETERNIK B", TransportLineID = "51BB" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za VETERNIK", TransportLineID = "52A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz VETERNIK", TransportLineID = "52B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za FUTOG STARI", TransportLineID = "53A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz FUTOG STARI", TransportLineID = "53B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za FUTOG GRMEČKA", TransportLineID = "54A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz FUTOG GRMEČKA", TransportLineID = "54B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za FUTOG BRAĆE BOŠNJAK", TransportLineID = "55A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz FUTOG BRAĆE BOŠNJAK", TransportLineID = "55B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BEGEČ", TransportLineID = "56A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BEGEČ", TransportLineID = "56B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za S.KARLOVCI - BELILO II", TransportLineID = "60A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BELILO II - S.KARLOVCI", TransportLineID = "60B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za SR.KARLOVCI VINOGRADARSKA", TransportLineID = "61A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz SR.KARLOVCI VINOGRADARSKA", TransportLineID = "61B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za SR.KARLOVCI DUDARA", TransportLineID = "62A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz SR.KARLOVCI DUDARA", TransportLineID = "62B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BUKOVAC", TransportLineID = "64A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BUKOVAC", TransportLineID = "64B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za SR.KAMENICA - VOJINOVO", TransportLineID = "68A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz VOJINOVO - SR.KAMENICA", TransportLineID = "68B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za SR.KAMENICA - ČARDAK", TransportLineID = "69A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz ČARDAK - SR.KAMENICA", TransportLineID = "69B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za SR.KAMENICA - BOCKE", TransportLineID = "71A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BOCKE - SR.KAMENICA", TransportLineID = "71B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za PARAGOVO", TransportLineID = "72A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz PARAGOVO", TransportLineID = "72B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za MOŠINA VILA", TransportLineID = "73A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz MOŠINA VILA", TransportLineID = "73B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za POPOVICA", TransportLineID = "74A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz POPOVICA", TransportLineID = "74B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za S.LEDINCI", TransportLineID = "76A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz S.LEDINCI", TransportLineID = "76B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za STARI RAKOVAC", TransportLineID = "77A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz STARI RAKOVAC", TransportLineID = "77B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BEOČIN SELO", TransportLineID = "78A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BEOČIN SELO", TransportLineID = "78B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za ČEREVIĆ", TransportLineID = "79A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz ČEREVIĆ", TransportLineID = "79B" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za B.A.S.", TransportLineID = "80A" });
            //context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz B.A.S.", TransportLineID = "80B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BANOŠTOR", TransportLineID = "81A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BANOŠTOR", TransportLineID = "81B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za LUG", TransportLineID = "84A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz LUG", TransportLineID = "84B" });


            context.SaveChanges();


            //razdvojiti u metode kasnije 
            //vremena
            var validFrom = DateTime.Now;

            //context.Departures.Add(new Departures() { TransportLineID = "1A", TimeTable = "\"Radni_dan\": [\"04:30\", \"05:00\", \"05:15\", \"05:36\", \"05:45\", \"05:54\", \"22:40\", \"23:00\", \"23:20\", \"23:40\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:20\", \"05:37\", \"06:00\", \"23:10\", \"23:35\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:20\", \"21:50\", \"22:10\", \"22:30\", \"22:50\", \"23:10\", \"23:35\", \"00:00\"]", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "1B", TimeTable = "\"Radni_dan\": [\"04:30\", \"05:00\", \"05:18\", \"05:36\", \"05:45\", \"05:54\", \"06:03\", \"06:12\", \"06:21\", \"06:30\", \"21:40\", \"22:00\", \"22:20\", \"22:40\", \"23:00\", \"23:20\", \"23:40\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:20\", \"05:40\", \"06:00\", \"22:30\", \"22:50\", \"23:10\", \"23:35\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:20\", \"05:40\", \"06:00\", \"20:30\", \"20:50\", \"21:10\", \"21:30\", \"21:50\", \"22:10\", \"22:30\", \"22:50\", \"23:10\", \"23:35\", \"00:00\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "2A", TimeTable = "\"Radni_dan\": [\"04:30\", \"05:00\", \"05:22\", \"05:35\", \"05:48\", \"06:01\", \"06:14\", \"06:26\", \"06:37\", \"06:48\", \"21:36\", \"22:00\", \"22:24\", \"22:48\", \"23:12\", \"23:36\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:24\", \"05:48\", \"06:12\", \"06:28\", \"06:44\", \"19:53\", \"20:15\", \"20:37\", \"20:59\", \"21:21\", \"21:43\", \"22:05\", \"22:28\", \"22:51\", \"23:14\", \"23:37\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:24\", \"05:48\", \"20:16\", \"20:38\", \"21:00\", \"21:22\", \"21:44\", \"22:06\", \"22:28\", \"22:51\", \"23:14\", \"23:37\", \"00:00\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "2B", TimeTable = "\"Radni_dan\": [\"04:30\", \"05:00\", \"05:22\", \"05:35\", \"05:47\", \"05:56\", \"06:09\", \"06:18\", \"06:31\", \"06:41\", \"23:36\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:24\", \"05:46\", \"23:35\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:24\", \"05:46\", \"06:03\", \"23:35\", \"00:00\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "3A", TimeTable = "\"Radni_dan\": [\"04:30\", \"05:00\", \"05:20\", \"05:40\", \"05:50\", \"06:00\", \"06:10\", \"06:20\", \"06:30\", \"19:30\", \"19:50\", \"20:10\", \"00:00\"], \"Subota\": [\"04:30\", \"05:10\", \"05:37\", \"06:02\", \"06:21\", \"06:40\", \"06:59\", \"07:18\", \"21:31\", \"21:56\", \"22:21\", \"22:46\", \"23:11\", \"23:36\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:10\", \"05:37\", \"23:25\", \"00:00\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "3B", TimeTable = "\"Radni_dan\": [\"04:30\", \"05:00\", \"05:20\", \"05:40\", \"05:50\", \"06:00\",\"13:56\", \"14:06\", \"14:16\", \"14:26\",  \"20:50\", \"21:10\", \"21:30\", \"21:50\", \"22:10\", \"22:30\", \"22:50\", \"23:10\", \"23:35\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"20:54\", \"21:19\", \"21:44\", \"22:09\", \"22:34\", \"22:59\", \"23:25\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:25\", \"21:32\", \"21:57\", \"22:22\", \"22:47\", \"23:12\", \"23:37\", \"00:00\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "3AA", TimeTable = "\"Radni_dan\": [\"07:00\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "3AB", TimeTable = "\"Radni_dan\": [\"16:10\"]", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "4A", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:00\", \"05:18\", \"05:36\", \"05:54\", \"06:12\", \"06:30\", \"06:48\", \"07:06\", \"07:21\", \"07:35\", \"07:49\", \"08:03\", \"08:17\", \"08:31\", \"08:45\", \"08:59\", \"09:13\", \"09:27\", \"09:41\", \"09:55\", \"10:09\", \"10:23\", \"10:37\", \"10:51\", \"11:05\", \"11:19\", \"11:33\", \"11:47\", \"12:01\", \"12:15\", \"12:29\", \"12:43\", \"12:57\", \"13:11\", \"13:25\", \"13:39\", \"13:53\", \"14:07\", \"14:21\", \"14:35\", \"14:49\", \"15:03\", \"15:17\", \"15:31\", \"15:45\", \"15:59\", \"16:13\", \"16:27\", \"16:41\", \"16:55\", \"17:09\", \"17:23\", \"17:37\", \"17:51\", \"18:05\", \"18:19\", \"18:33\", \"18:48\", \"19:06\", \"19:24\", \"19:42\", \"20:00\", \"20:18\", \"20:36\", \"20:54\", \"21:12\", \"21:30\", \"21:48\", \"22:06\", \"22:24\", \"22:42\", \"23:00\", \"23:20\", \"23:40\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:27\", \"05:45\", \"06:03\", \"06:21\", \"06:39\", \"06:55\", \"07:09\", \"07:23\", \"07:37\", \"07:51\", \"08:05\", \"08:19\", \"08:33\", \"08:47\", \"09:01\", \"09:15\", \"09:29\", \"09:43\", \"09:57\", \"10:11\", \"10:25\", \"10:39\", \"10:53\", \"11:07\", \"11:21\", \"11:35\", \"11:49\", \"12:03\", \"12:17\", \"12:31\", \"12:45\", \"12:59\", \"13:13\", \"13:27\", \"13:41\", \"13:55\", \"14:09\", \"14:23\", \"14:37\", \"14:51\", \"15:05\", \"15:19\", \"15:33\", \"15:47\", \"16:01\", \"16:15\", \"16:29\", \"16:43\", \"16:57\", \"17:11\", \"17:25\", \"17:39\", \"17:53\", \"18:07\", \"18:23\", \"18:41\", \"18:49\", \"19:17\", \"19:35\", \"19:53\", \"20:11\", \"20:29\", \"20:47\", \"21:05\", \"21:23\", \"21:41\", \"21:59\", \"22:17\", \"22:35\", \"22:53\", \"23:13\", \"23:33\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:27\", \"05:45\", \"06:03\", \"06:21\", \"06:39\", \"06:55\", \"07:09\", \"07:23\", \"07:37\", \"07:51\", \"08:05\", \"08:19\", \"08:33\", \"08:47\", \"09:01\", \"09:15\", \"09:29\", \"09:43\", \"09:57\", \"10:11\", \"10:25\", \"10:39\", \"10:53\", \"11:07\", \"11:21\", \"11:35\", \"11:49\", \"12:03\", \"12:17\", \"12:31\", \"12:45\", \"12:59\", \"13:13\", \"13:27\", \"13:41\", \"13:55\", \"14:09\", \"14:23\", \"14:37\", \"14:51\", \"15:05\", \"15:19\", \"15:33\", \"15:47\", \"16:01\", \"16:15\", \"16:29\", \"16:43\", \"16:57\", \"17:11\", \"17:25\", \"17:39\", \"17:53\", \"18:07\", \"18:23\", \"18:41\", \"18:49\", \"19:17\", \"19:35\", \"19:53\", \"20:11\", \"20:29\", \"20:47\", \"21:05\", \"21:23\", \"21:41\", \"21:59\", \"22:17\", \"22:35\", \"22:53\", \"23:13\", \"23:33\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "4B", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:00\", \"05:27\", \"05:45\", \"06:03\", \"06:21\", \"06:39\", \"06:53\", \"07:07\", \"07:21\", \"07:35\", \"07:49\", \"08:03\", \"08:17\", \"08:31\", \"08:45\", \"08:59\", \"09:13\", \"09:27\", \"09:41\", \"09:55\", \"10:09\", \"10:23\", \"10:37\", \"10:51\", \"11:05\", \"11:19\", \"11:33\", \"11:47\", \"12:01\", \"12:15\", \"12:29\", \"12:43\", \"12:57\", \"13:11\", \"13:25\", \"13:39\", \"13:53\", \"14:07\", \"14:21\", \"14:35\", \"14:49\", \"15:03\", \"15:17\", \"15:31\", \"15:45\", \"15:59\", \"16:13\", \"16:27\", \"16:41\", \"16:55\", \"17:09\", \"17:23\", \"17:37\", \"17:51\", \"18:05\", \"18:19\", \"18:33\", \"18:47\", \"19:01\", \"19:15\", \"19:33\", \"19:51\", \"20:09\", \"20:27\", \"20:45\", \"21:03\", \"21:21\", \"21:39\", \"21:57\", \"22:15\", \"22:33\", \"22:51\", \"23:10\", \"23:30\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:18\", \"05:36\", \"05:54\", \"06:12\", \"06:30\", \"06:48\", \"07:06\", \"07:23\", \"07:37\", \"07:51\", \"08:05\", \"08:19\", \"08:33\", \"08:47\", \"09:01\", \"09:15\", \"09:29\", \"09:43\", \"09:57\", \"10:11\", \"10:25\", \"10:39\", \"10:53\", \"11:07\", \"11:21\", \"11:35\", \"11:49\", \"12:03\", \"12:17\", \"12:31\", \"12:45\", \"12:59\", \"13:13\", \"13:27\", \"13:41\", \"13:55\", \"14:09\", \"14:23\", \"14:37\", \"14:51\", \"15:05\", \"15:19\", \"15:33\", \"15:47\", \"16:01\", \"16:15\", \"16:29\", \"16:43\", \"16:57\", \"17:11\", \"17:25\", \"17:39\", \"17:56\", \"18:14\", \"18:32\", \"18:50\", \"19:08\", \"19:26\", \"19:44\", \"20:02\", \"20:20\", \"20:38\", \"20:56\", \"21:14\", \"21:32\", \"21:50\", \"22:08\", \"22:26\", \"22:44\", \"23:02\", \"23:20\", \"23:40\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:18\", \"05:36\", \"05:54\", \"06:12\", \"06:30\", \"06:48\", \"07:06\", \"07:23\", \"07:37\", \"07:51\", \"08:05\", \"08:19\", \"08:33\", \"08:47\", \"09:01\", \"09:15\", \"09:29\", \"09:43\", \"09:57\", \"10:11\", \"10:25\", \"10:39\", \"10:53\", \"11:07\", \"11:21\", \"11:35\", \"11:49\", \"12:03\", \"12:17\", \"12:31\", \"12:45\", \"12:59\", \"13:13\", \"13:27\", \"13:41\", \"13:55\", \"14:09\", \"14:23\", \"14:37\", \"14:51\", \"15:05\", \"15:19\", \"15:33\", \"15:47\", \"16:01\", \"16:15\", \"16:29\", \"16:43\", \"16:57\", \"17:11\", \"17:25\", \"17:39\", \"17:56\", \"18:14\", \"18:32\", \"18:50\", \"19:08\", \"19:26\", \"19:44\", \"20:02\", \"20:20\", \"20:38\", \"20:56\", \"21:14\", \"21:32\", \"21:50\", \"22:08\", \"22:26\", \"22:44\", \"23:02\", \"23:20\", \"23:40\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "5A", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:00\", \"05:20\", \"05:40\", \"05:50\", \"06:00\", \"06:10\", \"06:20\", \"06:30\", \"06:40\", \"06:50\", \"07:00\", \"07:10\", \"07:19\", \"07:28\", \"07:37\", \"07:46\", \"07:55\", \"08:04\", \"08:13\", \"08:26\", \"08:39\", \"08:52\", \"09:05\", \"09:18\", \"09:31\", \"09:44\", \"09:57\", \"10:10\", \"10:23\", \"10:36\", \"10:49\", \"11:02\", \"11:15\", \"11:28\", \"11:41\", \"11:54\", \"12:07\", \"12:20\", \"12:33\", \"12:43\", \"12:53\", \"13:02\", \"13:11\", \"13:20\", \"13:29\", \"13:38\", \"13:47\", \"13:56\", \"14:05\", \"14:14\", \"14:23\", \"14:32\", \"14:42\", \"14:52\", \"15:02\", \"15:12\", \"15:22\", \"15:32\", \"15:42\", \"15:55\", \"16:08\", \"16:21\", \"16:34\", \"16:47\", \"17:00\", \"17:13\", \"17:26\", \"17:39\", \"17:52\", \"18:05\", \"18:18\", \"18:32\", \"18:52\", \"19:12\", \"19:32\", \"19:52\", \"20:12\", \"20:32\", \"20:52\", \"21:12\", \"21:32\", \"21:52\", \"22:12\", \"22:32\", \"22:54\", \"23:16\", \"23:38\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:20\", \"05:40\", \"06:00\", \"06:20\", \"06:33\", \"06:46\", \"06:59\", \"07:12\", \"07:25\", \"07:38\", \"07:51\", \"08:04\", \"08:17\", \"08:30\", \"08:43\", \"08:56\", \"09:09\", \"09:22\", \"09:35\", \"09:48\", \"10:01\", \"10:14\", \"10:27\", \"10:40\", \"10:53\", \"11:06\", \"11:19\", \"11:32\", \"11:45\", \"11:58\", \"12:11\", \"12:24\", \"12:37\", \"12:50\", \"13:03\", \"13:16\", \"13:29\", \"13:42\", \"13:55\", \"14:08\", \"14:21\", \"14:34\", \"14:47\", \"15:00\", \"15:13\", \"15:26\", \"15:39\", \"15:52\", \"16:05\", \"16:18\", \"16:31\", \"16:44\", \"16:57\", \"17:10\", \"17:23\", \"17:36\", \"17:49\", \"18:02\", \"18:15\", \"18:28\", \"18:41\", \"19:00\", \"19:20\", \"19:40\", \"20:00\", \"20:20\", \"20:40\", \"21:00\", \"21:20\", \"21:40\", \"22:00\", \"22:20\", \"22:45\", \"23:10\", \"23:35\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:20\", \"05:40\", \"06:00\", \"06:20\", \"06:40\", \"07:00\", \"07:16\", \"07:32\", \"07:48\", \"08:04\", \"08:20\", \"08:36\", \"08:52\", \"09:08\", \"09:24\", \"09:40\", \"09:56\", \"10:12\", \"10:28\", \"10:44\", \"11:00\", \"11:16\", \"11:32\", \"11:48\", \"12:04\", \"12:20\", \"12:36\", \"12:52\", \"13:08\", \"13:24\", \"13:40\", \"13:56\", \"14:12\", \"14:28\", \"14:44\", \"15:00\", \"15:16\", \"15:32\", \"15:48\", \"16:04\", \"16:20\", \"16:36\", \"16:52\", \"17:08\", \"17:24\", \"17:40\", \"17:56\", \"18:12\", \"18:28\", \"18:44\", \"19:02\", \"19:20\", \"19:40\", \"20:00\", \"20:20\", \"20:40\", \"21:00\", \"21:20\", \"21:40\", \"22:00\", \"22:20\", \"22:40\", \"23:00\", \"23:20\", \"23:40\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "5B", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:00\", \"05:20\", \"05:40\", \"05:50\", \"06:00\", \"06:10\", \"06:20\", \"06:29\", \"06:38\", \"06:47\", \"06:56\", \"07:05\", \"07:14\", \"07:23\", \"07:32\", \"07:41\", \"07:50\", \"08:00\", \"08:13\", \"08:26\", \"08:39\", \"08:52\", \"09:05\", \"09:18\", \"09:31\", \"09:44\", \"09:57\", \"10:10\", \"10:23\", \"10:36\", \"10:49\", \"11:02\", \"11:15\", \"11:28\", \"11:41\", \"11:54\", \"12:07\", \"12:20\", \"12:33\", \"12:43\", \"12:53\", \"13:03\", \"13:13\", \"13:23\", \"13:33\", \"13:42\", \"13:51\", \"14:00\", \"14:09\", \"14:18\", \"14:27\", \"14:36\", \"14:45\", \"14:54\", \"15:03\", \"15:12\", \"15:22\", \"15:32\", \"15:42\", \"15:55\", \"16:08\", \"16:21\", \"16:34\", \"16:47\", \"17:00\", \"17:13\", \"17:26\", \"17:39\", \"17:52\", \"18:08\", \"18:24\", \"18:40\", \"18:56\", \"19:12\", \"19:32\", \"19:52\", \"20:12\", \"20:32\", \"20:52\", \"21:12\", \"21:32\", \"21:52\", \"22:12\", \"22:32\", \"22:54\", \"23:16\", \"23:38\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:20\", \"05:40\", \"06:00\", \"06:20\", \"06:33\", \"06:46\", \"06:59\", \"07:12\", \"07:25\", \"07:38\", \"07:51\", \"08:04\", \"08:17\", \"08:30\", \"08:43\", \"08:56\", \"09:09\", \"09:22\", \"09:35\", \"09:48\", \"10:01\", \"10:14\", \"10:27\", \"10:40\", \"10:53\", \"11:06\", \"11:19\", \"11:32\", \"11:45\", \"11:58\", \"12:11\", \"12:24\", \"12:37\", \"12:50\", \"13:03\", \"13:16\", \"13:29\", \"13:42\", \"13:55\", \"14:08\", \"14:21\", \"14:34\", \"14:47\", \"15:00\", \"15:13\", \"15:26\", \"15:39\", \"15:52\", \"16:05\", \"16:18\", \"16:31\", \"16:44\", \"16:57\", \"17:10\", \"17:23\", \"17:36\", \"17:49\", \"18:02\", \"18:15\", \"18:28\", \"18:41\", \"19:00\", \"19:20\", \"19:40\", \"20:00\", \"20:20\", \"20:40\", \"21:00\", \"21:20\", \"21:40\", \"22:00\", \"22:20\", \"22:45\", \"23:10\", \"23:35\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:20\", \"05:40\", \"06:00\", \"06:20\", \"06:36\", \"06:52\", \"07:08\", \"07:24\", \"07:40\", \"07:56\", \"08:12\", \"08:28\", \"08:44\", \"09:00\", \"09:16\", \"09:32\", \"09:48\", \"10:04\", \"10:20\", \"10:36\", \"10:52\", \"11:08\", \"11:24\", \"11:40\", \"11:56\", \"12:12\", \"12:28\", \"12:44\", \"13:00\", \"13:16\", \"13:32\", \"13:48\", \"14:04\", \"14:20\", \"14:36\", \"14:52\", \"15:08\", \"15:24\", \"15:40\", \"15:56\", \"16:12\", \"16:28\", \"16:44\", \"17:00\", \"17:16\", \"17:32\", \"17:48\", \"18:04\", \"18:20\", \"18:40\", \"19:00\", \"19:20\", \"19:40\", \"20:00\", \"20:20\", \"20:40\", \"21:00\", \"21:20\", \"21:40\", \"22:00\", \"22:20\", \"22:40\", \"23:00\", \"23:20\", \"23:40\", \"00:00\"]}", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "6A", TimeTable = "\"Radni_dan\": [\"04:30\", \"05:10\", \"05:37\", \"05:57\", \"06:10\", \"19:24\", \"19:49\", \"20:14\", \"20:39\", \"21:04\", \"21:29\", \"21:54\", \"22:19\", \"22:44\", \"23:09\", \"23:34\", \"00:00\"], \"Subota\": [\"05:00\", \"05:38\", \"21:51\", \"22:16\", \"22:41\", \"23:06\", \"23:33\", \"00:00\"], \"Nedelja\": [\"05:00\", \"05:37\", \"06:14\", \"06:51\", \"07:17\", \"22:46\", \"23:23\", \"00:00\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "6B", TimeTable = "\"Radni_dan\": [\"04:30\", \"05:00\", \"05:30\", \"05:50\", \"06:10\", \"06:23\", \"06:37\", \"06:50\", \"07:03\", \"07:10\", \"21:17\", \"21:42\", \"22:07\", \"22:32\", \"22:57\", \"23:25\", \"00:00\"], \"Subota\": [\"05:00\", \"05:25\", \"22:03\", \"22:28\", \"22:55\", \"23:25\", \"00:00\"], \"Nedelja\": [\"05:00\", \"05:37\", \"22:46\", \"23:23\", \"00:00\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "6AA", TimeTable = "\"Radni_dan\": [\"07:30\", \"12:55\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "6AB", TimeTable = "\"Radni_dan\": [\"11:40\", \"12:30\", \"13:20\", \"16:50\", \"17:40\", \"18:30\"]", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "7A", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:00\", \"05:25\", \"05:50\", \"06:03\", \"06:15\", \"06:28\", \"06:40\", \"06:53\", \"07:05\", \"07:18\", \"07:30\", \"07:43\", \"07:55\", \"08:08\", \"08:20\", \"08:33\", \"08:45\", \"08:58\", \"09:10\", \"09:23\", \"09:35\", \"09:48\", \"10:00\", \"10:13\", \"10:25\", \"10:38\", \"10:50\", \"11:03\", \"11:15\", \"11:28\", \"11:40\", \"11:53\", \"12:05\", \"12:18\", \"12:30\", \"12:43\", \"12:55\", \"13:08\", \"13:20\", \"13:33\", \"13:45\", \"13:58\", \"14:10\", \"14:23\", \"14:35\", \"14:48\", \"15:00\", \"15:13\", \"15:25\", \"15:38\", \"15:50\", \"16:03\", \"16:15\", \"16:28\", \"16:40\", \"16:53\", \"17:05\", \"17:18\", \"17:30\", \"17:43\", \"17:55\", \"18:08\", \"18:20\", \"18:33\", \"18:50\", \"19:07\", \"19:24\", \"19:41\", \"19:58\", \"20:15\", \"20:32\", \"20:49\", \"21:06\", \"21:23\", \"21:40\", \"21:57\", \"22:14\", \"22:31\", \"22:48\", \"23:10\", \"23:35\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:25\", \"05:50\", \"06:07\", \"06:24\", \"06:40\", \"06:53\", \"07:05\", \"07:18\", \"07:30\", \"07:43\", \"07:55\", \"08:08\", \"08:20\", \"08:33\", \"08:45\", \"08:58\", \"09:10\", \"09:23\", \"09:35\", \"09:48\", \"10:00\", \"10:13\", \"10:25\", \"10:38\", \"10:50\", \"11:03\", \"11:15\", \"11:28\", \"11:40\", \"11:53\", \"12:05\", \"12:18\", \"12:30\", \"12:43\", \"12:55\", \"13:11\", \"13:28\", \"13:45\", \"14:01\", \"14:18\", \"14:35\", \"14:51\", \"15:08\", \"15:25\", \"15:41\", \"15:58\", \"16:15\", \"16:31\", \"16:48\", \"17:05\", \"17:21\", \"17:38\", \"17:55\", \"18:11\", \"18:28\", \"18:45\", \"19:01\", \"19:18\", \"19:35\", \"19:51\", \"20:08\", \"20:25\", \"20:41\", \"20:58\", \"21:15\", \"21:31\", \"21:48\", \"22:05\", \"22:24\", \"22:48\", \"23:12\", \"23:36\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:25\", \"05:50\", \"06:07\", \"06:24\", \"06:41\", \"06:58\", \"07:15\", \"07:32\", \"07:49\", \"08:06\", \"08:23\", \"08:40\", \"21:25\", \"21:42\", \"21:59\", \"22:16\", \"22:33\", \"22:50\", \"23:10\", \"23:35\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "7B", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:00\", \"05:25\", \"05:50\", \"06:03\", \"06:15\", \"06:28\", \"06:40\", \"06:53\", \"07:05\", \"07:18\", \"07:30\", \"07:43\", \"07:55\", \"08:08\", \"08:20\", \"08:33\", \"08:45\", \"08:58\", \"09:10\", \"09:23\", \"09:35\", \"09:48\", \"10:00\", \"10:13\", \"10:25\", \"10:38\", \"10:50\", \"11:03\", \"11:15\", \"11:28\", \"11:40\", \"11:53\", \"12:05\", \"12:18\", \"12:30\", \"12:43\", \"12:55\", \"13:08\", \"13:20\", \"13:33\", \"13:45\", \"13:58\", \"14:10\", \"14:23\", \"14:35\", \"14:48\", \"15:00\", \"15:13\", \"15:25\", \"15:38\", \"15:50\", \"16:03\", \"16:15\", \"16:28\", \"16:40\", \"16:53\", \"17:05\", \"17:18\", \"17:30\", \"17:43\", \"17:55\", \"18:08\", \"18:20\", \"18:33\", \"18:50\", \"19:07\", \"19:24\", \"19:41\", \"19:58\", \"20:15\", \"20:32\", \"20:49\", \"21:06\", \"21:23\", \"21:40\", \"21:57\", \"22:14\", \"22:31\", \"22:48\", \"23:05\", \"23:22\", \"23:40\"], \"Subota\": [\"04:30\", \"05:00\", \"05:25\", \"05:50\", \"06:07\", \"06:24\", \"06:40\", \"06:53\", \"07:05\", \"07:18\", \"07:30\", \"07:43\", \"07:55\", \"08:08\", \"08:20\", \"08:33\", \"08:45\", \"08:58\", \"09:10\", \"09:23\", \"09:35\", \"09:48\", \"10:00\", \"10:13\", \"10:25\", \"10:38\", \"10:50\", \"11:03\", \"11:15\", \"11:28\", \"11:40\", \"11:53\", \"12:07\", \"12:18\", \"12:30\", \"12:43\", \"12:55\", \"13:11\", \"13:28\", \"13:45\", \"14:01\", \"14:18\", \"14:35\", \"14:51\", \"15:08\", \"15:25\", \"15:41\", \"15:58\", \"16:15\", \"16:31\", \"16:48\", \"17:05\", \"17:21\", \"17:38\", \"17:55\", \"18:11\", \"18:28\", \"18:45\", \"19:01\", \"19:18\", \"19:35\", \"19:51\", \"20:08\", \"20:25\", \"20:41\", \"20:58\", \"21:15\", \"21:31\", \"21:48\", \"22:05\", \"22:24\", \"22:43\", \"23:02\", \"23:21\", \"23:40\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:25\", \"05:50\", \"06:07\", \"06:24\", \"06:41\", \"06:58\", \"07:15\", \"07:32\", \"07:49\", \"08:06\", \"08:23\", \"08:40\", \"23:42\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "8A", TimeTable = "{\"Radni_dan\": [\"05:00\", \"05:20\", \"05:40\", \"05:50\", \"06:00\", \"06:10\", \"06:18\", \"06:26\", \"06:34\", \"06:42\", \"06:50\", \"06:58\", \"07:06\", \"07:14\", \"07:22\", \"07:30\", \"07:38\", \"07:46\", \"07:54\", \"08:02\", \"08:10\", \"08:24\", \"08:38\", \"08:52\", \"09:06\", \"09:20\", \"09:34\", \"09:48\", \"10:02\", \"10:16\", \"10:30\", \"10:44\", \"10:58\", \"11:12\", \"11:26\", \"11:40\", \"11:54\", \"12:08\", \"12:22\", \"12:36\", \"12:50\", \"12:58\", \"13:06\", \"13:14\", \"13:22\", \"13:30\", \"13:38\", \"13:46\", \"13:54\", \"14:02\", \"14:10\", \"14:18\", \"14:26\", \"14:34\", \"14:42\", \"14:50\", \"14:58\", \"15:06\", \"15:14\", \"15:22\", \"15:30\", \"15:38\", \"15:46\", \"15:54\", \"16:08\", \"16:22\", \"16:36\", \"16:50\", \"17:04\", \"17:18\", \"17:32\", \"17:46\", \"18:00\", \"18:14\", \"18:28\", \"18:42\", \"18:56\", \"19:10\", \"19:30\", \"19:50\", \"20:10\", \"20:30\", \"20:50\", \"21:10\", \"21:30\", \"21:55\", \"22:20\", \"22:45\", \"23:10\", \"23:35\", \"00:00\"], \"Subota\": [\"05:00\", \"05:20\", \"05:40\", \"06:00\", \"06:16\", \"06:32\", \"06:48\", \"07:04\", \"07:20\", \"07:36\", \"07:52\", \"08:06\", \"08:20\", \"08:34\", \"08:48\", \"09:02\", \"09:16\", \"09:30\", \"09:44\", \"09:58\", \"10:12\", \"10:26\", \"10:40\", \"10:54\", \"11:08\", \"11:22\", \"11:36\", \"11:50\", \"12:04\", \"12:20\", \"12:36\", \"12:52\", \"13:08\", \"13:24\", \"13:40\", \"13:56\", \"14:12\", \"14:28\", \"14:44\", \"15:00\", \"15:16\", \"15:32\", \"15:48\", \"16:04\", \"16:20\", \"16:36\", \"16:52\", \"17:08\", \"17:24\", \"17:40\", \"17:56\", \"18:12\", \"18:28\", \"18:44\", \"19:00\", \"19:20\", \"19:40\", \"20:00\", \"20:20\", \"20:40\", \"21:00\", \"21:20\", \"21:40\", \"22:00\", \"22:20\", \"22:40\", \"23:00\", \"23:20\", \"23:40\", \"00:00\"], \"Nedelja\": [\"05:00\", \"05:26\", \"05:53\", \"06:20\", \"06:46\", \"07:06\", \"07:26\", \"07:46\", \"08:06\", \"08:26\", \"08:46\", \"23:31\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "8B", TimeTable = "{\"Radni_dan\": [\"05:00\", \"05:20\", \"05:40\", \"05:50\", \"06:00\", \"06:10\", \"06:18\", \"06:26\", \"06:34\", \"06:42\", \"06:50\", \"06:58\", \"07:06\", \"07:14\", \"07:22\", \"07:30\", \"07:38\", \"07:46\", \"07:54\", \"08:02\", \"08:10\", \"08:24\", \"08:38\", \"08:52\", \"09:06\", \"09:20\", \"09:34\", \"09:48\", \"10:02\", \"10:16\", \"10:30\", \"10:44\", \"10:58\", \"11:12\", \"11:26\", \"11:40\", \"11:54\", \"12:08\", \"12:22\", \"12:36\", \"12:50\", \"12:58\", \"13:06\", \"13:14\", \"13:22\", \"13:30\", \"13:38\", \"13:46\", \"13:54\", \"14:02\", \"14:10\", \"14:18\", \"14:26\", \"14:34\", \"14:42\", \"14:50\", \"14:58\", \"15:06\", \"15:14\", \"15:22\", \"15:30\", \"15:38\", \"15:46\", \"15:54\", \"16:08\", \"16:22\", \"16:36\", \"16:50\", \"17:04\", \"17:18\", \"17:32\", \"17:46\", \"18:00\", \"18:14\", \"18:28\", \"18:42\", \"18:56\", \"19:10\", \"19:30\", \"19:50\", \"20:10\", \"20:30\", \"20:50\", \"21:10\", \"21:30\", \"21:55\", \"22:20\", \"22:45\", \"23:10\", \"23:35\", \"00:00\"], \"Subota\": [\"05:00\", \"05:20\", \"05:40\", \"06:00\", \"06:20\", \"06:40\", \"06:56\", \"07:10\", \"07:24\", \"07:38\", \"07:52\", \"08:06\", \"08:20\", \"08:34\", \"08:48\", \"09:02\", \"09:16\", \"09:30\", \"09:44\", \"09:58\", \"10:12\", \"10:26\", \"10:40\", \"10:54\", \"11:08\", \"11:22\", \"11:36\", \"11:50\", \"12:04\", \"12:18\", \"12:32\", \"12:46\", \"13:00\", \"13:16\", \"13:32\", \"13:48\", \"14:04\", \"14:20\", \"14:36\", \"14:52\", \"15:08\", \"15:24\", \"15:40\", \"15:56\", \"16:12\", \"16:28\", \"16:44\", \"17:00\", \"17:16\", \"17:32\", \"17:48\", \"18:04\", \"18:20\", \"18:40\", \"19:00\", \"19:20\", \"19:40\", \"20:00\", \"20:20\", \"20:40\", \"21:00\", \"21:20\", \"21:40\", \"22:00\", \"22:20\", \"22:40\", \"23:00\", \"23:20\", \"23:40\", \"00:00\"], \"Nedelja\": [\"05:00\", \"05:40\", \"06:06\", \"06:26\", \"06:46\", \"07:06\", \"07:26\", \"07:46\", \"08:06\", \"08:26\", \"08:46\", \"09:06\", \"09:26\", \"09:46\", \"10:06\", \"10:26\", \"10:46\", \"11:06\", \"11:26\", \"11:46\", \"12:06\", \"12:26\", \"12:46\", \"13:06\", \"13:26\", \"13:46\", \"14:06\", \"14:26\", \"14:46\", \"15:06\", \"15:26\", \"15:46\", \"16:06\", \"16:26\", \"16:46\", \"17:06\", \"17:26\", \"17:46\", \"18:06\", \"18:26\", \"18:46\", \"19:12\", \"19:39\", \"20:07\", \"20:34\", \"21:01\", \"21:28\", \"21:55\", \"22:22\", \"22:51\", \"23:20\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "9A", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:10\", \"05:31\", \"05:52\", \"06:06\", \"06:20\", \"06:31\", \"06:41\", \"06:51\", \"07:01\", \"07:11\", \"07:21\", \"07:31\", \"07:41\", \"07:51\", \"08:01\", \"08:11\", \"08:21\", \"08:35\", \"08:49\", \"09:03\", \"09:17\", \"09:31\", \"09:45\", \"09:59\", \"10:13\", \"10:27\", \"10:41\", \"10:55\", \"11:09\", \"11:23\", \"11:37\", \"11:51\", \"12:05\", \"12:19\", \"12:33\", \"12:47\", \"13:00\", \"13:10\", \"13:20\", \"13:30\", \"13:40\", \"13:50\", \"14:00\", \"14:10\", \"14:20\", \"14:30\", \"14:40\", \"14:50\", \"15:00\", \"15:10\", \"15:20\", \"15:30\", \"15:40\", \"15:51\", \"16:05\", \"16:19\", \"16:33\", \"16:47\", \"17:01\", \"17:15\", \"17:29\", \"17:43\", \"17:57\", \"18:11\", \"18:25\", \"18:39\", \"18:53\", \"19:07\", \"19:21\", \"19:35\", \"19:49\", \"20:03\", \"20:17\", \"20:31\", \"20:45\", \"20:59\", \"21:13\", \"21:27\", \"21:41\", \"22:09\", \"22:37\", \"23:05\", \"23:33\", \"00:00\"], \"Subota\": [\"04:30\", \"05:10\", \"05:31\", \"05:52\", \"06:13\", \"06:34\", \"06:55\", \"07:16\", \"07:33\", \"07:47\", \"08:01\", \"08:15\", \"08:29\", \"08:43\", \"08:57\", \"09:11\", \"09:25\", \"09:39\", \"09:53\", \"10:07\", \"10:21\", \"10:35\", \"10:49\", \"11:03\", \"11:17\", \"11:31\", \"11:45\", \"11:59\", \"12:13\", \"12:27\", \"12:41\", \"12:55\", \"13:09\", \"13:23\", \"13:37\", \"13:54\", \"14:11\", \"14:28\", \"14:45\", \"15:02\", \"15:19\", \"15:36\", \"15:53\", \"16:10\", \"16:27\", \"16:44\", \"17:01\", \"17:18\", \"17:36\", \"17:57\", \"18:18\", \"18:39\", \"19:00\", \"19:21\", \"19:49\", \"20:17\", \"20:45\", \"21:13\", \"21:41\", \"22:09\", \"22:37\", \"23:05\", \"23:33\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:10\", \"05:31\", \"05:52\", \"06:13\", \"06:34\", \"06:55\", \"07:16\", \"07:37\", \"07:58\", \"08:19\", \"08:40\", \"09:01\", \"09:22\", \"09:43\", \"10:04\", \"10:25\", \"10:46\", \"11:07\", \"11:28\", \"11:49\", \"12:10\", \"12:31\", \"12:52\", \"13:13\", \"13:34\", \"13:55\", \"14:16\", \"14:37\", \"14:58\", \"15:19\", \"15:40\", \"16:01\", \"16:22\", \"16:43\", \"17:04\", \"17:25\", \"17:53\", \"18:21\", \"18:49\", \"19:17\", \"19:45\", \"20:13\", \"20:41\", \"21:09\", \"21:37\", \"22:05\", \"22:33\", \"23:01\", \"23:30\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "9B", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:10\", \"05:31\", \"05:52\", \"06:06\", \"06:20\", \"06:32\", \"06:43\", \"06:54\", \"07:05\", \"07:16\", \"07:26\", \"07:36\", \"07:46\", \"07:56\", \"08:07\", \"08:21\", \"08:35\", \"08:49\", \"09:03\", \"09:17\", \"09:31\", \"09:45\", \"09:59\", \"10:13\", \"10:27\", \"10:41\", \"10:55\", \"11:09\", \"11:23\", \"11:37\", \"11:51\", \"12:05\", \"12:19\", \"12:33\", \"12:45\", \"12:55\", \"13:05\", \"13:15\", \"13:25\", \"13:35\", \"13:45\", \"13:55\", \"14:05\", \"14:15\", \"14:25\", \"14:35\", \"14:45\", \"14:55\", \"15:05\", \"15:15\", \"15:25\", \"15:35\", \"15:45\", \"15:55\", \"16:05\", \"16:19\", \"16:33\", \"16:47\", \"17:01\", \"17:15\", \"17:29\", \"17:43\", \"17:57\", \"18:11\", \"18:25\", \"18:39\", \"18:53\", \"19:07\", \"19:21\", \"19:35\", \"19:49\", \"20:03\", \"20:17\", \"20:31\", \"20:45\", \"20:59\", \"21:13\", \"21:27\", \"21:41\", \"21:55\", \"22:23\", \"22:51\", \"23:20\", \"00:00\"], \"Subota\": [\"04:30\", \"05:10\", \"05:31\", \"05:52\", \"06:13\", \"06:34\", \"06:51\", \"07:08\", \"07:25\", \"07:42\", \"07:59\", \"08:15\", \"08:29\", \"08:43\", \"08:57\", \"09:11\", \"09:25\", \"09:39\", \"09:53\", \"10:07\", \"10:21\", \"10:35\", \"10:49\", \"11:03\", \"11:17\", \"11:31\", \"11:45\", \"11:59\", \"12:13\", \"12:27\", \"12:41\", \"12:55\", \"13:12\", \"13:29\", \"13:46\", \"14:03\", \"14:20\", \"14:37\", \"14:54\", \"15:11\", \"15:28\", \"15:45\", \"16:02\", \"16:19\", \"16:36\", \"16:53\", \"17:10\", \"17:27\", \"17:44\", \"18:01\", \"18:18\", \"18:39\", \"19:07\", \"19:35\", \"20:03\", \"20:31\", \"20:59\", \"21:27\", \"21:55\", \"22:23\", \"22:51\", \"23:20\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:10\", \"05:31\", \"05:52\", \"06:13\", \"06:34\", \"06:55\", \"07:16\", \"07:37\", \"07:58\", \"08:19\", \"08:40\", \"09:01\", \"09:22\", \"09:43\", \"10:04\", \"10:25\", \"10:46\", \"11:07\", \"11:28\", \"11:49\", \"12:10\", \"12:31\", \"12:52\", \"13:13\", \"13:34\", \"13:55\", \"14:16\", \"14:37\", \"14:58\", \"15:19\", \"15:40\", \"16:01\", \"16:22\", \"16:43\", \"17:04\", \"17:25\", \"17:46\", \"18:07\", \"18:35\", \"19:03\", \"19:31\", \"19:59\", \"20:27\", \"20:55\", \"21:23\", \"21:51\", \"22:19\", \"22:48\", \"23:20\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "10A", TimeTable = "{\"Radni_dan\": [\"05:20\", \"05:25\", \"05:26\", \"05:30\", \"05:35\", \"06:15\", \"06:40\", \"07:35\", \"07:40\", \"13:25\", \"13:25\", \"13:30\", \"13:35\", \"21:25\", \"21:25\", \"21:35\"], \"Subota\": [\"05:35\", \"13:35\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "10B", TimeTable = "{\"Radni_dan\": [\"06:05\", \"06:15\", \"06:20\", \"14:10\", \"14:15\", \"14:15\", \"14:20\", \"15:05\", \"16:10\", \"16:15\", \"22:15\", \"22:15\", \"22:20\"], \"Subota\": [\"06:10\", \"14:10\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "11A", TimeTable = "{\"Radni_dan\": [\"05:00\", \"05:15\", \"05:30\", \"05:43\", \"05:55\", \"06:08\", \"06:20\", \"06:33\", \"06:45\", \"06:58\", \"07:10\", \"07:20\", \"07:23\", \"07:30\", \"07:35\", \"07:40\", \"07:48\", \"08:00\", \"08:13\", \"08:25\", \"08:37\", \"08:49\", \"09:01\", \"09:13\", \"09:25\", \"09:37\", \"09:49\", \"10:01\", \"10:13\", \"10:25\", \"10:37\", \"10:49\", \"11:01\", \"11:13\", \"11:25\", \"11:37\", \"11:49\", \"12:01\", \"12:13\", \"12:25\", \"12:37\", \"12:50\", \"13:02\", \"13:15\", \"13:27\", \"13:40\", \"13:52\", \"14:05\", \"14:17\", \"14:30\", \"14:42\", \"14:55\", \"15:07\", \"15:20\", \"15:32\", \"15:45\", \"15:57\", \"16:10\", \"16:22\", \"16:34\", \"16:46\", \"16:58\", \"17:10\", \"17:22\", \"17:34\", \"17:46\", \"17:58\", \"18:10\", \"18:26\", \"18:42\", \"18:58\", \"19:14\", \"19:30\", \"19:46\", \"20:02\", \"20:18\", \"20:38\", \"21:00\", \"21:22\", \"21:44\", \"22:06\", \"22:28\", \"22:50\", \"23:12\", \"23:36\", \"00:00\"], \"Subota\": [\"05:00\", \"05:22\", \"05:45\", \"06:07\", \"06:30\", \"06:52\", \"07:07\", \"07:22\", \"07:37\", \"07:52\", \"08:07\", \"08:22\", \"08:37\", \"08:52\", \"09:07\", \"09:22\", \"09:37\", \"09:52\", \"10:07\", \"10:22\", \"10:37\", \"10:52\", \"11:07\", \"11:22\", \"11:37\", \"11:52\", \"12:07\", \"12:22\", \"12:37\", \"12:52\", \"13:07\", \"13:22\", \"13:37\", \"13:52\", \"14:07\", \"14:22\", \"14:37\", \"14:52\", \"15:07\", \"15:22\", \"15:37\", \"15:52\", \"16:07\", \"16:22\", \"16:37\", \"16:52\", \"17:07\", \"17:22\", \"17:37\", \"17:52\", \"18:07\", \"18:22\", \"18:37\", \"18:52\", \"19:07\", \"19:22\", \"19:44\", \"20:06\", \"20:28\", \"20:50\", \"21:12\", \"21:34\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"], \"Nedelja\": [\"05:00\", \"05:22\", \"05:45\", \"06:07\", \"06:30\", \"06:52\", \"07:07\", \"07:22\", \"07:37\", \"07:52\", \"08:07\", \"08:22\", \"08:37\", \"08:52\", \"09:07\", \"09:22\", \"09:37\", \"09:52\", \"10:07\", \"10:22\", \"10:37\", \"10:52\", \"11:07\", \"11:22\", \"11:37\", \"11:52\", \"12:07\", \"12:22\", \"12:37\", \"12:52\", \"13:07\", \"13:22\", \"13:37\", \"13:52\", \"14:07\", \"14:22\", \"14:37\", \"14:52\", \"15:07\", \"15:22\", \"15:37\", \"15:52\", \"16:07\", \"16:22\", \"16:37\", \"16:52\", \"17:07\", \"17:22\", \"17:37\", \"17:52\", \"18:07\", \"18:22\", \"18:37\", \"18:52\", \"19:07\", \"19:22\", \"19:44\", \"20:06\", \"20:28\", \"20:50\", \"21:12\", \"21:34\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "11B", TimeTable = "{\"Radni_dan\": [\"05:00\", \"05:15\", \"05:27\", \"05:39\", \"05:51\", \"06:03\", \"06:15\", \"06:27\", \"06:39\", \"06:51\", \"07:03\", \"07:15\", \"07:27\", \"07:43\", \"07:59\", \"08:15\", \"08:31\", \"08:47\", \"09:03\", \"09:19\", \"09:35\", \"09:51\", \"10:07\", \"10:23\", \"10:39\", \"10:55\", \"11:11\", \"11:27\", \"11:43\", \"11:59\", \"12:15\", \"12:31\", \"12:47\", \"12:59\", \"13:11\", \"13:23\", \"13:35\", \"13:47\", \"13:59\", \"14:00\", \"14:11\", \"14:23\", \"14:35\", \"14:47\", \"14:59\", \"15:11\", \"15:23\", \"15:35\", \"15:51\", \"16:07\", \"16:23\", \"16:39\", \"16:55\", \"17:11\", \"17:27\", \"17:43\", \"17:59\", \"18:15\", \"18:31\", \"18:47\", \"19:03\", \"19:19\", \"19:35\", \"19:51\", \"20:07\", \"20:23\", \"20:39\", \"20:55\", \"21:11\", \"21:27\", \"21:43\", \"22:00\", \"22:24\", \"22:48\", \"23:12\", \"23:36\", \"00:00\"], \"Subota\": [\"05:00\", \"05:22\", \"05:45\", \"06:07\", \"06:30\", \"06:52\", \"07:07\", \"07:22\", \"07:37\", \"07:52\", \"08:07\", \"08:22\", \"08:37\", \"08:52\", \"09:07\", \"09:22\", \"09:37\", \"09:52\", \"10:07\", \"10:22\", \"10:37\", \"10:52\", \"11:07\", \"11:22\", \"11:37\", \"11:52\", \"12:07\", \"12:22\", \"12:37\", \"12:52\", \"13:07\", \"13:22\", \"13:37\", \"13:52\", \"14:07\", \"14:22\", \"14:37\", \"14:52\", \"15:07\", \"15:22\", \"15:37\", \"15:52\", \"16:07\", \"16:22\", \"16:37\", \"16:52\", \"17:07\", \"17:22\", \"17:37\", \"17:52\", \"18:07\", \"18:22\", \"18:37\", \"18:52\", \"19:07\", \"19:22\", \"19:44\", \"20:06\", \"20:28\", \"20:50\", \"21:12\", \"21:34\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"], \"Nedelja\": [\"05:00\", \"05:22\", \"05:45\", \"06:07\", \"06:30\", \"06:52\", \"07:07\", \"07:22\", \"07:37\", \"07:52\", \"08:07\", \"08:22\", \"08:37\", \"08:52\", \"09:07\", \"09:22\", \"09:37\", \"09:52\", \"10:07\", \"10:22\", \"10:37\", \"10:52\", \"11:07\", \"11:22\", \"11:37\", \"11:52\", \"12:07\", \"12:22\", \"12:37\", \"12:52\", \"13:07\", \"13:22\", \"13:37\", \"13:52\", \"14:07\", \"14:22\", \"14:37\", \"14:52\", \"15:07\", \"15:22\", \"15:37\", \"15:52\", \"16:07\", \"16:22\", \"16:37\", \"16:52\", \"17:07\", \"17:22\", \"17:37\", \"17:52\", \"18:07\", \"18:22\", \"18:37\", \"18:52\", \"19:07\", \"19:22\", \"19:44\", \"20:06\", \"20:28\", \"20:50\", \"21:12\", \"21:34\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "12A", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:00\", \"05:27\", \"05:45\", \"06:00\", \"06:15\", \"06:30\", \"06:45\", \"07:00\", \"07:15\", \"07:30\", \"07:45\", \"08:00\", \"08:15\", \"08:33\", \"08:51\", \"09:09\", \"09:27\", \"09:45\", \"10:03\", \"10:21\", \"10:39\", \"10:57\", \"11:15\", \"11:33\", \"11:51\", \"12:09\", \"12:27\", \"12:45\", \"13:03\", \"13:21\", \"13:39\", \"13:54\", \"14:09\", \"14:24\", \"14:39\", \"14:54\", \"15:09\", \"15:24\", \"15:39\", \"15:54\", \"16:09\", \"16:27\", \"16:45\", \"17:03\", \"17:21\", \"17:39\", \"17:57\", \"18:15\", \"18:33\", \"18:51\", \"19:09\", \"19:27\", \"19:45\", \"20:03\", \"20:21\", \"20:39\", \"20:57\", \"21:15\", \"21:33\", \"21:51\", \"22:09\", \"22:27\", \"22:45\", \"23:03\", \"23:21\", \"23:40\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:26\", \"05:52\", \"06:17\", \"06:35\", \"06:49\", \"07:02\", \"07:16\", \"07:29\", \"07:43\", \"07:56\", \"08:10\", \"08:23\", \"08:37\", \"08:50\", \"09:04\", \"09:17\", \"09:31\", \"09:44\", \"09:58\", \"10:11\", \"10:25\", \"10:38\", \"10:52\", \"11:05\", \"11:19\", \"11:32\", \"11:50\", \"12:08\", \"12:26\", \"12:46\", \"13:12\", \"13:38\", \"14:04\", \"14:30\", \"14:56\", \"15:14\", \"15:32\", \"15:50\", \"16:08\", \"16:26\", \"16:44\", \"17:02\", \"17:20\", \"17:38\", \"17:56\", \"18:14\", \"18:32\", \"18:50\", \"19:08\", \"19:26\", \"19:44\", \"20:02\", \"20:20\", \"20:40\", \"21:06\", \"21:33\", \"22:00\", \"22:27\", \"22:54\", \"23:12\", \"23:30\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:28\", \"05:46\", \"06:04\", \"06:22\", \"06:40\", \"06:58\", \"07:16\", \"07:34\", \"07:52\", \"08:10\", \"08:28\", \"08:46\", \"09:04\", \"09:22\", \"09:40\", \"09:58\", \"10:16\", \"10:34\", \"10:52\", \"11:10\", \"11:28\", \"11:46\", \"12:04\", \"12:22\", \"12:42\", \"13:08\", \"13:34\", \"14:00\", \"14:26\", \"14:52\", \"15:10\", \"15:28\", \"15:46\", \"16:04\", \"16:22\", \"16:40\", \"16:58\", \"17:16\", \"17:34\", \"17:52\", \"18:10\", \"18:28\", \"18:46\", \"19:04\", \"19:22\", \"19:40\", \"19:58\", \"20:16\", \"20:34\", \"21:00\", \"21:27\", \"21:54\", \"22:21\", \"22:48\", \"23:06\", \"23:24\", \"23:42\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "12B", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:00\", \"05:18\", \"05:36\", \"05:54\", \"06:12\", \"06:30\", \"06:45\", \"07:00\", \"07:15\", \"07:30\", \"07:48\", \"08:06\", \"08:24\", \"08:42\", \"09:00\", \"09:18\", \"09:36\", \"09:54\", \"10:12\", \"10:30\", \"10:48\", \"11:06\", \"11:24\", \"11:42\", \"12:00\", \"12:18\", \"12:36\", \"12:54\", \"13:12\", \"13:30\", \"13:48\", \"14:06\", \"14:24\", \"14:39\", \"14:54\", \"15:09\", \"15:24\", \"15:42\", \"16:00\", \"16:18\", \"16:36\", \"16:54\", \"17:12\", \"17:30\", \"17:48\", \"18:06\", \"18:24\", \"18:42\", \"19:00\", \"19:18\", \"19:36\", \"19:54\", \"20:12\", \"20:30\", \"20:48\", \"21:06\", \"21:24\", \"21:42\", \"22:00\", \"22:18\", \"22:36\", \"22:54\", \"23:13\", \"23:33\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:26\", \"05:52\", \"06:17\", \"06:42\", \"07:01\", \"07:15\", \"07:28\", \"07:42\", \"07:55\", \"08:09\", \"08:22\", \"08:36\", \"08:49\", \"09:03\", \"09:16\", \"09:30\", \"09:43\", \"09:57\", \"10:10\", \"10:24\", \"10:37\", \"10:51\", \"11:04\", \"11:18\", \"11:31\", \"11:45\", \"11:58\", \"12:16\", \"12:34\", \"12:52\", \"13:12\", \"13:38\", \"14:04\", \"14:30\", \"14:56\", \"15:22\", \"15:40\", \"15:58\", \"16:16\", \"16:34\", \"16:52\", \"17:10\", \"17:28\", \"17:46\", \"18:04\", \"18:22\", \"18:40\", \"18:58\", \"19:16\", \"19:34\", \"19:52\", \"20:10\", \"20:28\", \"20:46\", \"21:06\", \"21:33\", \"22:00\", \"22:27\", \"22:54\", \"23:21\", \"23:40\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:18\", \"05:36\", \"05:54\", \"06:12\", \"06:30\", \"06:48\", \"07:06\", \"07:24\", \"07:42\", \"08:00\", \"08:18\", \"08:36\", \"08:54\", \"09:12\", \"09:30\", \"09:48\", \"10:06\", \"10:24\", \"10:42\", \"11:00\", \"11:18\", \"11:36\", \"11:54\", \"12:12\", \"12:30\", \"12:48\", \"13:08\", \"13:34\", \"14:00\", \"14:26\", \"14:52\", \"15:18\", \"15:36\", \"15:54\", \"16:12\", \"16:30\", \"16:48\", \"17:06\", \"17:24\", \"17:42\", \"18:00\", \"18:18\", \"18:36\", \"18:54\", \"19:12\", \"19:30\", \"19:48\", \"20:06\", \"20:24\", \"20:42\", \"21:00\", \"21:27\", \"21:54\", \"22:21\", \"22:48\", \"23:15\", \"23:35\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "13A", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:00\", \"05:30\", \"06:00\", \"06:20\", \"06:40\", \"07:00\", \"07:20\", \"07:22\", \"07:40\", \"08:00\", \"08:30\", \"09:00\", \"09:30\", \"10:00\", \"10:30\", \"11:00\", \"11:30\", \"12:00\", \"12:30\", \"13:00\", \"13:30\", \"13:50\", \"14:10\", \"14:30\", \"14:50\", \"15:10\", \"15:30\", \"16:00\", \"16:30\", \"17:00\", \"17:30\", \"18:00\", \"18:30\", \"19:00\", \"19:30\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:30\", \"06:00\", \"06:30\", \"07:00\", \"07:30\", \"08:00\", \"08:30\", \"09:00\", \"09:30\", \"10:00\", \"10:30\", \"11:00\", \"11:30\", \"12:00\", \"12:30\", \"13:00\", \"13:30\", \"14:00\", \"14:30\", \"15:00\", \"15:30\", \"16:00\", \"16:30\", \"17:00\", \"17:30\", \"18:00\", \"18:30\", \"19:00\", \"19:30\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:30\", \"06:00\", \"06:30\", \"07:00\", \"07:30\", \"08:00\", \"08:30\", \"09:00\", \"09:30\", \"10:00\", \"10:30\", \"11:00\", \"11:30\", \"12:00\", \"12:30\", \"13:00\", \"13:30\", \"14:00\", \"14:30\", \"15:00\", \"15:30\", \"16:00\", \"16:30\", \"17:00\", \"17:30\", \"18:00\", \"18:30\", \"19:00\", \"19:30\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "13B", TimeTable = "{\"Radni_dan\": [\"05:00\", \"05:30\", \"06:00\", \"06:30\", \"06:50\", \"07:10\", \"07:30\", \"08:00\", \"08:30\", \"09:00\", \"09:30\", \"10:00\", \"10:30\", \"11:00\", \"11:30\", \"12:00\", \"12:30\", \"13:00\", \"13:20\", \"13:40\", \"14:00\", \"14:20\", \"14:40\", \"15:00\", \"15:20\", \"15:40\", \"16:00\", \"16:30\", \"17:00\", \"17:30\", \"18:00\", \"18:30\", \"19:00\", \"19:30\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"], \"Subota\": [\"05:00\", \"05:30\", \"06:00\", \"06:30\", \"07:00\", \"07:30\", \"08:00\", \"08:30\", \"09:00\", \"09:30\", \"10:00\", \"10:30\", \"11:00\", \"11:30\", \"12:00\", \"12:30\", \"13:00\", \"13:30\", \"14:00\", \"14:30\", \"15:00\", \"15:30\", \"16:00\", \"16:30\", \"17:00\", \"17:30\", \"18:00\", \"18:30\", \"19:00\", \"19:30\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"], \"Nedelja\": [\"05:00\", \"05:30\", \"06:00\", \"06:30\", \"07:00\", \"07:30\", \"08:00\", \"08:30\", \"09:00\", \"09:30\", \"10:00\", \"10:30\", \"11:00\", \"11:30\", \"12:00\", \"12:30\", \"13:00\", \"13:30\", \"14:00\", \"14:30\", \"15:00\", \"15:30\", \"16:00\", \"16:30\", \"17:00\", \"17:30\", \"18:00\", \"18:30\", \"19:00\", \"19:30\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "14A", TimeTable = "{\"Radni_dan\": [\"05:00\", \"05:30\", \"06:00\", \"06:20\", \"06:38\", \"06:52\", \"07:06\", \"07:20\", \"07:34\", \"07:52\", \"08:11\", \"08:30\", \"08:48\", \"09:07\", \"09:26\", \"09:44\", \"10:03\", \"10:22\", \"10:40\", \"10:59\", \"11:18\", \"11:36\", \"11:55\", \"12:14\", \"12:32\", \"12:46\", \"13:00\", \"13:14\", \"13:28\", \"13:42\", \"13:56\", \"14:10\", \"14:24\", \"14:38\", \"14:52\", \"15:06\", \"15:20\", \"15:34\", \"15:48\", \"16:06\", \"16:25\", \"16:44\", \"17:02\", \"17:21\", \"17:40\", \"18:05\", \"18:30\", \"19:00\", \"19:30\", \"19:35\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"], \"Subota\": [\"05:00\", \"05:30\", \"06:00\", \"06:30\", \"07:00\", \"07:30\", \"08:00\", \"08:30\", \"09:00\", \"09:30\", \"10:00\", \"10:30\", \"11:00\", \"11:30\", \"12:00\", \"12:30\", \"13:00\", \"13:30\", \"14:00\", \"14:30\", \"15:00\", \"15:30\", \"16:00\", \"16:30\", \"17:00\", \"17:30\", \"18:00\", \"18:30\", \"19:00\", \"19:30\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"], \"Nedelja\": [\"05:00\", \"05:30\", \"06:00\", \"06:30\", \"07:00\", \"07:30\", \"08:00\", \"08:30\", \"09:00\", \"09:30\", \"10:00\", \"10:30\", \"11:00\", \"11:30\", \"12:00\", \"12:30\", \"13:00\", \"13:30\", \"14:00\", \"14:30\", \"15:00\", \"15:30\", \"16:00\", \"16:30\", \"17:00\", \"17:30\", \"18:00\", \"18:30\", \"19:00\", \"19:30\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "14B", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:00\", \"05:30\", \"05:50\", \"06:10\", \"06:24\", \"06:38\", \"06:52\", \"07:06\", \"07:20\", \"07:25\", \"07:30\", \"07:34\", \"07:48\", \"08:02\", \"08:20\", \"08:39\", \"08:58\", \"09:16\", \"09:35\", \"09:54\", \"10:12\", \"10:31\", \"10:50\", \"11:08\", \"11:27\", \"11:46\", \"12:04\", \"12:23\", \"12:42\", \"13:00\", \"13:14\", \"13:20\", \"13:25\", \"13:28\", \"13:42\", \"13:56\", \"14:10\", \"14:24\", \"14:38\", \"14:52\", \"15:06\", \"15:20\", \"15:38\", \"15:57\", \"16:16\", \"16:34\", \"16:53\", \"17:12\", \"17:37\", \"18:02\", \"18:30\", \"19:00\", \"19:30\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"], \"Subota\": [\"04:30\", \"05:00\", \"05:30\", \"06:00\", \"06:30\", \"07:00\", \"07:30\", \"08:00\", \"08:30\", \"09:00\", \"09:30\", \"10:00\", \"10:30\", \"11:00\", \"11:30\", \"12:00\", \"12:30\", \"13:00\", \"13:30\", \"14:00\", \"14:30\", \"15:00\", \"15:30\", \"16:00\", \"16:30\", \"17:00\", \"17:30\", \"18:00\", \"18:30\", \"19:00\", \"19:30\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"], \"Nedelja\": [\"04:30\", \"05:00\", \"05:30\", \"06:00\", \"06:30\", \"07:00\", \"07:30\", \"08:00\", \"08:30\", \"09:00\", \"09:30\", \"10:00\", \"10:30\", \"11:00\", \"11:30\", \"12:00\", \"12:30\", \"13:00\", \"13:30\", \"14:00\", \"14:30\", \"15:00\", \"15:30\", \"16:00\", \"16:30\", \"17:00\", \"17:30\", \"18:00\", \"18:30\", \"19:00\", \"19:30\", \"20:00\", \"20:30\", \"21:00\", \"21:30\", \"22:00\", \"22:30\", \"23:00\", \"23:30\", \"00:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "15A", TimeTable = "{\"Radni_dan\": [\"05:20\", \"05:30\", \"06:05\", \"06:35\", \"07:00\", \"07:30\", \"09:30\", \"12:30\", \"13:20\", \"17:20\", \"21:30\"], \"Subota\": [\"05:20\", \"05:35\", \"06:40\", \"13:20\", \"17:20\", \"21:20\"], \"Nedelja\": [\"05:30\", \"13:20\", \"17:20\", \"21:20\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "15B", TimeTable = "{\"Radni_dan\": [\"06:15\", \"07:30\", \"10:05\", \"13:20\", \"14:15\", \"15:10\", \"16:10\", \"18:15\", \"21:20\", \"22:15\"], \"Subota\": [\"06:10\", \"06:15\", \"14:15\", \"18:15\", \"22:15\"], \"Nedelja\": [\"06:15\", \"14:15\", \"18:15\", \"22:15\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "16A", TimeTable = "{\"Radni_dan\": [\"05:40\", \"06:45\", \"07:45\", \"13:35\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "16B", TimeTable = "{\"Radni_dan\": [\"06:15\", \"14:15\", \"16:10\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "17A", TimeTable = "{\"Radni_dan\": [\"09:50\", \"10:25\", \"11:00\", \"11:35\", \"12:10\", \"12:45\", \"13:20\", \"13:55\", \"14:30\", \"15:05\", \"15:40\", \"16:15\", \"16:50\", \"17:25\", \"18:00\", \"18:35\", \"19:10\", \"19:45\", \"20:20\", \"20:55\", \"21:30\", \"22:00\", \"22:30\"], \"Subota\": [\"09:50\", \"10:25\", \"11:00\", \"11:35\", \"12:10\", \"12:45\", \"13:20\", \"13:55\", \"14:30\", \"15:05\", \"15:40\", \"16:15\", \"16:50\", \"17:25\", \"18:00\", \"18:35\", \"19:10\", \"19:45\", \"20:20\", \"20:55\", \"21:30\", \"22:00\", \"22:30\"], \"Nedelja\": [\"09:50\", \"10:25\", \"11:00\", \"11:35\", \"12:10\", \"12:45\", \"13:20\", \"13:55\", \"14:30\", \"15:05\", \"15:40\", \"16:15\", \"16:50\", \"17:25\", \"18:00\", \"18:35\", \"19:10\", \"19:45\", \"20:20\", \"20:55\", \"21:30\", \"22:00\", \"22:30\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "17B", TimeTable = "{\"Radni_dan\": [\"09:30\", \"10:05\", \"10:40\", \"11:15\", \"11:50\", \"12:25\", \"13:00\", \"13:35\", \"14:10\", \"14:45\", \"15:20\", \"15:55\", \"16:30\", \"17:05\", \"17:40\", \"18:15\", \"18:50\", \"19:25\", \"20:00\", \"20:35\", \"21:10\", \"21:45\", \"22:15\"], \"Subota\": [\"09:30\", \"10:05\", \"10:40\", \"11:15\", \"11:50\", \"12:25\", \"13:00\", \"13:35\", \"14:10\", \"14:45\", \"15:20\", \"15:55\", \"16:30\", \"17:05\", \"17:40\", \"18:15\", \"18:50\", \"19:25\", \"20:00\", \"20:35\", \"21:10\", \"21:45\", \"22:15\"], \"Nedelja\": [\"09:30\", \"10:05\", \"10:40\", \"11:15\", \"11:50\", \"12:25\", \"13:00\", \"13:35\", \"14:10\", \"14:45\", \"15:20\", \"15:55\", \"16:30\", \"17:05\", \"17:40\", \"18:15\", \"18:50\", \"19:25\", \"20:00\", \"20:35\", \"21:10\", \"21:45\", \"22:15\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "18A", TimeTable = "{\"Radni_dan\": [\"00:30\", \"01:30\", \"02:30\", \"03:30\"], \"Subota\": [\"00:30\", \"01:30\", \"02:30\", \"03:30\"], \"Nedelja\": [\"00:30\", \"01:30\", \"02:30\", \"03:30\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "18B", TimeTable = "{\"Radni_dan\": [\"00:00\", \"01:00\", \"02:00\", \"03:00\", \"04:00\"], \"Subota\": [\"00:00\", \"01:00\", \"02:00\", \"03:00\", \"04:00\"], \"Nedelja\": [\"00:00\", \"01:00\", \"02:00\", \"03:00\", \"04:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "20A", TimeTable = "{\"Radni_dan\": [\"07:30\", \"09:00\", \"11:30\", \"14:30\", \"17:00\", \"18:30\"], \"Subota\": [\"07:30\", \"09:00\", \"11:30\", \"14:30\", \"17:00\", \"18:30\"], \"Nedelja\": [\"07:30\", \"09:00\", \"12:00\", \"15:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "20B", TimeTable = "{\"Radni_dan\": [\"08:00\", \"09:30\", \"12:00\", \"15:00\", \"17:30\", \"19:30\", \"20:30\"], \"Subota\": [\"08:00\", \"09:30\", \"12:00\", \"15:00\", \"17:30\", \"19:30\", \"20:30\"], \"Nedelja\": [\"08:00\", \"09:30\", \"12:30\", \"16:30\", \"17:30\"]}", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "21A", TimeTable = "\"Radni_dan\": [\"04:35\", \"05:00\", \"05:35\", \"21:30\", \"22:45\"], \"Subota\": [\"04:35\", \"05:40\", \"06:40\", \"18:10\", \"19:20\", \"20:30\", \"21:30\", \"22:45\"], \"Nedelja\": [\"04:35\", \"05:40\", \"20:30\", \"21:30\", \"22:45\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "21B", TimeTable = "\"Radni_dan\": [\"04:55\", \"05:20\", \"06:10\", \"06:35\", \"07:00\", \"07:20\", \"07:50\", \"16:50\", \"18:05\", \"19:10\", \"19:50\", \"21:00\", \"22:05\", \"23:05\"], \"Subota\": [\"04:55\",\"20:50\", \"22:05\", \"23:05\"], \"Nedelja\": [\"04:55\", \"05:30\", \"06:05\", \"20:50\", \"22:05\", \"23:05\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "22A", TimeTable = "\"Radni_dan\": [\"05:20\", \"06:10\", \"07:15\", \"21:25\"], \"Subota\": [\"08:40\", \"09:50\", \"11:00\", \"13:00\", \"18:30\", \"22:20\"], \"Nedelja\": [\"08:40\", \"09:50\", \"11:00\", \"13:00\", \"18:25\", \"22:30\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "22B", TimeTable = "\"Radni_dan\": [\"05:05\", \"05:55\", \"06:15\", \"06:30\", \"06:45\", \"19:35\", \"20:00\"], \"Subota\": [\"05:10\", \"06:15\", \"07:15\", \"08:05\", \"09:15\", \"10:25\", \"11:35\", \"13:35\", \"19:00\", \"22:55\"], \"Nedelja\": [\"05:10\", \"09:15\", \"10:25\", \"11:35\", \"13:35\", \"19:00\", \"23:00\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "23A", TimeTable = "\"Radni_dan\": [\"04:40\", \"07:05\", \"21:00\", \"23:30\"], \"Subota\": [\"07:20\", \"12:00\", \"14:10\", \"14:55\", \"15:30\", \"16:05\", \"16:50\", \"17:55\", \"20:25\"], \"Nedelja\": [\"07:20\", \"12:00\", \"15:50\", \"20:20\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "23B", TimeTable = "\"Radni_dan\": [\"04:50\", \"05:10\", \"06:00\", \"21:35\", \"03:55\"], \"Subota\": [\"04:50\", \"05:15\",  \"21:05\",], \"Nedelja\": [\"06:10\", \"07:55\", \"12:40\", \"16:30\", \"21:00\", \"03:55\"]", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "24A", TimeTable = "{\"Radni_dan\": [\"04:00\", \"05:00\", \"05:35\", \"06:40\", \"07:30\", \"08:30\", \"09:30\", \"10:30\", \"11:30\", \"12:25\", \"13:05\", \"13:30\", \"14:05\", \"14:15\", \"14:40\", \"15:15\", \"15:35\", \"16:10\", \"16:40\", \"17:15\", \"18:00\", \"18:35\", \"19:35\", \"20:25\", \"21:30\", \"22:20\", \"22:45\", \"01:15\"], \"Subota\": [\"04:00\", \"05:10\", \"06:00\", \"07:00\", \"08:10\", \"09:20\", \"10:20\", \"11:30\", \"12:30\", \"13:20\", \"13:50\", \"14:20\", \"15:05\", \"16:10\", \"17:00\", \"18:00\", \"19:00\", \"20:00\", \"21:00\", \"22:45\", \"01:15\"], \"Nedelja\": [\"04:00\", \"05:10\", \"06:00\", \"07:00\", \"08:10\", \"09:20\", \"10:20\", \"11:30\", \"12:20\", \"13:20\", \"13:50\", \"14:20\", \"15:00\", \"16:10\", \"17:00\", \"18:00\", \"19:00\", \"20:00\", \"21:00\", \"22:45\", \"01:15\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "24B", TimeTable = "{\"Radni_dan\": [\"04:15\", \"04:45\", \"04:50\", \"05:10\", \"05:45\", \"06:00\", \"06:30\", \"06:50\", \"07:40\", \"08:30\", \"09:30\", \"10:30\", \"11:30\", \"12:30\", \"13:25\", \"14:05\", \"14:30\", \"15:15\", \"15:40\", \"16:15\", \"16:35\", \"17:10\", \"17:40\", \"18:15\", \"18:55\", \"19:30\", \"20:35\", \"21:20\", \"22:20\", \"23:35\", \"02:45\"], \"Subota\": [\"04:30\", \"05:00\", \"06:10\", \"07:00\", \"08:00\", \"09:10\", \"10:20\", \"11:20\", \"12:30\", \"13:20\", \"14:20\", \"14:50\", \"15:10\", \"16:00\", \"17:10\", \"18:00\", \"19:00\", \"20:00\", \"21:00\", \"21:50\", \"23:30\", \"02:45\"], \"Nedelja\": [\"04:30\", \"05:00\", \"06:10\", \"07:00\", \"08:00\", \"09:10\", \"10:20\", \"11:20\", \"12:30\", \"13:20\", \"14:20\", \"14:50\", \"15:20\", \"16:00\", \"17:10\", \"18:00\", \"19:00\", \"20:00\", \"21:00\", \"21:50\", \"23:30\", \"02:45\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "30A", TimeTable = "{\"Radni_dan\": [\"06:15\", \"09:30\", \"12:30\", \"15:00\", \"18:30\", \"20:30\"], \"Subota\": [\"06:15\", \"15:00\", \"21:00\"], \"Nedelja\": [\"08:30\", \"13:00\", \"21:10\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "30B", TimeTable = "{\"Radni_dan\": [\"06:45\", \"10:00\", \"13:00\", \"15:30\", \"19:00\", \"21:00\"], \"Subota\": [\"06:45\", \"15:30\", \"21:30\"], \"Nedelja\": [\"09:00\", \"13:30\", \"21:40\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "31A", TimeTable = "{\"Radni_dan\": [\"04:55\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "31B", TimeTable = "{\"Radni_dan\": [\"05:25\", \"06:35\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "32A", TimeTable = "{\"Radni_dan\": [\"05:15\", \"06:15\", \"06:30\", \"07:10\", \"07:50\", \"08:50\", \"10:20\", \"10:40\", \"11:20\", \"11:50\", \"12:10\", \"12:30\", \"13:10\", \"14:05\", \"14:05\", \"14:15\", \"14:35\", \"15:00\", \"15:30\", \"15:50\", \"16:10\", \"16:50\", \"17:30\", \"18:10\", \"18:40\", \"19:00\", \"19:20\", \"19:35\", \"20:20\", \"22:15\", \"01:15\"], \"Subota\": [\"04:40\", \"05:20\", \"06:15\", \"06:50\", \"08:05\", \"08:30\", \"09:00\", \"09:50\", \"10:20\", \"11:30\", \"12:30\", \"13:30\", \"14:10\", \"14:30\", \"15:10\", \"15:30\", \"16:40\", \"17:40\", \"18:20\", \"19:00\", \"20:40\", \"01:15\"], \"Nedelja\": [\"05:20\", \"06:00\", \"06:50\", \"08:05\", \"08:30\", \"09:00\", \"09:50\", \"10:20\", \"11:30\", \"12:30\", \"13:30\", \"14:10\", \"14:30\", \"15:10\", \"15:30\", \"16:40\", \"17:40\", \"18:20\", \"19:00\", \"20:40\", \"01:15\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "32B", TimeTable = "{\"Radni_dan\": [\"04:50\", \"05:10\", \"05:40\", \"05:50\", \"06:05\", \"06:15\", \"06:30\", \"07:05\", \"07:20\", \"07:30\", \"08:00\", \"08:40\", \"09:40\", \"11:10\", \"11:30\", \"12:10\", \"12:40\", \"13:00\", \"13:20\", \"13:45\", \"14:05\", \"14:50\", \"15:05\", \"15:25\", \"15:50\", \"16:20\", \"16:40\", \"17:00\", \"17:40\", \"19:00\", \"19:50\", \"20:15\", \"21:10\", \"21:55\", \"02:45\", \"04:00\"], \"Subota\": [\"05:00\", \"05:25\", \"06:00\", \"07:05\", \"07:40\", \"08:50\", \"09:20\", \"09:50\", \"10:40\", \"11:10\", \"12:20\", \"13:20\", \"14:20\", \"14:55\", \"15:20\", \"16:00\", \"16:20\", \"17:30\", \"18:30\", \"19:10\", \"19:50\", \"21:30\", \"02:45\", \"04:00\"], \"Nedelja\": [\"05:00\", \"05:25\", \"06:00\", \"06:50\", \"07:40\", \"08:50\", \"09:20\", \"09:50\", \"10:40\", \"11:10\", \"12:20\", \"13:20\", \"14:20\", \"15:00\", \"15:20\", \"16:00\", \"16:20\", \"17:30\", \"18:30\", \"19:10\", \"19:50\", \"21:30\", \"02:45\", \"04:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "33A", TimeTable = "{\"Radni_dan\": [\"04:10\", \"05:50\", \"06:20\", \"07:35\", \"09:35\", \"11:00\", \"12:40\", \"13:30\", \"14:30\", \"15:10\", \"16:20\", \"17:10\", \"18:25\", \"19:45\", \"20:40\", \"21:40\", \"22:45\"], \"Subota\": [\"04:50\", \"06:00\", \"07:10\", \"08:40\", \"10:40\", \"12:40\", \"14:40\", \"15:45\", \"17:05\", \"19:20\", \"21:40\", \"22:45\"], \"Nedelja\": [\"04:50\", \"06:10\", \"07:10\", \"08:40\", \"10:40\", \"12:40\", \"14:40\", \"15:45\", \"17:05\", \"19:20\", \"21:40\", \"22:45\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "33B", TimeTable = "{\"Radni_dan\": [\"04:45\", \"05:15\", \"05:45\", \"06:50\", \"07:15\", \"08:35\", \"10:35\", \"12:00\", \"13:30\", \"14:30\", \"15:30\", \"16:10\", \"17:20\", \"18:10\", \"19:20\", \"20:45\", \"21:40\", \"22:30\"], \"Subota\": [\"04:45\", \"05:50\", \"07:00\", \"08:10\", \"09:40\", \"11:40\", \"13:40\", \"15:40\", \"16:40\", \"18:05\", \"20:20\", \"22:40\"], \"Nedelja\": [\"04:45\", \"05:50\", \"07:10\", \"08:10\", \"09:40\", \"11:40\", \"13:40\", \"15:40\", \"16:40\", \"18:05\", \"20:30\", \"22:40\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "35A", TimeTable = "{\"Radni_dan\": [\"05:00\", \"06:15\", \"07:10\", \"09:05\", \"10:30\", \"11:45\", \"13:00\", \"13:30\", \"14:30\", \"15:20\", \"16:20\", \"17:30\", \"18:40\", \"19:45\", \"20:40\", \"21:35\", \"22:45\"], \"Subota\": [\"06:00\", \"07:40\", \"10:30\", \"11:45\", \"13:20\", \"15:20\", \"16:30\", \"18:40\", \"20:40\", \"22:45\"], \"Nedelja\": [\"06:00\", \"07:40\", \"10:30\", \"11:45\", \"13:20\", \"15:20\", \"18:40\", \"20:40\", \"22:45\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "35B", TimeTable = "{\"Radni_dan\": [\"04:50\", \"05:40\", \"06:10\", \"06:35\", \"07:00\", \"08:00\", \"09:40\", \"11:15\", \"12:30\", \"13:45\", \"14:05\", \"15:10\", \"15:55\", \"16:55\", \"18:05\", \"19:20\", \"20:20\", \"21:15\", \"22:15\", \"23:15\"], \"Subota\": [\"05:15\", \"06:35\", \"08:15\", \"11:05\", \"12:20\", \"13:55\", \"15:55\", \"17:10\", \"19:15\", \"21:15\", \"23:20\"], \"Nedelja\": [\"05:15\", \"06:35\", \"08:15\", \"11:05\", \"12:20\", \"13:55\", \"15:55\", \"19:15\", \"21:15\", \"23:20\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "41A", TimeTable = "{\"Radni_dan\": [\"06:05\", \"06:40\", \"07:30\", \"08:20\", \"08:55\", \"10:55\", \"11:55\", \"12:20\", \"13:15\", \"13:30\", \"14:20\", \"14:40\", \"15:20\", \"16:15\", \"17:00\", \"17:25\", \"18:10\", \"19:10\"], \"Subota\": [\"06:00\", \"14:15\", \"15:10\"], \"Nedelja\": [\"03:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "41B", TimeTable = "{\"Radni_dan\": [\"05:25\", \"06:00\", \"06:30\", \"06:45\", \"07:05\", \"07:20\", \"07:55\", \"08:45\", \"09:25\", \"11:25\", \"12:25\", \"12:50\", \"13:05\", \"13:40\", \"14:00\", \"14:45\", \"15:05\", \"15:50\", \"16:45\", \"17:30\", \"17:55\", \"18:40\", \"19:35\", \"03:00\"], \"Subota\": [\"05:55\", \"06:30\", \"14:45\", \"15:40\", \"03:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "42A", TimeTable = "{\"Radni_dan\": [\"04:50\", \"05:20\", \"06:10\", \"06:30\", \"07:00\", \"09:10\", \"11:30\", \"12:15\", \"13:35\", \"14:10\", \"14:25\", \"15:25\", \"15:50\", \"17:45\", \"19:15\", \"20:15\", \"22:20\"], \"Subota\": [\"04:50\", \"06:10\", \"07:00\", \"08:40\", \"09:30\", \"10:20\", \"11:55\", \"12:50\", \"14:00\", \"15:50\", \"17:00\", \"18:20\", \"19:50\", \"20:30\"], \"Nedelja\": [\"04:50\", \"06:10\", \"07:00\", \"08:40\", \"09:30\", \"10:20\", \"12:00\", \"12:50\", \"13:45\", \"15:50\", \"16:40\", \"18:20\", \"19:50\", \"20:40\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "42B", TimeTable = "{\"Radni_dan\": [\"04:45\", \"05:00\", \"05:30\", \"06:00\", \"06:10\", \"06:50\", \"07:10\", \"07:40\", \"09:50\", \"12:10\", \"12:55\", \"14:15\", \"15:05\", \"16:05\", \"16:30\", \"18:25\", \"19:55\", \"20:55\", \"02:45\"], \"Subota\": [\"04:55\", \"05:30\", \"06:50\", \"07:40\", \"09:20\", \"10:10\", \"11:00\", \"12:40\", \"13:30\", \"14:45\", \"16:30\", \"17:40\", \"19:00\", \"20:30\", \"21:10\", \"02:45\"], \"Nedelja\": [\"04:55\", \"05:30\", \"06:50\", \"07:40\", \"09:20\", \"10:10\", \"11:00\", \"12:40\", \"13:30\", \"14:25\", \"16:30\", \"17:20\", \"19:00\", \"20:30\", \"02:45\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "43A", TimeTable = "{\"Radni_dan\": [\"05:55\", \"06:50\", \"07:50\", \"09:35\", \"10:10\", \"11:15\", \"12:00\", \"12:50\", \"13:40\", \"14:45\", \"15:30\", \"16:30\", \"17:10\", \"18:20\", \"19:35\", \"20:35\", \"21:25\", \"22:45\", \"01:15\"], \"Subota\": [\"05:30\", \"07:40\", \"11:10\", \"14:20\", \"15:15\", \"17:30\", \"19:00\", \"21:20\", \"22:45\", \"01:15\"], \"Nedelja\": [\"05:30\", \"07:40\", \"11:10\", \"14:20\", \"15:00\", \"17:20\", \"19:00\", \"21:20\", \"22:45\", \"01:15\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "43B", TimeTable = "{\"Radni_dan\": [\"04:55\", \"05:40\", \"06:10\", \"06:45\", \"07:35\", \"08:45\", \"10:25\", \"11:05\", \"12:05\", \"12:50\", \"13:45\", \"14:30\", \"15:40\", \"16:20\", \"17:25\", \"18:05\", \"19:15\", \"20:25\", \"21:25\", \"22:20\", \"23:40\", \"02:30\"], \"Subota\": [\"04:40\", \"06:20\", \"08:40\", \"12:00\", \"15:10\", \"16:05\", \"18:20\", \"19:50\", \"22:10\", \"23:35\", \"02:30\"], \"Nedelja\": [\"04:40\", \"06:20\", \"08:40\", \"12:00\", \"15:10\", \"15:50\", \"18:10\", \"19:50\", \"22:10\", \"23:35\", \"02:30\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "51AA", TimeTable = "{\"Radni_dan\": [\"05:55\", \"06:50\", \"09:05\", \"12:00\", \"14:10\", \"15:50\", \"17:24\", \"18:30\", \"20:00\"], \"Subota\": [\"08:00\"], \"Nedelja\": [\"08:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "51AB", TimeTable = "{\"Radni_dan\": [\"05:30\", \"06:30\", \"07:20\", \"09:35\", \"12:35\", \"14:45\", \"16:25\", \"17:55\", \"19:00\", \"20:30\"], \"Subota\": [\"08:30\"], \"Nedelja\": [\"08:30\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "51BA", TimeTable = "{\"Radni_dan\": [\"05:30\", \"06:55\", \"09:10\", \"12:15\", \"14:15\", \"16:15\", \"17:30\", \"18:35\", \"20:05\"], \"Subota\": [\"12:05\"], \"Nedelja\": [\"12:05\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "51BB", TimeTable = "{\"Radni_dan\": [\"05:55\", \"07:25\", \"09:40\", \"12:40\", \"14:50\", \"16:50\", \"18:00\", \"19:05\", \"20:35\"], \"Subota\": [\"12:35\"], \"Nedelja\": [\"12:35\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "52A", TimeTable = "{\"Radni_dan\": [\"06:25\", \"07:00\", \"07:30\", \"07:55\", \"08:40\", \"09:55\", \"10:10\", \"10:55\", \"11:10\", \"12:10\", \"13:05\", \"13:30\", \"13:50\", \"14:25\", \"14:45\", \"15:05\", \"15:33\", \"16:35\", \"17:40\", \"18:55\", \"19:20\", \"20:35\", \"21:00\", \"21:40\", \"22:10\", \"22:40\", \"23:40\"], \"Subota\": [\"05:30\", \"06:35\", \"09:00\", \"11:00\", \"13:05\", \"14:10\", \"15:15\", \"16:20\", \"17:25\", \"18:30\", \"19:35\", \"23:40\"], \"Nedelja\": [\"05:30\", \"06:35\", \"09:00\", \"11:00\", \"13:05\", \"14:10\", \"15:15\", \"16:20\", \"17:25\", \"18:30\", \"19:30\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "52B", TimeTable = "{\"Radni_dan\": [\"04:55\", \"05:20\", \"06:20\", \"06:40\", \"06:55\", \"07:10\", \"07:30\", \"08:05\", \"08:25\", \"09:15\", \"10:30\", \"10:45\", \"11:30\", \"11:45\", \"12:50\", \"13:10\", \"13:40\", \"14:10\", \"14:30\", \"15:00\", \"15:25\", \"15:40\", \"16:10\", \"17:10\", \"18:15\", \"19:30\", \"19:55\", \"21:10\", \"21:30\", \"22:15\", \"22:45\", \"23:10\"], \"Subota\": [\"04:55\", \"06:00\", \"07:05\", \"09:30\", \"11:30\", \"13:35\", \"14:40\", \"15:45\", \"16:50\", \"17:55\", \"19:00\", \"20:05\"], \"Nedelja\": [\"04:55\", \"06:00\", \"07:05\", \"09:30\", \"11:30\", \"13:35\", \"14:40\", \"15:45\", \"16:50\", \"17:55\", \"19:00\", \"20:05\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "53A", TimeTable = "{\"Radni_dan\": [\"05:10\", \"06:20\", \"06:35\", \"06:55\", \"07:28\", \"08:00\", \"08:35\", \"10:30\", \"11:05\", \"11:20\", \"12:30\", \"12:48\", \"13:57\", \"14:15\", \"14:40\", \"15:10\", \"15:30\", \"16:05\", \"17:00\", \"18:10\", \"18:20\", \"18:57\", \"19:20\", \"20:15\", \"20:55\", \"21:50\"], \"Subota\": [\"05:20\", \"06:10\", \"07:35\", \"08:40\", \"09:50\", \"10:40\", \"12:35\", \"14:15\", \"14:40\", \"15:05\", \"15:30\", \"15:55\", \"17:00\", \"18:25\", \"20:55\", \"21:45\"], \"Nedelja\": [\"05:20\", \"06:10\", \"07:35\", \"08:40\", \"09:50\", \"10:40\", \"12:35\", \"14:15\", \"15:05\", \"15:55\", \"17:00\", \"18:25\", \"20:55\", \"21:45\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "53B", TimeTable = "{\"Radni_dan\": [\"04:00\", \"04:30\", \"05:10\", \"05:55\", \"06:00\", \"06:25\", \"07:15\", \"07:40\", \"08:15\", \"08:45\", \"09:20\", \"11:15\", \"11:50\", \"12:00\", \"12:20\", \"13:15\", \"13:35\", \"14:40\", \"14:55\", \"15:25\", \"15:55\", \"16:20\", \"17:40\", \"18:55\", \"19:05\", \"19:40\", \"20:05\", \"21:00\", \"21:40\", \"22:35\", \"03:10\"], \"Subota\": [\"04:00\", \"05:00\", \"05:55\", \"06:30\", \"07:40\", \"08:20\", \"10:40\", \"11:25\", \"13:20\", \"15:00\", \"15:35\", \"17:15\", \"19:10\", \"22:30\", \"03:10\"], \"Nedelja\": [\"04:00\", \"05:00\", \"05:55\", \"08:20\", \"10:35\", \"11:25\", \"13:20\", \"15:00\", \"15:35\", \"17:15\", \"19:10\", \"22:30\", \"03:10\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "54A", TimeTable = "{\"Radni_dan\": [\"05:00\", \"06:15\", \"06:53\", \"07:35\", \"09:15\", \"09:40\", \"10:15\", \"12:00\", \"12:25\", \"13:00\", \"13:15\", \"13:20\", \"14:10\", \"15:35\", \"16:25\", \"16:45\", \"17:35\"], \"Subota\": [\"06:30\", \"07:20\", \"08:25\", \"09:30\", \"11:45\", \"17:20\", \"19:00\"], \"Nedelja\": [\"06:30\", \"07:20\", \"08:25\", \"09:30\", \"11:45\", \"17:20\", \"19:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "54B", TimeTable = "{\"Radni_dan\": [\"05:10\", \"05:30\", \"05:45\", \"06:10\", \"06:50\", \"07:00\", \"07:35\", \"08:20\", \"10:00\", \"10:25\", \"11:00\", \"12:45\", \"13:10\", \"13:45\", \"14:00\", \"14:10\", \"15:00\", \"17:15\", \"17:30\", \"18:20\"], \"Subota\": [\"05:30\", \"06:00\", \"06:55\", \"07:20\", \"07:45\", \"09:10\", \"10:15\", \"12:30\", \"15:50\", \"18:05\", \"19:45\", \"21:40\"], \"Nedelja\": [\"06:55\", \"07:45\", \"09:10\", \"10:15\", \"12:30\", \"15:50\", \"18:05\", \"19:45\", \"21:40\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "55A", TimeTable = "{\"Radni_dan\": [\"04:00\", \"04:50\", \"05:30\", \"05:40\", \"06:10\", \"06:45\", \"07:10\", \"07:45\", \"08:25\", \"08:50\", \"09:25\", \"09:50\", \"10:40\", \"11:10\", \"11:30\", \"11:55\", \"12:10\", \"12:45\", \"13:00\", \"13:35\", \"13:51\", \"14:05\", \"14:15\", \"14:20\", \"14:35\", \"14:52\", \"15:40\", \"15:50\", \"16:10\", \"16:30\", \"16:55\", \"17:25\", \"17:45\", \"17:55\", \"18:25\", \"18:35\", \"19:00\", \"19:15\", \"19:30\", \"19:50\", \"20:05\", \"20:40\", \"21:10\", \"22:15\", \"00:00\"], \"Subota\": [\"04:50\", \"05:10\", \"05:55\", \"07:00\", \"08:10\", \"09:15\", \"10:20\", \"10:55\", \"11:30\", \"12:00\", \"12:50\", \"13:10\", \"13:40\", \"14:00\", \"14:50\", \"15:10\", \"16:10\", \"16:30\", \"17:35\", \"18:10\", \"18:40\", \"19:30\", \"19:50\", \"20:05\", \"20:40\", \"21:10\", \"22:00\"], \"Nedelja\": [\"04:50\", \"05:10\", \"05:55\", \"07:00\", \"08:10\", \"09:15\", \"10:20\", \"10:55\", \"11:30\", \"12:00\", \"12:50\", \"13:10\", \"13:40\", \"14:00\", \"14:50\", \"15:20\", \"16:10\", \"16:30\", \"17:35\", \"18:10\", \"18:40\", \"19:35\", \"19:50\", \"20:05\", \"20:40\", \"21:10\", \"22:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "55B", TimeTable = "{\"Radni_dan\": [\"04:45\", \"05:00\", \"05:25\", \"05:35\", \"06:05\", \"06:20\", \"06:35\", \"06:55\", \"07:05\", \"07:30\", \"07:55\", \"08:30\", \"09:10\", \"09:40\", \"10:15\", \"10:40\", \"11:30\", \"12:10\", \"12:25\", \"12:45\", \"12:55\", \"13:35\", \"13:50\", \"14:25\", \"14:40\", \"14:50\", \"15:05\", \"15:25\", \"15:40\", \"16:25\", \"16:35\", \"16:50\", \"17:00\", \"17:15\", \"17:40\", \"18:10\", \"18:30\", \"18:40\", \"19:10\", \"19:25\", \"19:45\", \"20:00\", \"20:15\", \"20:40\", \"20:55\", \"21:25\", \"21:55\", \"23:00\"], \"Subota\": [\"04:30\", \"04:45\", \"05:35\", \"06:05\", \"06:40\", \"07:15\", \"08:05\", \"08:55\", \"09:30\", \"10:00\", \"11:05\", \"11:40\", \"12:15\", \"12:45\", \"13:35\", \"13:55\", \"14:25\", \"14:50\", \"16:05\", \"16:40\", \"16:55\", \"17:45\", \"18:20\", \"18:55\", \"19:25\", \"20:15\", \"20:35\", \"20:50\", \"21:25\", \"21:55\", \"22:45\"], \"Nedelja\": [\"04:30\", \"04:45\", \"05:35\", \"06:05\", \"06:40\", \"07:15\", \"08:05\", \"08:55\", \"09:25\", \"10:00\", \"11:05\", \"11:40\", \"12:15\", \"12:45\", \"13:35\", \"13:55\", \"14:25\", \"14:45\", \"16:05\", \"16:40\", \"16:55\", \"17:45\", \"18:20\", \"18:55\", \"19:25\", \"20:15\", \"20:35\", \"20:50\", \"21:25\", \"21:55\", \"22:45\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "56A", TimeTable = "{\"Radni_dan\": [\"04:30\", \"05:20\", \"05:55\", \"06:30\", \"07:20\", \"08:10\", \"09:00\", \"10:05\", \"10:55\", \"11:50\", \"12:50\", \"13:25\", \"14:00\", \"14:30\", \"14:55\", \"15:15\", \"15:43\", \"16:20\", \"17:10\", \"18:00\", \"18:50\", \"19:40\", \"20:35\", \"21:25\", \"22:30\", \"22:50\", \"23:45\", \"01:15\"], \"Subota\": [\"04:30\", \"05:40\", \"06:45\", \"07:50\", \"09:00\", \"10:05\", \"11:10\", \"12:20\", \"13:25\", \"14:30\", \"15:40\", \"16:45\", \"17:50\", \"19:15\", \"20:20\", \"21:30\", \"22:25\", \"22:50\", \"23:45\", \"01:15\"], \"Nedelja\": [\"04:30\", \"05:40\", \"06:45\", \"07:50\", \"09:00\", \"10:05\", \"11:10\", \"12:20\", \"13:25\", \"14:30\", \"15:40\", \"16:45\", \"17:50\", \"19:15\", \"20:20\", \"21:30\", \"22:25\", \"22:50\", \"23:45\", \"01:15\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "56B", TimeTable = "{\"Radni_dan\": [\"03:50\", \"04:35\", \"05:15\", \"05:45\", \"06:05\", \"06:45\", \"07:20\", \"08:10\", \"09:00\", \"09:50\", \"10:55\", \"11:45\", \"12:40\", \"13:40\", \"14:15\", \"14:50\", \"15:20\", \"15:45\", \"16:05\", \"16:30\", \"17:10\", \"18:00\", \"18:50\", \"19:40\", \"20:30\", \"21:25\", \"22:15\", \"23:20\", \"23:35\", \"03:05\"], \"Subota\": [\"04:35\", \"05:15\", \"06:00\", \"06:30\", \"07:35\", \"08:40\", \"09:50\", \"10:55\", \"12:00\", \"13:10\", \"14:15\", \"15:20\", \"16:30\", \"17:35\", \"18:40\", \"20:05\", \"21:10\", \"22:20\", \"23:15\", \"23:35\", \"03:05\"], \"Nedelja\": [\"04:35\", \"05:15\", \"06:30\", \"07:35\", \"08:40\", \"09:50\", \"10:55\", \"12:00\", \"13:10\", \"14:15\", \"15:20\", \"16:30\", \"17:35\", \"18:40\", \"20:05\", \"21:10\", \"22:20\", \"23:15\", \"23:35\", \"03:05\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "60A", TimeTable = "{\"Radni_dan\": [\"08:30\", \"13:05\", \"16:50\", \"18:43\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "60B", TimeTable = "{\"Radni_dan\": [\"06:45\", \"09:15\", \"13:50\", \"17:35\", \"19:40\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "61A", TimeTable = "{\"Radni_dan\": [\"05:20\", \"06:10\", \"06:45\", \"06:55\", \"07:50\", \"08:10\", \"09:10\", \"09:30\", \"10:30\", \"11:30\", \"12:05\", \"12:45\", \"12:55\", \"13:30\", \"14:25\", \"15:10\", \"16:10\", \"17:15\", \"18:15\", \"18:55\", \"19:40\", \"20:35\", \"21:40\", \"01:50\"], \"Subota\": [\"05:25\", \"06:05\", \"07:20\", \"07:55\", \"09:15\", \"10:35\", \"11:55\", \"13:20\", \"14:30\", \"15:30\", \"16:30\", \"17:30\", \"19:10\", \"20:10\", \"21:30\", \"01:50\"], \"Nedelja\": [\"05:25\", \"06:00\", \"07:20\", \"07:55\", \"09:15\", \"10:35\", \"11:55\", \"13:20\", \"14:30\", \"15:30\", \"16:30\", \"17:30\", \"19:10\", \"20:10\", \"21:30\", \"01:50\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "61B", TimeTable = "{\"Radni_dan\": [\"04:05\", \"05:30\", \"05:55\", \"06:20\", \"06:30\", \"06:50\", \"07:25\", \"07:30\", \"08:30\", \"08:50\", \"09:50\", \"10:10\", \"11:10\", \"12:10\", \"12:50\", \"13:30\", \"13:35\", \"14:10\", \"15:10\", \"16:55\", \"17:55\", \"19:35\", \"22:15\", \"23:35\"], \"Subota\": [\"04:05\", \"06:05\", \"06:40\", \"08:00\", \"08:35\", \"09:55\", \"11:15\", \"12:35\", \"14:00\", \"15:10\", \"16:10\", \"17:10\", \"19:50\", \"22:10\", \"23:35\"], \"Nedelja\": [\"04:05\", \"06:05\", \"06:40\", \"08:00\", \"08:35\", \"09:55\", \"11:15\", \"12:35\", \"14:00\", \"15:10\", \"16:10\", \"17:10\", \"19:50\", \"22:10\", \"23:35\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "62A", TimeTable = "{\"Radni_dan\": [\"04:45\", \"05:30\", \"06:05\", \"06:50\", \"07:30\", \"08:50\", \"10:10\", \"10:50\", \"11:50\", \"12:15\", \"12:40\", \"13:20\", \"14:05\", \"14:15\", \"14:40\", \"15:15\", \"15:30\", \"15:45\", \"16:35\", \"17:30\", \"17:55\", \"18:25\", \"19:15\", \"20:15\", \"21:00\", \"22:45\"], \"Subota\": [\"04:45\", \"06:40\", \"08:40\", \"10:00\", \"11:15\", \"12:30\", \"14:10\", \"15:10\", \"16:00\", \"17:10\", \"17:50\", \"18:50\", \"20:30\", \"22:45\"], \"Nedelja\": [\"04:45\", \"06:40\", \"08:40\", \"10:00\", \"11:15\", \"12:30\", \"14:10\", \"15:10\", \"16:00\", \"17:10\", \"17:50\", \"18:50\", \"20:30\", \"22:45\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "62B", TimeTable = "{\"Radni_dan\": [\"04:45\", \"05:20\", \"06:10\", \"06:50\", \"07:30\", \"08:10\", \"09:30\", \"10:50\", \"11:30\", \"12:35\", \"12:55\", \"13:20\", \"14:05\", \"14:45\", \"15:00\", \"15:30\", \"16:00\", \"16:10\", \"16:30\", \"17:20\", \"18:10\", \"18:40\", \"19:05\", \"19:55\", \"20:55\", \"21:40\", \"23:25\"], \"Subota\": [\"04:45\", \"05:20\", \"07:15\", \"09:20\", \"10:40\", \"11:55\", \"13:10\", \"14:50\", \"15:50\", \"16:40\", \"17:50\", \"18:30\", \"19:30\", \"21:10\", \"23:25\"], \"Nedelja\": [\"04:45\", \"05:20\", \"07:15\", \"09:20\", \"10:40\", \"11:55\", \"13:10\", \"14:50\", \"15:50\", \"16:40\", \"17:50\", \"18:30\", \"19:30\", \"21:10\", \"23:25\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "64A", TimeTable = "{\"Radni_dan\": [\"05:15\", \"05:30\", \"06:05\", \"06:30\", \"06:55\", \"07:20\", \"07:50\", \"09:10\", \"10:30\", \"11:10\", \"11:55\", \"12:25\", \"12:55\", \"13:30\", \"14:05\", \"14:20\", \"14:40\", \"15:00\", \"15:15\", \"15:30\", \"16:10\", \"16:30\", \"17:25\", \"18:30\", \"19:15\", \"19:40\", \"20:20\", \"21:00\", \"21:30\", \"22:15\", \"22:45\", \"23:20\"], \"Subota\": [\"04:25\", \"04:50\", \"05:50\", \"06:30\", \"07:20\", \"08:25\", \"09:35\", \"10:45\", \"12:00\", \"12:35\", \"13:30\", \"14:40\", \"15:30\", \"16:30\", \"17:20\", \"18:20\", \"19:30\", \"20:30\", \"21:35\", \"22:45\"], \"Nedelja\": [\"04:20\", \"04:50\", \"05:50\", \"06:40\", \"07:20\", \"08:30\", \"09:35\", \"10:45\", \"11:55\", \"12:35\", \"13:30\", \"14:40\", \"15:30\", \"16:30\", \"17:20\", \"18:30\", \"19:30\", \"20:30\", \"21:35\", \"22:45\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "64B", TimeTable = "{\"Radni_dan\": [\"04:05\", \"04:50\", \"05:10\", \"05:30\", \"05:50\", \"06:10\", \"06:40\", \"07:05\", \"07:30\", \"07:55\", \"08:30\", \"09:50\", \"11:10\", \"11:50\", \"12:35\", \"13:05\", \"13:35\", \"14:10\", \"14:45\", \"15:20\", \"15:40\", \"15:55\", \"16:10\", \"16:50\", \"17:10\", \"18:05\", \"19:10\", \"19:55\", \"20:20\", \"21:00\", \"21:40\", \"22:10\", \"22:55\", \"23:20\", \"23:55\"], \"Subota\": [\"04:05\", \"05:00\", \"05:25\", \"06:25\", \"07:05\", \"07:55\", \"09:05\", \"10:15\", \"11:25\", \"12:35\", \"13:15\", \"14:10\", \"15:20\", \"16:10\", \"17:10\", \"18:00\", \"19:00\", \"20:10\", \"21:10\", \"22:10\", \"23:20\"], \"Nedelja\": [\"04:05\", \"04:55\", \"05:25\", \"06:25\", \"07:15\", \"07:55\", \"09:10\", \"10:15\", \"11:25\", \"12:35\", \"13:15\", \"14:10\", \"15:20\", \"16:10\", \"17:10\", \"18:00\", \"19:10\", \"20:10\", \"21:10\", \"22:10\", \"23:20\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "68A", TimeTable = "{\"Radni_dan\": [\"06:20\", \"07:10\", \"10:50\", \"12:00\", \"12:40\", \"13:30\", \"14:10\", \"15:10\", \"16:20\"], \"Subota\": [\"05:55\", \"07:15\", \"09:05\", \"11:45\", \"14:10\", \"16:35\", \"18:45\", \"21:05\"], \"Nedelja\": [\"05:55\", \"07:15\", \"09:20\", \"12:00\", \"12:35\", \"13:35\", \"16:00\", \"17:20\", \"18:40\", \"21:05\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "68B", TimeTable = "{\"Radni_dan\": [\"04:05\", \"05:30\", \"07:10\", \"07:45\", \"11:00\", \"11:30\", \"12:35\", \"13:15\", \"14:10\", \"15:00\", \"15:45\", \"16:55\"], \"Subota\": [\"05:35\", \"06:30\", \"07:50\", \"09:40\", \"12:20\", \"14:45\", \"17:10\", \"19:20\", \"21:40\"], \"Nedelja\": [\"05:35\", \"06:30\", \"07:50\", \"09:55\", \"12:35\", \"13:10\", \"14:10\", \"16:35\", \"17:55\", \"19:15\", \"21:40\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "69A", TimeTable = "{\"Radni_dan\": [\"06:05\", \"07:25\", \"08:20\", \"10:10\", \"11:20\", \"12:25\", \"13:30\", \"14:15\", \"15:35\", \"16:55\", \"18:00\", \"19:05\", \"20:00\", \"22:30\"], \"Subota\": [\"06:30\", \"08:35\", \"11:20\", \"14:05\", \"15:35\", \"17:55\", \"21:20\"], \"Nedelja\": [\"06:30\", \"08:35\", \"11:15\", \"14:05\", \"15:35\", \"17:55\", \"21:20\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "69B", TimeTable = "{\"Radni_dan\": [\"05:00\", \"05:50\", \"06:50\", \"07:25\", \"08:10\", \"09:00\", \"10:50\", \"12:00\", \"13:00\", \"13:40\", \"14:10\", \"15:00\", \"16:15\", \"17:30\", \"18:40\", \"19:40\", \"20:40\"], \"Subota\": [\"05:10\", \"07:10\", \"09:15\", \"12:00\", \"14:45\", \"16:15\", \"18:35\", \"22:00\"], \"Nedelja\": [\"06:10\", \"07:10\", \"09:15\", \"11:55\", \"14:45\", \"16:15\", \"18:35\", \"22:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "71A", TimeTable = "{\"Radni_dan\": [\"05:40\", \"06:10\", \"06:40\", \"06:45\", \"07:00\", \"07:25\", \"08:10\", \"09:00\", \"09:15\", \"09:55\", \"11:10\", \"11:40\", \"12:20\", \"12:50\", \"13:10\", \"13:50\", \"14:00\", \"14:40\", \"15:00\", \"15:45\", \"16:00\", \"16:50\", \"17:15\", \"18:05\", \"18:50\", \"20:35\", \"21:05\", \"21:50\", \"22:10\", \"23:25\", \"00:00\"], \"Subota\": [\"05:15\", \"06:10\", \"06:40\", \"07:30\", \"07:45\", \"08:20\", \"09:20\", \"09:55\", \"10:25\", \"10:55\", \"12:00\", \"12:15\", \"12:35\", \"13:05\", \"13:35\", \"14:55\", \"16:00\", \"17:20\", \"18:10\", \"19:15\", \"19:45\", \"20:50\", \"21:55\", \"22:30\", \"23:00\", \"23:30\"], \"Nedelja\": [\"05:15\", \"06:10\", \"06:40\", \"07:30\", \"07:45\", \"08:20\", \"09:05\", \"09:55\", \"10:55\", \"12:15\", \"13:05\", \"14:10\", \"14:55\", \"15:30\", \"16:35\", \"18:10\", \"19:15\", \"19:45\", \"20:50\", \"21:55\", \"22:30\", \"23:00\", \"23:30\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "71B", TimeTable = "{\"Radni_dan\": [\"04:45\", \"05:50\", \"06:05\", \"06:20\", \"06:50\", \"07:20\", \"07:40\", \"08:00\", \"09:00\", \"09:40\", \"09:55\", \"10:30\", \"11:50\", \"12:20\", \"13:00\", \"13:30\", \"13:50\", \"14:30\", \"14:40\", \"15:20\", \"15:40\", \"16:20\", \"16:35\", \"17:30\", \"17:55\", \"18:45\", \"19:30\", \"21:10\", \"21:45\", \"22:25\", \"22:50\", \"00:00\", \"00:30\"], \"Subota\": [\"04:05\", \"05:50\", \"06:50\", \"07:20\", \"08:10\", \"08:25\", \"08:55\", \"10:00\", \"10:35\", \"11:05\", \"11:35\", \"12:40\", \"12:55\", \"13:15\", \"13:45\", \"14:15\", \"15:35\", \"16:40\", \"18:00\", \"18:50\", \"19:55\", \"20:25\", \"21:30\", \"22:30\", \"23:05\", \"23:35\", \"00:05\"], \"Nedelja\": [\"04:05\", \"05:50\", \"06:50\", \"07:20\", \"08:10\", \"08:25\", \"08:55\", \"09:45\", \"10:35\", \"11:35\", \"12:55\", \"13:45\", \"14:50\", \"15:35\", \"16:10\", \"17:15\", \"18:50\", \"19:55\", \"20:25\", \"21:30\", \"22:30\", \"23:05\", \"23:35\", \"00:05\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "72A", TimeTable = "{\"Radni_dan\": [\"05:30\", \"06:00\", \"06:50\", \"07:40\", \"08:30\", \"09:30\", \"10:35\", \"11:30\", \"11:50\", \"12:30\", \"13:20\", \"14:20\", \"15:15\", \"16:10\", \"17:00\", \"17:40\", \"18:10\", \"19:00\", \"19:30\", \"20:10\", \"20:50\", \"21:35\", \"22:40\"], \"Subota\": [\"04:35\", \"05:35\", \"06:55\", \"08:50\", \"09:35\", \"11:15\", \"12:15\", \"13:20\", \"14:25\", \"15:45\", \"16:15\", \"17:35\", \"18:25\", \"19:30\", \"20:35\", \"21:35\", \"22:45\"], \"Nedelja\": [\"04:35\", \"05:35\", \"06:55\", \"08:50\", \"09:35\", \"11:15\", \"12:30\", \"13:20\", \"14:25\", \"15:45\", \"16:15\", \"17:35\", \"18:25\", \"19:30\", \"20:35\", \"21:35\", \"22:45\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "72B", TimeTable = "{\"Radni_dan\": [\"05:00\", \"06:10\", \"06:40\", \"07:20\", \"07:30\", \"08:20\", \"09:10\", \"10:10\", \"11:20\", \"12:10\", \"12:30\", \"13:10\", \"13:25\", \"14:00\", \"15:00\", \"15:55\", \"16:50\", \"17:40\", \"18:20\", \"18:50\", \"19:40\", \"20:10\", \"20:50\", \"21:30\", \"22:15\", \"23:20\"], \"Subota\": [\"05:15\", \"06:15\", \"07:35\", \"09:30\", \"10:15\", \"11:55\", \"12:55\", \"14:00\", \"15:05\", \"16:25\", \"16:55\", \"18:15\", \"19:05\", \"20:10\", \"21:15\", \"22:15\", \"23:25\"], \"Nedelja\": [\"05:15\", \"06:15\", \"07:35\", \"09:30\", \"10:15\", \"11:55\", \"13:10\", \"14:00\", \"15:05\", \"16:25\", \"16:55\", \"18:15\", \"19:05\", \"20:10\", \"21:15\", \"22:15\", \"23:25\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "73A", TimeTable = "{\"Radni_dan\": [\"07:20\", \"09:40\", \"11:00\", \"13:40\"], \"Subota\": [\"10:20\", \"14:40\"], \"Nedelja\": [\"10:20\", \"14:40\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "73B", TimeTable = "{\"Radni_dan\": [\"08:00\", \"10:20\", \"11:40\", \"14:20\"], \"Subota\": [\"11:00\", \"15:20\"], \"Nedelja\": [\"11:00\", \"15:20\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "74A", TimeTable = "{\"Radni_dan\": [\"05:10\", \"05:50\", \"06:30\", \"07:55\", \"08:50\", \"10:20\", \"11:20\", \"12:10\", \"13:00\", \"14:30\", \"15:30\", \"16:35\", \"17:30\", \"18:35\", \"19:15\", \"20:00\", \"21:20\", \"22:30\"], \"Subota\": [\"05:20\", \"06:25\", \"08:00\", \"09:40\", \"10:40\", \"12:50\", \"13:55\", \"15:15\", \"17:05\", \"19:00\", \"20:00\", \"22:10\"], \"Nedelja\": [\"05:20\", \"06:25\", \"08:00\", \"10:40\", \"12:50\", \"13:55\", \"15:15\", \"17:05\", \"19:00\", \"20:00\", \"22:10\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "74B", TimeTable = "{\"Radni_dan\": [\"05:00\", \"05:50\", \"06:25\", \"07:10\", \"08:35\", \"09:30\", \"11:00\", \"12:00\", \"12:50\", \"13:40\", \"15:10\", \"16:10\", \"17:15\", \"18:10\", \"19:15\", \"19:55\", \"20:40\", \"22:00\", \"23:10\"], \"Subota\": [\"05:00\", \"06:00\", \"07:05\", \"08:40\", \"10:20\", \"11:20\", \"13:30\", \"14:35\", \"15:55\", \"17:45\", \"19:40\", \"20:40\", \"22:50\"], \"Nedelja\": [\"05:00\", \"06:00\", \"07:05\", \"08:40\", \"11:20\", \"13:30\", \"14:35\", \"15:55\", \"17:45\", \"19:40\", \"20:40\", \"22:50\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "76A", TimeTable = "{\"Radni_dan\": [\"04:55\", \"06:10\", \"07:05\", \"07:30\", \"08:25\", \"09:25\", \"10:25\", \"11:15\", \"11:55\", \"12:45\", \"13:25\", \"13:55\", \"14:25\", \"14:50\", \"15:25\", \"16:25\", \"17:35\", \"18:40\", \"19:35\", \"20:25\", \"21:30\", \"22:45\"], \"Subota\": [\"05:40\", \"06:30\", \"07:30\", \"08:45\", \"10:15\", \"11:25\", \"13:10\", \"14:45\", \"15:30\", \"15:30\", \"16:40\", \"18:00\", \"19:00\", \"20:20\", \"21:25\", \"22:45\"], \"Nedelja\": [\"05:40\", \"06:50\", \"07:30\", \"08:45\", \"10:15\", \"11:30\", \"13:10\", \"14:45\", \"16:45\", \"18:00\", \"19:00\", \"20:20\", \"21:25\", \"22:45\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "76B", TimeTable = "{\"Radni_dan\": [\"05:00\", \"05:30\", \"06:00\", \"06:20\", \"06:45\", \"07:20\", \"07:45\", \"08:15\", \"09:05\", \"10:05\", \"11:15\", \"12:00\", \"12:40\", \"13:30\", \"14:10\", \"14:40\", \"15:10\", \"15:35\", \"16:10\", \"17:10\", \"18:20\", \"19:25\", \"20:20\", \"21:10\", \"22:10\", \"23:20\"], \"Subota\": [\"05:00\", \"06:15\", \"07:20\", \"08:15\", \"09:30\", \"11:00\", \"12:10\", \"13:55\", \"15:30\", \"16:15\", \"16:15\", \"17:25\", \"18:40\", \"19:45\", \"21:05\", \"22:00\", \"23:30\"], \"Nedelja\": [\"05:00\", \"06:15\", \"07:30\", \"08:15\", \"09:30\", \"11:00\", \"12:15\", \"13:55\", \"15:30\", \"17:30\", \"18:45\", \"19:45\", \"21:05\", \"22:00\", \"23:30\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "77A", TimeTable = "{\"Radni_dan\": [\"06:00\", \"11:45\", \"14:45\", \"16:15\"], \"Subota\": [\"07:00\", \"11:45\", \"19:20\"], \"Nedelja\": [\"07:00\", \"11:45\", \"19:20\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "77B", TimeTable = "{\"Radni_dan\": [\"05:45\", \"06:45\", \"12:30\", \"15:25\", \"17:00\"], \"Subota\": [\"05:00\", \"07:40\", \"12:25\", \"15:15\", \"20:00\"], \"Nedelja\": [\"05:00\", \"07:40\", \"12:35\", \"15:15\", \"20:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "78A", TimeTable = "{\"Radni_dan\": [\"10:35\", \"12:10\", \"13:20\", \"14:20\", \"15:10\", \"16:00\", \"17:00\", \"17:25\", \"18:30\", \"19:40\"], \"Subota\": [\"08:20\", \"10:10\", \"14:10\", \"16:15\", \"17:00\", \"18:00\"], \"Nedelja\": [\"08:20\", \"10:10\", \"14:10\", \"17:00\", \"18:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "78B", TimeTable = "{\"Radni_dan\": [\"04:35\", \"06:10\", \"11:30\", \"13:05\", \"15:10\", \"16:00\", \"16:50\", \"17:55\", \"18:15\", \"19:30\", \"20:40\"], \"Subota\": [\"06:40\", \"09:10\", \"11:00\", \"15:00\", \"17:55\", \"18:50\", \"19:40\"], \"Nedelja\": [\"06:40\", \"09:10\", \"11:00\", \"15:00\", \"17:55\", \"18:50\", \"19:40\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "79A", TimeTable = "{\"Radni_dan\": [\"04:35\", \"05:10\", \"05:55\", \"07:00\", \"07:50\", \"08:30\", \"10:20\", \"11:15\", \"12:40\", \"13:25\", \"14:35\", \"15:35\", \"16:30\", \"17:30\", \"18:05\", \"19:10\", \"20:30\", \"22:45\"], \"Subota\": [\"04:30\", \"05:30\", \"06:30\", \"07:10\", \"09:30\", \"10:40\", \"11:15\", \"12:40\", \"13:30\", \"15:00\", \"16:20\", \"18:30\", \"20:25\", \"22:45\"], \"Nedelja\": [\"04:30\", \"05:30\", \"07:10\", \"09:30\", \"10:40\", \"11:15\", \"12:40\", \"13:30\", \"15:00\", \"16:20\", \"18:25\", \"20:25\", \"22:45\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "79B", TimeTable = "{\"Radni_dan\": [\"04:20\", \"04:55\", \"05:35\", \"06:20\", \"06:50\", \"07:20\", \"08:05\", \"08:50\", \"09:25\", \"11:20\", \"12:15\", \"13:40\", \"14:25\", \"15:35\", \"16:35\", \"17:30\", \"18:30\", \"19:05\", \"20:10\", \"21:50\", \"23:40\"], \"Subota\": [\"04:30\", \"05:30\", \"06:25\", \"07:35\", \"08:10\", \"10:30\", \"11:40\", \"12:20\", \"13:40\", \"14:30\", \"16:00\", \"17:20\", \"19:30\", \"21:50\", \"23:40\"], \"Nedelja\": [\"04:30\", \"05:30\", \"06:25\", \"08:15\", \"10:30\", \"11:40\", \"12:20\", \"13:40\", \"14:30\", \"16:00\", \"17:20\", \"19:30\", \"21:50\", \"23:40\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "80A", TimeTable = "{\"Radni_dan\": [\"19:00\", \"01:00\"], \"Subota\": [\"01:00\"], \"Nedelja\": [\"01:00\"]}", ValidFrom = validFrom });
            //context.Departures.Add(new Departures() { TransportLineID = "80B", TimeTable = "{\"Radni_dan\": [\"06:25\", \"07:00\", \"12:30\", \"19:45\", \"01:30\"], \"Subota\": [\"01:30\"], \"Nedelja\": [\"01:30\"]}", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "81A", TimeTable = "\"Radni_dan\": [\"06:15\", \"09:10\", \"11:35\", \"15:45\", \"18:40\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "81B", TimeTable = "\"Radni_dan\": [\"06:05\", \"07:20\", \"10:20\", \"12:35\", \"16:50\", \"19:50\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "84A", TimeTable = "\"Radni_dan\": [\"05:40\", \"09:55\", \"11:40\", \"12:50\", \"13:35\", \"14:40\", \"17:35\", \"19:50\", \"21:35\"], \"Subota\": [\"05:40\", \"11:40\", \"14:40\", \"17:35\", \"20:00\", \"21:40\"], \"Nedelja\": [\"11:40\", \"14:40\", \"17:35\", \"20:00\", \"21:40\"]", ValidFrom = validFrom });
            context.Departures.Add(new Departures() { TransportLineID = "84B", TimeTable = "\"Radni_dan\": [\"05:00\", \"05:45\", \"21:15\", \"22:50\"], \"Subota\": [\"05:00\", \"07:15\", \"12:55\", \"16:15\", \"19:00\", \"21:25\", \"22:50\"], \"Nedelja\": [\"05:00\", \"12:55\", \"16:15\", \"19:00\", \"21:25\", \"22:50\"]", ValidFrom = validFrom });

            context.SaveChanges();

            //Station station1 = new Station() { Name = "USPENSKA - ŠAFARIKOVA", Address = "USPENSKA - ŠAFARIKOVA", X = 19.8416504854, Y = 45.255203707 };


            //context.Stations.Add(station1);

            //context.StationsOnLines.Add(new StationsOnLine() { StationID = station1.StationID, TransportLineID = "2A" });
            //context.StationsOnLines.Add(new StationsOnLine() { StationID = station1.StationID, TransportLineID = "2B" });


            //context.StationsOnLines.Add(new StationsOnLine() { StationID = 1, TransportLineID = "6A" });

            //context.StationsOnLines.Add(new StationsOnLine() { StationID = 2, TransportLineID = "6B" });

            //context.StationsOnLines.Add(new StationsOnLine() { StationID = 3, TransportLineID = "6B" });

            //context.SaveChanges();

            AddStations(context);
            //context.SaveChanges();

            //

            //AddStations(context);
        }

        private void AddStations(ApplicationDbContext context)
        {
            string addr = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/busStopsPointsCorrected1.json");
            //string addr = "C:\\Users\\Mladen\\Desktop\\Web2\\WebApp\\WebApp\\App_Data\\busStopsPointsCorrected1.json";
            //string addr = @"C:\Users\Mladen\Desktop\Web2\WebApp\WebApp\App_Data\busStopsPointsCorrected1.json";
            //string json = "";

            string text = System.IO.File.ReadAllText(@"C:\Users\Mladen\Desktop\Web2\WebApp\WebApp\App_Data\busStopsPointsCorrected.json");

            var niz = text.Split('\n');

            //using (StreamReader reader = new StreamReader(text))
            //{
            //    json = reader.ReadToEnd();
            //}
            //TODO
            //popraviti ne radi iz nekog razloga ovde sa dynamic...
            //napraviti parsiranje novo-->

            //dynamic niz = JsonConvert.DeserializeObject(json);
            string[] delimers = new string[] { ",\"" };
            for(int i = 1; i < niz.Length; i++)
            {
                if (i == 20) break;
                string[] podniz = niz[i].Split(delimers,StringSplitOptions.None);
                for(int j = 0; j < podniz.Length; j++)
                {

                    string item = string.Empty;
                    string[] items = null;
                    string[] linesOnThisStation = null;
                    string oneLineOnThisStation = string.Empty;
                    double longitude = 0.0;
                    double latitude = 0.0;
                    string nameOfStation = string.Empty;
                    //if (j > 0)
                    //{
                    //    item = podniz[j];
                    //    items = item.Split('|');
                    //    ///////
                    //    ///
                    //    if (items[0].Split(',').Count() == 1)
                    //    {
                    //        oneLineOnThisStation = items[0];
                    //    }
                    //    else
                    //    {
                    //        linesOnThisStation = items[0].Split(',');
                    //    }

                    //    longitude = Double.Parse(items[1]);
                    //    latitude = Double.Parse(items[2]);
                    //    nameOfStation = items[3].Trim();
                    //}
                    //else
                    //{
                        item = podniz[j];
                        items = item.Split('|');
                        //if (items[0].Split(',').Count() == 1)
                        //{
                        //    oneLineOnThisStation = items[0];
                        //}
                        //else
                        //{
                            linesOnThisStation = items[0].Split(',');
                        //}
                    //if(Double.TryParse(items[1].Trim(), out double o))
                    //{
                    //    longitude = o;
                    //}
                    //else
                    //{
                    //    longitude = 0.0;
                    //}
                    //if (Double.TryParse(items[2].Trim(), out double s))
                    //{
                    //    latitude = s;
                    //}
                    //else
                    //{
                    //    latitude = 0.0;
                    //}
                    longitude = Double.Parse(items[1].Trim());
                    latitude = Double.Parse(items[2].Trim());
                    nameOfStation = items[3].Trim();

                    //}
                    //var station = new Station()
                    //{
                    //    Name = nameOfStation,
                    //    Address = nameOfStation,
                    //    Y = longitude,
                    //    X = latitude
                    //};
                    var station = new Station()
                    {
                        Name = nameOfStation,
                        Address = nameOfStation,
                        Y = longitude,
                        X = latitude
                    };

                    context.Stations.Add(station);

                    //context.Stations.Add(station);
                    //context.StationsOnLines.Add(new StationsOnLine());
                    //context.SaveChanges();

                    if (linesOnThisStation.Length==0)
                    {
                        string lineID = oneLineOnThisStation.Replace('[', ' ').Replace(']', ' ').Trim();

                        if (!context.TransportLines.Any(l => l.TransportLineID == lineID))
                        {
                            continue;
                        }

                        //var line = context.TransportLines.Where(x => x.TransportLineID == lineID).FirstOrDefault();
                        //List<Station> list = new List<Station>();
                        //var stationOnLine = new StationsOnLine()
                        //{
                        //    StationID = station.StationID,
                        //    TransportLineID = lineID
                        //};
                        //var line = context.Set<TransportLine>().Find(lineID);
                        //list.Add(station);
                        //line.Stations = list;


                        context.StationsOnLines.Add(new StationsOnLine()
                        {
                            StationID = station.StationID,
                            TransportLineID = lineID
                        });
                        //context.SaveChanges();
                    }
                    else
                    {
                        for (int k = 0; k < linesOnThisStation.Length; k++)
                        {
                            string lineID = linesOnThisStation[k].Replace('[', ' ').Replace(']', ' ').Trim();

                            if (!context.TransportLines.Any(l => l.TransportLineID == lineID))
                            {
                                continue;
                            }

                            //List<Station> list = new List<Station>();
                            ////var stationOnLine = new StationsOnLine()
                            ////{
                            ////    StationID = station.StationID,
                            ////    TransportLineID = lineID
                            ////};
                            //var line = context.Set<TransportLine>().Find(lineID);
                            //list.Add(station);
                            //line.Stations = list;
                            //line.Stations.Add(station);


                            context.StationsOnLines.Add(new StationsOnLine()
                            {
                                StationID = station.StationID,
                                TransportLineID = lineID
                            });
                            //context.SaveChanges();
                        }
                        //station.StationsOnLines = context.StationsOnLines
                        context.SaveChanges();
                    }
                    //context.SaveChanges();
                }

            }
            //context.SaveChanges();

            //    for (int i = 0; i < Enumerable.Count(niz); i++)
            //    {
            //        for (int j = 0; j < Enumerable.Count(niz[i]); j++)
            //        {
            //            string item = string.Empty;
            //            string[] items = null;
            //            string[] linesOnThisStation = null;
            //            double longitude = 0.0;
            //            double latitude = 0.0;
            //            string nameOfStation = string.Empty;

            //            if (j > 0)
            //            {
            //                item = niz[i][j];
            //                items = item.Split('|');
            //                ///////
            //                ///
            //                linesOnThisStation = items[0].Trim().Split(',');

            //                longitude = Double.Parse(items[1].Trim());
            //                latitude = Double.Parse(items[2].Trim());
            //                nameOfStation = items[3].Trim();
            //            }
            //            else
            //            {
            //                item = niz[i][j];
            //                items = item.Split('|');
            //                linesOnThisStation = items[1].Trim().Split(',');

            //                longitude = Double.Parse(items[2].Trim());
            //                latitude = Double.Parse(items[3].Trim());
            //                nameOfStation = items[4].Trim();

            //            }
            //            var station = new Station()
            //            {
            //                Name = nameOfStation,
            //                Address = nameOfStation,
            //                Y = longitude,
            //                X = latitude
            //            };
            //            context.Stations.Add(station);
            //            context.SaveChanges();

            //            for (int k = 0; k < linesOnThisStation.Length; k++)
            //            {

            //                string lineID = linesOnThisStation[k].Replace('[', ' ').Replace(']', ' ').Trim();

            //                if (!context.TransportLines.Any(l => l.TransportLineID == lineID))
            //                {
            //                    continue;
            //                }

            //                context.StationsOnLines.Add(new StationsOnLine()
            //                {
            //                    StationID = station.StationID,
            //                    TransportLineID = lineID
            //                });
            //                context.SaveChanges();
            //            }
            //        }
            //    }
            //    context.SaveChanges();
        }
    }
}
