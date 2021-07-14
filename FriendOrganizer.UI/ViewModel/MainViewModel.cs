using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        private IMessageDialogService _messageDialogService;
        private IDetailViewModel _detailViewModel;

        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _messageDialogService = messageDialogService; // stores at the ready a message dialog service

            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
               .Subscribe(OnOpenFriendDetailView); // open the detail view on the mainview
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>()
                .Subscribe(AfterFriendDeleted); // Remove the detail view on the main
                                                // view after friend object has been deleted

            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);

            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public ICommand CreateNewFriendCommand { get; }

        public INavigationViewModel NavigationViewModel { get; }

        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            private set 
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        private async void OnOpenFriendDetailView(int? friendId)
        {
            // message dialog that alerts the user they havn't saved their changes
            if (DetailViewModel!=null && DetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Do you want to navigate away without Saving?", "Question");
                if(result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            DetailViewModel = _friendDetailViewModelCreator();
            await DetailViewModel.LoadAsync(friendId);
        }

        private void OnCreateNewFriendExecute()
        {
            OnOpenFriendDetailView(null);
        }

        private void AfterFriendDeleted(int friendId)
        {
            DetailViewModel = null;
        }
    }
}
