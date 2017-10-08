using Autofac.Features.Indexed;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        

        private IDetailViewModel _selecteddetailViewModel;
        private IMessageDialogService _messageDialogService;
        
        private IIndex<string, IDetailViewModel> _detailViewModelCreator;

        public MainViewModel(INavigationViewModel navigationViewModel,
                             IIndex<string, IDetailViewModel> detailViewModelCreator,
                            //Func<IFriendDetailViewModel> friendDetailViewModelCreator,
                            // Func<IMeetingDetailViewModel> meetingDetailViewModelCreator,
                             IEventAggregator eventAggregator,
                             IMessageDialogService messageDialogService)
        {
            NavigationViewModel = navigationViewModel;

            _detailViewModelCreator = detailViewModelCreator;
            DetailViewModels = new ObservableCollection<ViewModel.IDetailViewModel>(); 
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(OnOpenDetailView);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            _eventAggregator.GetEvent<AfterDetailClosedEvent>().Subscribe(AfterDetailClosed);
            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            OpenSingleDetailViewCommand = new DelegateCommand<Type>(OnOpenSingleDetailViewExecute);
        }


        public INavigationViewModel NavigationViewModel { get; }


        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }



        public IDetailViewModel SelectedDetailViewModel
        {
            get { return _selecteddetailViewModel; }
            set
            {
                _selecteddetailViewModel = value;
                OnPropertyChanged();
            }
        }


        //public IDetailViewModel DetailViewModel
        //{
        //    get { return _detailViewModel; }
        //    private set
        //    {
        //        _detailViewModel = value;
        //        OnPropertyChanged();
        //    }
        //}

        public ICommand CreateNewDetailCommand { get; }

        public ICommand OpenSingleDetailViewCommand { get; }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private int nextNewItemId = 0;
        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new Event.OpenDetailViewEventArgs {  Id= nextNewItemId--, ViewModelName= viewModelType .Name} );
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args) // making friendId nullable to support adding new friend.
        {
            var detailViewModel = DetailViewModels.SingleOrDefault(vm => vm.Id == args.Id
            && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel= _detailViewModelCreator[args.ViewModelName];
                await detailViewModel.LoadAsync(args.Id);
                DetailViewModels.Add(detailViewModel);
            }

            //if (SelectedDetailViewModel != null && SelectedDetailViewModel.HasChanges)
            //{
            //    MessageDialogResult result = _messageDialogService.ShowOkCancelDialog("You have made changes. Do you want to navigate away?", "Confirmation.");
            //    if (result== MessageDialogResult.Cancel)
            //    {
            //        return;
            //    }
            //}

            //switch(args.ViewModelName)
            //{
            //    case nameof(FriendDetailViewModel):
            //        DetailViewModel = _friendDetailViewModelCreator();
            //        break;

            //    case nameof(MeetingDetailViewModel):
            //        DetailViewModel = _meetingDetailViewModelCreator();
            //        break;
            //    default:
            //        throw new Exception($"ViewModel {args.ViewModelName} not mapped");
            //        break;

            //}

            SelectedDetailViewModel = detailViewModel;// _detailViewModelCreator[args.ViewModelName];
            
            //await SelectedDetailViewModel.LoadAsync(args.Id);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);

            //SelectedDetailViewModel = null;
        }


        //public ObservableCollection<Friend> Friends { get; set; }

        //public Friend SelectedFriend
        //{
        //    get { return _selectedFriend; }
        //    set
        //    {
        //        _selectedFriend = value;
        //        OnPropertChanged();
        //    }
        //}


        private void AfterDetailClosed(AfterDetailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void RemoveDetailViewModel(int id, string vmName)
        {
            var detailViewModel = DetailViewModels.SingleOrDefault(vm => vm.Id == id
                          && vm.GetType().Name == vmName);

            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }

        private void OnOpenSingleDetailViewExecute(Type viewModelType)
        {
            OnOpenDetailView(
           new OpenDetailViewEventArgs
           {
               Id = -1,
               ViewModelName = viewModelType.Name
           });
        }
    }

}
