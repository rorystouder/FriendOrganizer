using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace FriendOrganizer.DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizerDbContext context)
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
              new ProgrammingLanguage { Name = "JavaScript" });

            context.SaveChanges();

            context.FriendPhoneNumbers.AddOrUpdate(pn => pn.Number,
              new FriendPhoneNumber { Number = "+1 5742693154", FriendId = context.Friends.First().Id });

            context.Meetings.AddOrUpdate(m => m.Title,
              new Meeting
              {
                  Title = "Datto Agent Install",
                  DateFrom = new DateTime(2021, 7, 21),
                  DateTo = new DateTime(2021, 7, 21),
                  Friends = new List<Friend>
                {
            context.Friends.Single(f => f.FirstName == "Rory" && f.LastName == "Stouder"),
            context.Friends.Single(f => f.FirstName == "Thor" && f.LastName == "Odenson")
                }
              });
        }
    }
}
