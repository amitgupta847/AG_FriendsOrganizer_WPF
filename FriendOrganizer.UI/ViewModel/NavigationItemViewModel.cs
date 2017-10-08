using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
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
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        private IEventAggregator _eventAggregator;
        private string _detailViewModelName;

        public NavigationItemViewModel(int id, string displayName, 
            string detailViewModelName,
            IEventAggregator eventAggregator)
        {
            ID = id;
            _displayMember = displayName;
            _detailViewModelName = detailViewModelName;
            
            _eventAggregator = eventAggregator;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        public int ID { get; }
        public string DisplayMember
        {
            get
            {
                return _displayMember;
            }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenDetailViewCommand { get; }
        
        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(new OpenDetailViewEventArgs { Id = ID, ViewModelName = _detailViewModelName } );
        }

    }
}
