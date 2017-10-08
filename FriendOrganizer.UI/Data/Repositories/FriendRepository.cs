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
    public class FriendRepository :GenericRepository<Friend,FriendOrganizerDbContext>, IFriendRepository
    {
        //private FriendOrganizerDbContext _context;

        // private Func<FriendOrganizerDbContext> _dbcontextCreator;

        public FriendRepository(FriendOrganizerDbContext dbContext):base(dbContext) //Func<FriendOrganizerDbContext> dbContextCreator
        {
           // _context = dbContext;
        }


        public override async Task<Friend> GetByIdAsync(int friendId)
        {
            return await Context.Friends.Include(f=>f.PhoneNumbers).SingleAsync(f => f.Id == friendId);
            //ctx.Friends.AsNoTracking().SingleAsync(f => f.Id == friendId);
        }

        public override async Task<List<Friend>> GetAllAsync()
        {
            // using (var ctx = _dbcontextCreator())
            // {
            return await Context.Friends.ToListAsync();

            //var friends = await ctx.Friends.AsNoTracking().ToListAsync();
            //await Task.Delay(5000);    //simulating a delay of 5 sec to demonstarte that UI is responsive while data is being fetched.
            //return friends;
            // }

            //    //TODO: later data from the real data base
            //    yield return new Friend { FirstName="Thomas",LastName="Huber"};
            //    yield return new Friend { FirstName = "Andreas", LastName = "Boehler" };
            //    yield return new Friend { FirstName = "Julia", LastName = "Huber" };
            //    yield return new Friend { FirstName = "Christ", LastName = "Egin" };
            //
        }

        //public async Task SaveAsync()//Friend friend)
        //{
            //using (var ctx = _dbcontextCreator())
            //{
            //_context.Friends.Attach(friend);
            //_context.Entry(friend).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            // }
        //}

        //we can use this method also to enable and disable the save button or 
        // to disable navigation if there are any changes.
        //public bool HasChanges()
        //{
        //    return _context.ChangeTracker.HasChanges();
        //}

        //public void Add(Friend friend)
        //{
        //    _context.Friends.Add(friend);

        //}

        //public void Remove(Friend model)
        //{
        //    _context.Friends.Remove(model);
        //}

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.FriendPhoneNumbers.Remove(model);
        }


        public async Task<bool> HasMeetingsAsync(int friendId)
        {
            return await Context.Meetings.AsNoTracking()
              .Include(m => m.Friends)
              .AnyAsync(m => m.Friends.Any(f => f.Id == friendId));
        }
    }
}
