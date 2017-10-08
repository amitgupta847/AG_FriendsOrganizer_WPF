using Autofac;
using FriendOrganizer.DataAccess;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.ViewModel;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainWindow>().AsSelf();
            
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<FriendRepository>().As<IFriendRepository>();
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();
            builder.RegisterType<ProgrammingLanguageRepository>().As<IProgrammingLanguageRepository>();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();

            builder.RegisterType<FriendDetailViewModel>().Keyed<IDetailViewModel>(nameof(FriendDetailViewModel));
            builder.RegisterType<MeetingDetailViewModel>().Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));
            builder.RegisterType<ProgrammingLanguageDetailViewModel>().Keyed<IDetailViewModel>(nameof(ProgrammingLanguageDetailViewModel));
                                                                                                    
            //builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>();
            //builder.RegisterType<MeetingDetailViewModel>().As<IMeetingDetailViewModel>();





            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();
            // .SingleInstance   -- we dont want the single instance of DBContext.
            //we require it to be created everyt time we ask for.
            return builder.Build();
        }
    }
}

