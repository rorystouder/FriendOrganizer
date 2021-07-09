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
                new Friend { FirstName = "Bruce", LastName = "Wayne" },
                new Friend { FirstName = "Peter", LastName = "Parker" },
                new Friend { FirstName = "Tony", LastName = "Stark" },
                new Friend { FirstName = "Thor", LastName = "Odenson" }
                );
        }
    }
}
