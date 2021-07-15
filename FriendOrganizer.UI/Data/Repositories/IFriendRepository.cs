using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Respositories
{
    public interface IFriendRepository:IGenericRepository<Friend>
    {
        void RemovePhoneNumber(FriendPhoneNumber model);
    }
}