using FriendOrganizer.Model;
using FriendOrganizer.DataAccess;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : GenericRepository<Friend,FriendOrganizerDbContext>,
        IFriendRepository
    {
        public FriendRepository(FriendOrganizerDbContext context)
            :base(context)
        {
        }
        public override async Task<Friend> GetByIDAsync(int friendId)
        {
            return await Context.Friends
                .Include(f => f.PhoneNumbers)
                .SingleAsync(f => f.Id == friendId);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.FriendPhoneNumbers.Remove(model);
        }
    }
}
