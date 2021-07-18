using FriendOrganizer.Model;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using FriendOrganizer.UI.Data.Lookups;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IFriendLookupDataService _friendLookupService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMeetingLookupDataService _meetingLookupService;

        public NavigationViewModel(IFriendLookupDataService friendLookupService,
          IMeetingLookupDataService meetingLookupService,
          IEventAggregator eventAggregator)
        {
            _friendLookupService = friendLookupService;
            _meetingLookupService = meetingLookupService;
            _eventAggregator = eventAggregator;
            Friends = new System.Collections.ObjectModel.ObservableCollection<NavigationItemViewModel>();
            Meetings = new System.Collections.ObjectModel.ObservableCollection<NavigationItemViewModel>();
            _ = _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _ = _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            System.Collections.Generic.IEnumerable<LookupItem> lookup = await _friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (LookupItem item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                  nameof(FriendDetailViewModel),
                  _eventAggregator));
            }
            lookup = await _meetingLookupService.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (LookupItem item in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                  nameof(MeetingDetailViewModel),
                  _eventAggregator));
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<NavigationItemViewModel> Friends { get; }

        public System.Collections.ObjectModel.ObservableCollection<NavigationItemViewModel> Meetings { get; }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailDeleted(Friends, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, args);
                    break;
                default:
                    break;
            }
        }

        private void AfterDetailDeleted(System.Collections.ObjectModel.ObservableCollection<NavigationItemViewModel> items,
          AfterDetailDeletedEventArgs args)
        {
            NavigationItemViewModel item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item != null)
            {
                _ = items.Remove(item);
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailSaved(Friends, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, args);
                    break;
                default:
                    break;
            }
        }

        private void AfterDetailSaved(System.Collections.ObjectModel.ObservableCollection<NavigationItemViewModel> items,
          AfterDetailSavedEventArgs args)
        {
            NavigationItemViewModel lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember,
                  args.ViewModelName,
                  _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }
    }
}
