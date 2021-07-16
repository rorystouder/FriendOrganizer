using Prism.Commands;
using System;

namespace FriendOrganizer.UI.ViewModel
{
    public class ObserableCollection<T>
    {
        public static implicit operator ObserableCollection<T>(DelegateCommand v)
        {
            throw new NotImplementedException();
        }
    }
}