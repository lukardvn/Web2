namespace WebApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApp.Models;
    using WebApp.Models.DomainEntities;

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

            if (!context.Users.Any(u => u.UserName == "admin@yahoo.com"))
            {
                var user = new ApplicationUser() { Id = "admin", UserName = "admin@yahoo", Email = "admin@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Admin123!")/*, UserTypeID = 1*/ };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "appu@yahoo.com"))
            { 
                var user = new ApplicationUser() { Id = "appu", UserName = "appu@yahoo", Email = "appu@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Appu123!")/*,UserTypeID = 2*/ };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");
            }

            //gradski za ns
            context.TransportLines.Add(new TransportLine() { FromTo = "KLISA - CENTAR - LIMAN I", LineNumber = "1A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "LIMAN I - CENTAR - KLISA", LineNumber = "1B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - NOVO NASELJE", LineNumber = "2A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "NOVO NASELJE - CENTAR", LineNumber = "2B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "PETROVARADIN - CENTAR - DETELINARA", LineNumber = "3A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "DETELINARA - CENTAR - PETROVARADIN", LineNumber = "3B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ZEL.STANICA - POBEDA", LineNumber = "3AA" });
            context.TransportLines.Add(new TransportLine() { FromTo = "POBEDA - ZEL.STANICA", LineNumber = "3AB" });
            context.TransportLines.Add(new TransportLine() { FromTo = "LIMAN IV - CENTAR - Z.STANICA", LineNumber = "4A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Z.STANICA - CENTAR - LIMAN IV", LineNumber = "4B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "TEMERINSKI PUT - CENTAR - AVIJATIČAR.NASELJE", LineNumber = "5A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "AVIJATIČAR.NASELJE - CENTAR - TEMERINSKI PUT", LineNumber = "5B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "PODBARA - CENTAR - ADICE", LineNumber = "6A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ADICE - CENTAR - PODBARA", LineNumber = "6B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ADICE - VETERNIK (O.S.MARIJA TRANDAFIL)", LineNumber = "6AA" });
            context.TransportLines.Add(new TransportLine() { FromTo = "VETERNIK (O.S.MARIJA TRANDAFIL) - ADICE", LineNumber = "6AB" });
            context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - Z.STAN - F.PIJA - LIMAN4 - N.NASELJE", LineNumber = "7A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - LIMAN4 - F.PIJA - Z.STAN - N.NASELJE", LineNumber = "7B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "NOVO NASELJE - CENTAR - LIMAN I", LineNumber = "8A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "LIMAN I - CENTAR - NOVO NASELJE", LineNumber = "8B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "NOVO NASELJE - LIMAN - PETROVARADIN", LineNumber = "9A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "PETROVARADIN - LIMAN - NOVO NASELJE", LineNumber = "9B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - INDUST.ZONA JUG", LineNumber = "10A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "INDUST.ZONA JUG - CENTAR", LineNumber = "10B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Z.STANICA - BOLNICA - LIMAN - Z.STANICA", LineNumber = "11A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Z.STANICA - LIMAN - BOLNICA - Z.STANICA", LineNumber = "11B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - TELEP", LineNumber = "12A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "TELEP - CENTAR", LineNumber = "12B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "DETELINARA - GRBAVICA - UNIVERZITET", LineNumber = "13A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "UNIVERZITET - GRBAVICA - DETELINARA", LineNumber = "13B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - SAJLOVO", LineNumber = "14A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "SAJLOVO - CENTAR", LineNumber = "14B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - INDUSTR.ZONA SEVER", LineNumber = "15A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "INDUSTR.ZONA SEVER - CENTAR", LineNumber = "15B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ZELE.STANICA - PRISTANISNA ZONA", LineNumber = "16A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "PRISTANISNA ZONA - ZELE.STANICA", LineNumber = "16B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "BIG TC - CENTAR", LineNumber = "17A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - BIG TC", LineNumber = "17B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - DETELINARA - CENTAR - LIMAN - N.NASELJE", LineNumber = "18A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - LIMAN - CENTAR - DETELINARA - N.NASELJE", LineNumber = "18B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ZEL.STANICA - LESNINA", LineNumber = "20A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "LESNINA - ZEL.STANICA", LineNumber = "20B" });

            context.SaveChanges();
        }
    }
}
