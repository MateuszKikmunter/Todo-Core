using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ToDoItem.Web.Helpers.Extensions;
using Xunit;

namespace ToDoItem.UnitTests.ExtensionsTests
{
    public class ModelStateExtensionsTests
    {
        [Fact]
        public void GetValidationErrors_NoErrors_Should_Return_Empty_Collection()
        {
            //arrange
            var modelState = new ModelStateDictionary();

            //act
            var result = modelState.GetValidationErrors();

            //assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetValidationErrors_Model_State_Invalid_Should_Return_Collection_Of_Strings()
        {
            //arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("Error", "Error1");
            modelState.AddModelError("Ooops", "Error2");

            //act
            var result = modelState.GetValidationErrors();

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(2);
            result.First().Should().Be("Error1");
            result.Last().Should().Be("Error2");
        }
    }
}
