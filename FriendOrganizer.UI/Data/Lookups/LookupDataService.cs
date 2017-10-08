using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class LookupDataService : IFriendLookupDataService, 
        IProgrammingLanguageLookupDataService,
        IMeetingLookupDataService
    {
        private Func<FriendOrganizerDbContext> _dbcontextCreator;

        public LookupDataService(Func<FriendOrganizerDbContext> dbContextCreator)
        {
            _dbcontextCreator = dbContextCreator;
        }

        public async Task<List<LookupItem>> GetFriendLookupAsync()
        {
            using (var ctx = _dbcontextCreator())
            {
                return await ctx.Friends.AsNoTracking()
                    .Select(friend =>
                    new LookupItem
                    {
                        Id = friend.Id,
                        DisplayMember = friend.FirstName + " " + friend.LastName
                    }).ToListAsync();
            }
        }

        public async Task<List<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            using (var ctx = _dbcontextCreator())
            {
                return await ctx.ProgrammingLanguages.AsNoTracking()
                    .Select(item =>
                    new LookupItem
                    {
                        Id = item.Id,
                        DisplayMember = item.Name
                    }).ToListAsync();
            }
        }

        public async Task<List<LookupItem>> GetMeetingLookupAsync()
        {
            using (var ctx = _dbcontextCreator())
            {
                var items = await ctx.Meetings.AsNoTracking()
                  .Select(m =>
                     new LookupItem
                     {
                         Id = m.Id,
                         DisplayMember = m.Title
                     })
                  .ToListAsync();
                return items;
            }
        }
    }

}
