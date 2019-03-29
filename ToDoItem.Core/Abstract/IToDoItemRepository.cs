using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToDoItem.Core.Entities;

namespace ToDoItem.Core.Abstract
{
    public interface IToDoItemRepository
    {
        IQueryable<Item> GetAll();

        Task<IList<Item>> GetAllAsync();

        Task<Item> GetSingleAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<Item> CreateAsync(Item item);

        Task<Item> UpdateAsync(Item item);

        Task<IList<Item>> FindByAsync(Expression<Func<Item, bool>> predicate);

        Task<bool> ItemExistsAsync(Guid id);
    }
}
