using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoItem.Core.Entities;
using ToDoItem.Infrastructure.Identity;

namespace ToDoItem.Infrastructure.DataAccess.Helpers
{
    public static class Seed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "darth.vader@darkside.com",
                Email = "darth.vader@darkside.com",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(defaultUser, "P@ssword1");
        }

        public static async Task SeedToDoItemsAsync(IServiceProvider provider)
        {
            using (var toDoItemDbContext = new ToDoItemDbContext(provider.GetRequiredService<DbContextOptions<ToDoItemDbContext>>()))
            {
                using (var applicationDbContext = new ApplicationDbContext(provider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
                {
                    var defaultUser = await applicationDbContext.Users.FirstAsync(u => u.Email.Equals("darth.vader@darkside.com"));

                    if (!toDoItemDbContext.ToDoItems.Any())
                    {
                        var items = new List<Item>
                        {
                            new Item
                            {
                                AdditionalInformation = "I have to make Luke think of that I'm his father, he obviously forgot...",
                                Completed = false,
                                DeadLine = DateTime.Today.AddDays(2),
                                LastUpdated = DateTime.Today.AddDays(1),
                                UserId = new Guid(defaultUser.Id),
                                Name = "Talk to Luke"
                            },

                            new Item
                            {
                                AdditionalInformation = "Again, if I want something done right, I have to do it myself...",
                                Completed = false,
                                DeadLine = DateTime.Today.AddDays(3),
                                LastUpdated = DateTime.Today.AddDays(2),
                                UserId = new Guid(defaultUser.Id),
                                Name = "Find stolen Death Star plans"
                            }
                        };

                        await toDoItemDbContext.ToDoItems.AddRangeAsync(items);
                        await toDoItemDbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
