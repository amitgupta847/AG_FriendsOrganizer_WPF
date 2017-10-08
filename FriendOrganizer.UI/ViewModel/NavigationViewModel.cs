using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        //private NavigationItemViewModel _selectedFriend;
        IFriendLookupDataService _friendLookupDataService;
        private IEventAggregator _eventAggregator;
        private IMeetingLookupDataService _meetingLookUpService;

        public NavigationViewModel(IFriendLookupDataService friendLookUpDataService,
                                   IMeetingLookupDataService meetingLookUpService,
                                   IEventAggregator eventAggregator)
        {
            _friendLookupDataService = friendLookUpDataService;
            _meetingLookUpService = meetingLookUpService;

            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            Friends.Clear();

            var lookup = await _friendLookupDataService.GetFriendLookupAsync();

            foreach (var friend in lookup)
            {
                Friends.Add(new NavigationItemViewModel(friend.Id, friend.DisplayMember,
                                                        nameof(FriendDetailViewModel),
                                                        _eventAggregator));
            }




            Meetings.Clear();

            lookup = await _meetingLookUpService.GetMeetingLookupAsync();

            foreach (var meeting in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(meeting.Id, meeting.DisplayMember,
                                                        nameof(MeetingDetailViewModel),
                                                        _eventAggregator));
            }
        }
        public ObservableCollection<NavigationItemViewModel> Friends { get; }

        public ObservableCollection<NavigationItemViewModel> Meetings { get; }

        private void AfterDetailSaved(AfterDetailSavedEventArgs arg)
        {
            switch (arg.ViewModelName)
            {
                case nameof(FriendDetailViewModel):

                    AfterDetailSaved(Friends, arg);
                    break;

                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, arg);
                    break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs arg)
        {
            var lookUpItem = items.SingleOrDefault(l => l.ID == arg.Id);

            if (lookUpItem == null)
            {
                lookUpItem = new NavigationItemViewModel(arg.Id, arg.DisplayMember,
                                                         arg.ViewModelName,
                                                         _eventAggregator);
                items.Add(lookUpItem);
            }
            else
            {
                lookUpItem.DisplayMember = arg.DisplayMember;
            }
        }

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
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var itemToDelete = items.SingleOrDefault(item => item.ID == args.Id);
            if (itemToDelete != null)
            {
                items.Remove(itemToDelete);
            }
        }
        //public NavigationItemViewModel SelectedFriend
        //{
        //    get { return _selectedFriend; }
        //    set
        //    {
        //        _selectedFriend = value;
        //        OnPropertyChanged();

        //        if(_selectedFriend!=null)
        //        {
        //            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Publish(_selectedFriend.ID);
        //        }
        //    }
        //}

    }

}
