using System;
using System.Collections.Generic;
using ToDoItem.Core.Entities;

namespace ToDoItem.UnitTests.Helpers
{
    public static class TestSeed
    {
        public static List<Item> GetItemsForTesting()
        {
            return new List<Item>
            {
                new Item
                {
                    AdditionalInformation = "Some additional info this is",
                    Completed = true,
                    Deadline = DateTime.Today.AddDays(1),
                    LastUpdated = DateTime.Today,
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Name = "Train with Yoda"
                },

                new Item
                {
                    AdditionalInformation = "I have to make Luke think of that I'm his father, he obviously forgot...",
                    Completed = false,
                    Deadline = DateTime.Today.AddDays(2),
                    LastUpdated = DateTime.Today.AddDays(1),
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Name = "Talk to Luke"
                },

                new Item
                {
                    AdditionalInformation = "Again, if I want something done right, I have to do it myself...",
                    Completed = false,
                    Deadline = DateTime.Today.AddDays(3),
                    LastUpdated = DateTime.Today.AddDays(2),
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Name = "Find stolen Death Star plans"
                }
            };
        }
    }
}
