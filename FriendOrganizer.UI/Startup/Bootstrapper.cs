using Autofac;
using FriendOrganizer.DataAccess;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.ViewModel;
using Prism.Events;

namespace FriendOrganizer.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            ContainerBuilder builder = new ContainerBuilder();

            _ = builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            _ = builder.RegisterType<FriendOrganizerDbContext>().AsSelf();

            _ = builder.RegisterType<MainWindow>().AsSelf();

            _ = builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            _ = builder.RegisterType<MainViewModel>().AsSelf();
            _ = builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            _ = builder.RegisterType<FriendDetailViewModel>()
              .Keyed<IDetailViewModel>(nameof(FriendDetailViewModel));
            _ = builder.RegisterType<MeetingDetailViewModel>()
              .Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));

            _ = builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            _ = builder.RegisterType<FriendRespository>().As<IFriendRepository>();
            _ = builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();

            return builder.Build();
        }
    }
}
