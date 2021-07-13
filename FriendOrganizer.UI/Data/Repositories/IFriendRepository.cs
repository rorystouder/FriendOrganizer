using FriendOrganizer.Model;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Respositories
{
    public interface IFriendRepository
    {
        Task<Friend> GetByIDAsync(int friendId);
        Task SaveAsync();
        bool HasChanges();
    }
}