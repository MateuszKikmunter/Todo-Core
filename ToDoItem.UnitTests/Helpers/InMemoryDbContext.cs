using Microsoft.EntityFrameworkCore;
using ToDoItem.Infrastructure.DataAccess;

namespace ToDoItem.UnitTests.Helpers
{
    public static class InMemoryDbContext
    {
        /// <summary>
        /// Creates instance of InMemory ToDoItemDbContext with SQLite as Data Provider.
        /// </summary>
        public static ToDoItemDbContext GetContext() => CreateContext();

        private static ToDoItemDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ToDoItemDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new ToDoItemDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            return context;
        }
    }
}
