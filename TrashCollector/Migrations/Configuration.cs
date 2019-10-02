namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TrashCollector.Models;

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

            context.Days.AddOrUpdate(
                //d => d.Name,
                new Day { Id = 1, Name = "Monday" },
                new Day { Id = 2, Name = "Tuesday" },
                new Day { Id = 3, Name = "Wednesday" },
                new Day { Id = 4, Name = "Thursday" },
                new Day { Id = 5, Name = "Friday" },
                new Day { Id = 6, Name = "Saturday" }
                );
        }
    }
}
