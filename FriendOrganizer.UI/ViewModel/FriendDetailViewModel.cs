using FriendOrganizer.Model;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using FriendOrganizer.UI.Wrapper;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Data.Lookups;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : DetailViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendRepository _friendRepository;
        private FriendWrapper _friend;
        private FriendPhoneNumberWrapper _selectedPhoneNumber;
        //private bool _hasChanges;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;

        public FriendDetailViewModel(IFriendRepository friendRepository,
          IEventAggregator eventAggregator,
          IMessageDialogService messageDialogService,
          IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
          : base(eventAggregator)
        {
            _friendRepository = friendRepository;
            _messageDialogService = messageDialogService;
            _programmingLanguageLookupDataService = programmingLanguageLookupDataService;

            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            ProgrammingLanguages = new System.Collections.ObjectModel.ObservableCollection<LookupItem>();
            PhoneNumbers = new System.Collections.ObjectModel.ObservableCollection<FriendPhoneNumberWrapper>();
        }

        public override async Task LoadAsync(int? friendId)
        {
            Friend friend = friendId.HasValue
            ? await _friendRepository.GetByIdAsync(friendId.Value)
            : CreateNewFriend();

            InitializeFriend(friend);

            InitializeFriendPhoneNumbers(friend.PhoneNumbers);

            await LoadProgrammingLanguagesLookupAsync();
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _friendRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                // Little trick to trigger the validation
                Friend.FirstName = "";
            }
        }

        private void InitializeFriendPhoneNumbers(ICollection<FriendPhoneNumber> phoneNumbers)
        {
            foreach (FriendPhoneNumberWrapper wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (FriendPhoneNumber friendPhoneNumber in phoneNumbers)
            {
                FriendPhoneNumberWrapper wrapper = new FriendPhoneNumberWrapper(friendPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            }
        }

        private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _friendRepository.HasChanges();
            }
            if (e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = " - " });
            IEnumerable<LookupItem> lookup = await _programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (LookupItem lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        public FriendWrapper Friend
        {
            get => _friend;
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public FriendPhoneNumberWrapper SelectedPhoneNumber
        {
            get => _selectedPhoneNumber;
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddPhoneNumberCommand { get; }

        public ICommand RemovePhoneNumberCommand { get; }

        public System.Collections.ObjectModel.ObservableCollection<LookupItem> ProgrammingLanguages { get; }

        public System.Collections.ObjectModel.ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }

        protected override async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();
            HasChanges = _friendRepository.HasChanges();
            RaiseDetailSavedEvent(Friend.Id, $"{Friend.FirstName} {Friend.LastName}");
        }

        protected override bool OnSaveCanExecute()
        {
            return Friend != null
              && !Friend.HasErrors
              && PhoneNumbers.All(pn => !pn.HasErrors)
              && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            MessageDialogResult result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the friend {Friend.FirstName} {Friend.LastName}?",
              "Question");
            if (result == MessageDialogResult.OK)
            {
                _friendRepository.Remove(Friend.Model);
                await _friendRepository.SaveAsync();
                RaiseDetailDeletedEvent(Friend.Id);
            }
        }

        private void OnAddPhoneNumberExecute()
        {
            FriendPhoneNumberWrapper newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = ""; // Trigger validation :-)
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            _friendRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            _ = PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _friendRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }

        private Friend CreateNewFriend()
        {
            Friend friend = new Friend();
            _friendRepository.Add(friend);
            return friend;
        }
    }
}
