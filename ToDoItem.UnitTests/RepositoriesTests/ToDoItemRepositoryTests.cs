using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ToDoItem.Core.Entities;
using ToDoItem.Infrastructure.DataAccess.Repositories;
using ToDoItem.UnitTests.Helpers;
using Xunit;

namespace ToDoItem.UnitTests.RepositoriesTests
{
    public class ToDoItemRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_Should_Return_All_Entries()
        {
            using (var context = await InMemoryDbContext.GetContext().SeedDabase(TestSeed.GetItemsForTesting()))
            {
                //arrange
                var repository = new ToDoItemRepository(context);

                //act
                var result = await repository.GetAllAsync();

                //assert
                result.Should().NotBeNullOrEmpty();
                result.Count.Should().BeGreaterOrEqualTo(TestSeed.GetItemsForTesting().Count);
            }
        }

        [Fact]
        public async Task GetSingleAsync_Should_Return_Signle_Item()
        {
            using (var context = await InMemoryDbContext.GetContext().SeedDabase(TestSeed.GetItemsForTesting()))
            {
                //arrange
                var repository = new ToDoItemRepository(context);

                //act
                var itemToFind = await context.ToDoItems.FirstAsync();
                var result = await repository.GetSingleAsync(itemToFind.Id);

                //assert
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(itemToFind);
            }
        }

        [Fact]
        public async Task GetSingleAsync_No_Such_Item_Should_Return_Null()
        {
            using (var context = await InMemoryDbContext.GetContext().SeedDabase(TestSeed.GetItemsForTesting()))
            {
                //arrange
                var repository = new ToDoItemRepository(context);

                //act
                var result = await repository.GetSingleAsync(Guid.NewGuid());

                //assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Item()
        {
            using (var context = await InMemoryDbContext.GetContext().SeedDabase(TestSeed.GetItemsForTesting()))
            {
                //arrange
                var repository = new ToDoItemRepository(context);

                //act
                var itemToDelete = await context.ToDoItems.FirstAsync();
                var totalItems = await context.ToDoItems.CountAsync();
                await repository.DeleteAsync(itemToDelete.Id);

                //assert
                context.ToDoItems.Count().Should().Be(totalItems - 1);
            }
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Item()
        {
            using (var context = await InMemoryDbContext.GetContext().SeedDabase(TestSeed.GetItemsForTesting()))
            {
                //arrange
                var repository = new ToDoItemRepository(context);

                //act
                var itemToAdd = new Item
                {
                    Name = "Find Princess Leia",
                    AdditionalInformation = "Princess has been abducted, we have to find her!",
                    Completed = false,
                    Deadline = DateTime.Today.AddDays(1),
                    LastUpdated = DateTime.Today,
                    UserId = Guid.NewGuid()
                };

                var totalCount = await context.ToDoItems.CountAsync();
                var result = await repository.CreateAsync(itemToAdd);

                //assert
                result.Should().NotBeNull();
                result.Id.Should().NotBe(Guid.Empty);
                context.ToDoItems.Count().Should().Be(totalCount + 1);
            }
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Item()
        {
            using (var context = await InMemoryDbContext.GetContext().SeedDabase(TestSeed.GetItemsForTesting()))
            {
                //arrange
                var repository = new ToDoItemRepository(context);

                //act
                var item = await context.ToDoItems.FirstAsync();
                item.AdditionalInformation = "New Info";
                item.Name = "New Name";

                await repository.UpdateAsync(item);

                //assert
                var updatedItem = await context.ToDoItems.FirstAsync();
                updatedItem.AdditionalInformation.Should().Be(item.AdditionalInformation);
                updatedItem.Name.Should().Be(item.Name);
            }
        }

        [Fact]
        public async Task FindByAsync_ItemExists_Should_Return_Item()
        {
            using (var context = await InMemoryDbContext.GetContext().SeedDabase(TestSeed.GetItemsForTesting()))
            {
                //arrange
                var repository = new ToDoItemRepository(context);

                //act
                var result = await repository.FindByAsync(item => item.Completed);

                //assert
                result.Should().NotBeNull();
                result.Count.Should().Be(1);
            }
        }

        [Fact]
        public async Task FindByAsync_ItemDosNotExist_Should_Return_EmptyCollection()
        {
            using (var context = await InMemoryDbContext.GetContext().SeedDabase(TestSeed.GetItemsForTesting()))
            {
                //arrange
                var repository = new ToDoItemRepository(context);

                //act
                var result = await repository.FindByAsync(item => item.Name.Equals("Oh my God, Zombies!"));

                //assert
                result.Should().BeNullOrEmpty();
            }
        }

        [Fact]
        public async Task ItemExistsAsync_ItemDosNotExist_Should_Return_False()
        {
            using (var context = await InMemoryDbContext.GetContext().SeedDabase(TestSeed.GetItemsForTesting()))
            {
                //arrange
                var repository = new ToDoItemRepository(context);

                //act
                var result = await repository.ItemExistsAsync(Guid.Empty);

                //assert
                result.Should().BeFalse();
            }
        }

        [Fact]
        public async Task ItemExistsAsync_ItemExists_Should_Return_True()
        {
            using (var context = await InMemoryDbContext.GetContext().SeedDabase(TestSeed.GetItemsForTesting()))
            {
                //arrange
                var repository = new ToDoItemRepository(context);

                //act
                var item = await context.ToDoItems.FirstAsync();
                var result = await repository.ItemExistsAsync(item.Id);

                //assert
                result.Should().BeTrue();
            }
        }
    }
}
