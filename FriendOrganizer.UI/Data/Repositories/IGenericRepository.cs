using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public interface IGenericRepository<T>
    {
        Task<T> GetByIdAsync(int id);

        Task<List<T>> GetAllAsync();

        Task SaveAsync();

        bool HasChanges();

        void Add(T model);
        void Remove(T model);
    }
}
