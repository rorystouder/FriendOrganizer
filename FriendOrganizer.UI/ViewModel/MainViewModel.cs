﻿using Autofac.Features.Indexed;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private IDetailViewModel _selectedDetailViewModel;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IIndex<string, IDetailViewModel> _detailViewModelCreator;

        public MainViewModel(INavigationViewModel navigationViewModel,
          IIndex<string, IDetailViewModel> detailViewModelCreator,
          IEventAggregator eventAggregator,
          IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _detailViewModelCreator = detailViewModelCreator;
            _messageDialogService = messageDialogService;

            DetailViewModels = new ObservableCollection<IDetailViewModel>();

            _ = _eventAggregator.GetEvent<OpenDetailViewEvent>()
             .Subscribe(OnOpenDetailView);
            _ = _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
              .Subscribe(AfterDetailDeleted);
            _ = _eventAggregator.GetEvent<AfterDetailClosedEvent>()
              .Subscribe(AfterDetailClosed);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);

            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public ICommand CreateNewDetailCommand { get; }

        public INavigationViewModel NavigationViewModel { get; }

        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }

        public IDetailViewModel SelectedDetailViewModel
        {
            get => _selectedDetailViewModel;
            set
            {
                _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            IDetailViewModel detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == args.Id
                && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = _detailViewModelCreator[args.ViewModelName];
                await detailViewModel.LoadAsync(args.Id);
                DetailViewModels.Add(detailViewModel);
            }

            SelectedDetailViewModel = detailViewModel;
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(
              new OpenDetailViewEventArgs { ViewModelName = viewModelType.Name });
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void AfterDetailClosed(AfterDetailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }
        private void RemoveDetailViewModel(int id, string viewModelName)
        {
            IDetailViewModel detailViewModel = DetailViewModels
                            .SingleOrDefault(vm => vm.Id == id
                            && vm.GetType().Name == viewModelName);
            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }
    }
}
