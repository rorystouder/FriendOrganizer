using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRespository : GenericRepository<Friend, FriendOrganizerDbContext>,
                                   IFriendRepository
    {
        public FriendRespository(FriendOrganizerDbContext context)
          : base(context)
        {
        }

        public override async Task<Friend> GetByIdAsync(int friendId)
        {
            return await Context.Friends
              .Include(f => f.PhoneNumbers)
              .SingleAsync(f => f.Id == friendId);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            _ = Context.FriendPhoneNumbers.Remove(model);
        }
    }
}
