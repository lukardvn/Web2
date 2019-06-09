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
            context.TransportLines.Add(new TransportLine() { FromTo = "KLISA - CENTAR - LIMAN I", TransportLineID = "1A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "LIMAN I - CENTAR - KLISA", TransportLineID = "1B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - NOVO NASELJE", TransportLineID = "2A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "NOVO NASELJE - CENTAR", TransportLineID = "2B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "PETROVARADIN - CENTAR - DETELINARA", TransportLineID = "3A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "DETELINARA - CENTAR - PETROVARADIN", TransportLineID = "3B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ZEL.STANICA - POBEDA", TransportLineID = "3AA" });
            context.TransportLines.Add(new TransportLine() { FromTo = "POBEDA - ZEL.STANICA", TransportLineID = "3AB" });
            context.TransportLines.Add(new TransportLine() { FromTo = "LIMAN IV - CENTAR - Z.STANICA", TransportLineID = "4A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Z.STANICA - CENTAR - LIMAN IV", TransportLineID = "4B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "TEMERINSKI PUT - CENTAR - AVIJATIČAR.NASELJE", TransportLineID = "5A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "AVIJATIČAR.NASELJE - CENTAR - TEMERINSKI PUT", TransportLineID = "5B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "PODBARA - CENTAR - ADICE", TransportLineID = "6A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ADICE - CENTAR - PODBARA", TransportLineID = "6B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ADICE - VETERNIK (O.S.MARIJA TRANDAFIL)", TransportLineID = "6AA" });
            context.TransportLines.Add(new TransportLine() { FromTo = "VETERNIK (O.S.MARIJA TRANDAFIL) - ADICE", TransportLineID = "6AB" });
            context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - Z.STAN - F.PIJA - LIMAN4 - N.NASELJE", TransportLineID = "7A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - LIMAN4 - F.PIJA - Z.STAN - N.NASELJE", TransportLineID = "7B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "NOVO NASELJE - CENTAR - LIMAN I", TransportLineID = "8A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "LIMAN I - CENTAR - NOVO NASELJE", TransportLineID = "8B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "NOVO NASELJE - LIMAN - PETROVARADIN", TransportLineID = "9A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "PETROVARADIN - LIMAN - NOVO NASELJE", TransportLineID = "9B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - INDUST.ZONA JUG", TransportLineID = "10A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "INDUST.ZONA JUG - CENTAR", TransportLineID = "10B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Z.STANICA - BOLNICA - LIMAN - Z.STANICA", TransportLineID = "11A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Z.STANICA - LIMAN - BOLNICA - Z.STANICA", TransportLineID = "11B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - TELEP", TransportLineID = "12A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "TELEP - CENTAR", TransportLineID = "12B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "DETELINARA - GRBAVICA - UNIVERZITET", TransportLineID = "13A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "UNIVERZITET - GRBAVICA - DETELINARA", TransportLineID = "13B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - SAJLOVO", TransportLineID = "14A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "SAJLOVO - CENTAR", TransportLineID = "14B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - INDUSTR.ZONA SEVER", TransportLineID = "15A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "INDUSTR.ZONA SEVER - CENTAR", TransportLineID = "15B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ZELE.STANICA - PRISTANISNA ZONA", TransportLineID = "16A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "PRISTANISNA ZONA - ZELE.STANICA", TransportLineID = "16B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "BIG TC - CENTAR", TransportLineID = "17A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "CENTAR - BIG TC", TransportLineID = "17B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - DETELINARA - CENTAR - LIMAN - N.NASELJE", TransportLineID = "18A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "N.NASELJE - LIMAN - CENTAR - DETELINARA - N.NASELJE", TransportLineID = "18B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "ZEL.STANICA - LESNINA", TransportLineID = "20A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "LESNINA - ZEL.STANICA", TransportLineID = "20B" });

            //prigradski
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za ŠANGAJ", TransportLineID = "21A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz ŠANGAJ", TransportLineID = "21B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za KAĆ", TransportLineID = "22A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz KAĆ", TransportLineID = "22B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BUDISAVA", TransportLineID = "23A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BUDISAVA", TransportLineID = "23B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za KOVILJ", TransportLineID = "24A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz KOVILJ", TransportLineID = "24B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za PEJIĆEVI SALAŠI", TransportLineID = "30A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz PEJIĆEVI SALAŠI", TransportLineID = "30B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BAČKI JARAK", TransportLineID = "31A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BAČKI JARAK", TransportLineID = "31B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za TEMERIN", TransportLineID = "32A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz TEMERIN", TransportLineID = "32B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za GOSPOĐINCI", TransportLineID = "33A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz GOSPOĐINCI", TransportLineID = "33B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za ČENEJ", TransportLineID = "35A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz ČENEJ", TransportLineID = "35B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za RUMENKA", TransportLineID = "41A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz RUMENKA", TransportLineID = "41B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za KISAČ", TransportLineID = "42A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz KISAČ", TransportLineID = "42B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za STEPANOVIĆEVO", TransportLineID = "43A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz STEPANOVIĆEVO", TransportLineID = "43B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za VETERNIK A", TransportLineID = "51AA" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz VETERNIK A", TransportLineID = "51AB" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za VETERNIK B", TransportLineID = "51BA" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz VETERNIK B", TransportLineID = "51BB" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za VETERNIK", TransportLineID = "52A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz VETERNIK", TransportLineID = "52B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za FUTOG STARI", TransportLineID = "53A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz FUTOG STARI", TransportLineID = "53B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za FUTOG GRMEČKA", TransportLineID = "54A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz FUTOG GRMEČKA", TransportLineID = "54B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za FUTOG BRAĆE BOŠNJAK", TransportLineID = "55A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz FUTOG BRAĆE BOŠNJAK", TransportLineID = "55B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BEGEČ", TransportLineID = "56A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BEGEČ", TransportLineID = "56B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za S.KARLOVCI - BELILO II", TransportLineID = "60A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BELILO II - S.KARLOVCI", TransportLineID = "60B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za SR.KARLOVCI VINOGRADARSKA", TransportLineID = "61A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz SR.KARLOVCI VINOGRADARSKA", TransportLineID = "61B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za SR.KARLOVCI DUDARA", TransportLineID = "62A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz SR.KARLOVCI DUDARA", TransportLineID = "62B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BUKOVAC", TransportLineID = "64A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BUKOVAC", TransportLineID = "64B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za SR.KAMENICA - VOJINOVO", TransportLineID = "68A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz VOJINOVO - SR.KAMENICA", TransportLineID = "68B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za SR.KAMENICA - ČARDAK", TransportLineID = "69A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz ČARDAK - SR.KAMENICA", TransportLineID = "69B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za SR.KAMENICA - BOCKE", TransportLineID = "71A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BOCKE - SR.KAMENICA", TransportLineID = "71B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za PARAGOVO", TransportLineID = "72A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz PARAGOVO", TransportLineID = "72B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za MOŠINA VILA", TransportLineID = "73A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz MOŠINA VILA", TransportLineID = "73B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za POPOVICA", TransportLineID = "74A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz POPOVICA", TransportLineID = "74B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za S.LEDINCI", TransportLineID = "76A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz S.LEDINCI", TransportLineID = "76B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za STARI RAKOVAC", TransportLineID = "77A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz STARI RAKOVAC", TransportLineID = "77B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BEOČIN SELO", TransportLineID = "78A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BEOČIN SELO", TransportLineID = "78B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za ČEREVIĆ", TransportLineID = "79A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz ČEREVIĆ", TransportLineID = "79B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za B.A.S.", TransportLineID = "80A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz B.A.S.", TransportLineID = "80B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za BANOŠTOR", TransportLineID = "81A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz BANOŠTOR", TransportLineID = "81B" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci za LUG", TransportLineID = "84A" });
            context.TransportLines.Add(new TransportLine() { FromTo = "Polasci iz LUG", TransportLineID = "84B" });


            context.SaveChanges();
        }
    }
}
