namespace FriendORganizer.DataAccess.Migrations
{
    using FriendOrganizer.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendORganizer.DataAccess.FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendORganizer.DataAccess.FriendOrganizerDbContext context)
        {
            context.Friends.AddOrUpdate(
                f => f.FirstName,
                new Friend { FirstName = "Rory", LastName = "Stouder" },
                new Friend { FirstName = "Randy", LastName = "Stouder" },
                new Friend { FirstName = "Jane", LastName = "Stouder" },
                new Friend { FirstName = "Nick", LastName = "Stouder" },
                new Friend { FirstName = "Scott", LastName = "Stouder" }
                );
        }
    }
}
