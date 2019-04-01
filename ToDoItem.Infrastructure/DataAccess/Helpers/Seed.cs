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
                                Deadline = DateTime.Now.AddDays(8),
                                LastUpdated = DateTime.Now.AddDays(-6),
                                UserId = new Guid(defaultUser.Id),
                                Name = "Talk to Luke"
                            },

                            new Item
                            {
                                AdditionalInformation = "Again, if I want something done right, I have to do it myself...",
                                Completed = false,
                                Deadline = DateTime.Now.AddDays(10),
                                LastUpdated = DateTime.Now.AddDays(-9),
                                UserId = new Guid(defaultUser.Id),
                                Name = "Find stolen Death Star plans"
                            },

                            new Item
                            {
                                AdditionalInformation = "Hopefully they'll do it betther than last time, I'm tired of choking them.",
                                Completed = false,
                                Deadline = DateTime.Now.AddDays(2),
                                LastUpdated = DateTime.Now.AddDays(-1),
                                UserId = new Guid(defaultUser.Id),
                                Name = "Pick up cape from dry cleaners"
                            },

                            new Item
                            {
                                AdditionalInformation = "I don't know if it's a good idea, but hey, who does not risk does not win!",
                                Completed = false,
                                Deadline = DateTime.Now.AddDays(3),
                                LastUpdated = DateTime.Now.AddDays(-2),
                                UserId = new Guid(defaultUser.Id),
                                Name = "Call Padme..."
                            },

                            new Item
                            {
                                AdditionalInformation = string.Empty,
                                Completed = true,
                                Deadline = DateTime.Now.AddDays(3).AddHours(-1),
                                LastUpdated = DateTime.Now.AddDays(-3),
                                UserId = new Guid(defaultUser.Id),
                                Name = "Where's my arm..."
                            },
                        };

                        await toDoItemDbContext.ToDoItems.AddRangeAsync(items);
                        await toDoItemDbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
