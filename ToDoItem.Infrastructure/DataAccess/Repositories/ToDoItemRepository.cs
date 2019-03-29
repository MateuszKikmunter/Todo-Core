using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoItem.Core.Abstract;
using ToDoItem.Core.Entities;

namespace ToDoItem.Infrastructure.DataAccess.Repositories
{
    public class ToDoItemRepository : IToDoItemRepository
    {
        private readonly ToDoItemDbContext _context;

        public ToDoItemRepository(ToDoItemDbContext context)
        {
            _context = context;
        }

        public IQueryable<Item> GetAll()
        {
            return _context.ToDoItems;
        }

        public async Task<IList<Item>> GetAllAsync()
        {
            return await _context.ToDoItems.ToListAsync();
        }

        public async Task<Item> GetSingleAsync(Guid id)
        {
            return await _context.ToDoItems.FindAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var itemToDelete = await GetSingleAsync(id);
            _context.ToDoItems.Remove(itemToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<Item> CreateAsync(Item item)
        {
            await _context.ToDoItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item> UpdateAsync(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IList<Item>> FindByAsync(Expression<Func<Item, bool>> predicate)
        {
            return await _context.ToDoItems.Where(predicate).ToListAsync();
        }

        public async Task<bool> ItemExistsAsync(Guid id)
        {
            return await _context.ToDoItems.AnyAsync(item => item.Id == id);
        }
    }
}
