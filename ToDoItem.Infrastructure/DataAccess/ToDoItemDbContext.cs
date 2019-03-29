using Microsoft.EntityFrameworkCore;
using ToDoItem.Core.Entities;
using ToDoItem.Infrastructure.DataAccess.Config;

namespace ToDoItem.Infrastructure.DataAccess
{
    public class ToDoItemDbContext : DbContext
    {
        public ToDoItemDbContext(DbContextOptions<ToDoItemDbContext> options) : base(options)
        {
        }

        public DbSet<Item> ToDoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ToDoItemConfig());
        }
    }
}
