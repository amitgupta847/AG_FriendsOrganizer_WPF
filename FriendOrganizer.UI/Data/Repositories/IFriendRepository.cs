﻿using System.Collections.Generic;
using FriendOrganizer.Model;
using System.Threading.Tasks;


namespace FriendOrganizer.UI.Data
{
    public interface IFriendRepository:IGenericRepository<Friend>
    {
        void RemovePhoneNumber(FriendPhoneNumber model);

        Task<bool> HasMeetingsAsync(int friendId);
    }
}