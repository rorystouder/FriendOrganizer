using FriendOrganizer.Model;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend> GetAll()
        {
            // TODO: Load data from a real database
            yield return new Friend { FirstName = "Rory", LastName = "Stouder" };
            yield return new Friend { FirstName = "Alicia", LastName = "Fennig" };
            yield return new Friend { FirstName = "Dylan", LastName = "Stouder" };
            yield return new Friend { FirstName = "Gianna", LastName = "Stouder" };
        }
    }
}
