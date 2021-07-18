using FriendOrganizer.Model;
using Prism.Commands;
using System;

namespace FriendOrganizer.UI.ViewModel
{
    public class ObservableCollection<T>
    {
        public static implicit operator ObservableCollection<T>(DelegateCommand v)
        {
            throw new NotImplementedException();
        }

        internal void Clear()
        {
            throw new NotImplementedException();
        }

        internal void Add(Friend addedFriend)
        {
            throw new NotImplementedException();
        }
    }
}