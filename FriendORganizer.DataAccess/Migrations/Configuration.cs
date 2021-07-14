using FriendOrganizer.Model;
using System.Data.Entity.Migrations;

namespace FriendORganizer.DataAccess.Migrations
{
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
            context.ProgrammingLanguages.AddOrUpdate(
                pl => pl.Name,
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "XAML" },
                new ProgrammingLanguage { Name = "HTML5" },
                new ProgrammingLanguage { Name = "Python" },
                new ProgrammingLanguage { Name = "Solidity" },
                new ProgrammingLanguage { Name = "Cobalt" },
                new ProgrammingLanguage { Name = "Java" },
                new ProgrammingLanguage { Name = "JavaScript" }
                );
        }
    }
}
