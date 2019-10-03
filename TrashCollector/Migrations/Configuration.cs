namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TrashCollector.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TrashCollector.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.



            //context.Days.AddOrUpdate(
            //    new Models.Day { Name = "Sunday" },
            //    new Models.Day { Name = "Monday" },
            //    new Models.Day { Name = "Tuesday" },
            //    new Models.Day { Name = "Wednesday" },
            //    new Models.Day { Name = "Thursday" },
            //    new Models.Day { Name = "Friday" },
            //    new Models.Day { Name = "Saturday" }
            //    );
        }
    }
}
