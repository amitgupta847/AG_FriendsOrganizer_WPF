using System;
using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public interface IMeetingLookupDataService
    {
        Task<List<LookupItem>> GetMeetingLookupAsync();
    }
}
