using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ToDoItem.Core.Entities;
using ToDoItem.UnitTests.Helpers;
using ToDoItem.Web.Helpers;
using ToDoItem.Web.Helpers.DataTablesServerSide;
using Xunit;

namespace ToDoItem.UnitTests.HelpersTests
{
    public class PagedListTests
    {
        [Fact]
        public void Create_Should_Create_PagedList_With_Data_Only_For_The_First_Page()
        {
            //arrange
            var paginationData = new DataTablesOptions
            {
                Length = 3,
                Start = 0
            };

            var items = TestSeed.GetItemsForTesting();

            //act
            var result =  PagedList<Item>.Create(items.AsQueryable(), paginationData);

            //assert
            result.Count.Should().Be(paginationData.Length);
            result.PageSize.Should().Be(paginationData.Length);
            result.PageNumber.Should().Be(1);
            result.TotalCount.Should().Be(items.Count);
        }

        [Fact]
        public void Create_Should_Create_PagedList_With_Data_For_The_Next_Page()
        {
            //arrange
            var paginationData = new DataTablesOptions
            {
                Length = 5,
                Start = 5
            };

            var items = new List<Item>();
            for (var i = 0; i <= 5; i++)
            {
                items.AddRange(TestSeed.GetItemsForTesting());
            }

            //act
            var result = PagedList<Item>.Create(items.AsQueryable(), paginationData);

            //assert
            result.Count.Should().Be(paginationData.Length);
            result.PageSize.Should().Be(paginationData.Length);
            result.PageNumber.Should().Be(2);
            result.TotalCount.Should().Be(items.Count);
        }

        [Fact]
        public void Create_Should_Return_Data_For_The_Last_Page()
        {
            //arrange
            var items = new List<Item>();
            for (var i = 0; i <= 5; i++)
            {
                items.AddRange(TestSeed.GetItemsForTesting());
            }

            var sourceCount = items.Count;
            var paginationData = new DataTablesOptions
            {
                Length = 5,
                Start = 15
            };

            //act
            var result = PagedList<Item>.Create(items.AsQueryable(), paginationData);

            //assert
            result.Count.Should().Be(sourceCount - paginationData.Start);
            result.PageSize.Should().Be(paginationData.Length);
            result.PageNumber.Should().Be(4);
            result.TotalCount.Should().Be(sourceCount);
        }
    }
}
