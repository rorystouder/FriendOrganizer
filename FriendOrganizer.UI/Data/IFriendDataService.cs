using FriendOrganizer.Model;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public interface IFriendDataService
    {
        Task<Friend> GetByIDAsync(int friendId);
        Task SaveAsync(Friend friend);
    }
}