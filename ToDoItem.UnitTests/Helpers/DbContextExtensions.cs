using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoItem.Infrastructure.DataAccess;
using Xunit.Sdk;

namespace ToDoItem.UnitTests.Helpers
{
    public static class DbContextExtensions
    {
        public static async Task<ToDoItemDbContext> SeedDabase<T>(this DbContext context, IList<T> entities) where T : class
        {
            if (context == null)
            {
                throw new NullException(nameof(context));
            }

            if (entities == null || !entities.Any())
            {
                throw new NullException(nameof(entities));
            }

            await context.Set<T>().AddRangeAsync(entities);
            await context.SaveChangesAsync();

            return context as ToDoItemDbContext;
        }
    }
}
