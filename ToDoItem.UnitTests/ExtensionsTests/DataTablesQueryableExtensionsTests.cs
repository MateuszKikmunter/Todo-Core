using System.Linq;
using FluentAssertions;
using ToDoItem.UnitTests.Helpers;
using ToDoItem.Web.Helpers.DataTablesServerSide;
using Xunit;

namespace ToDoItem.UnitTests.ExtensionsTests
{
    public class DataTablesQueryableExtensionsTests
    {
        [Fact]
        public void HandleDataTablesRequest_No_Specific_Options_Should_Return_Items_Ordered_By_Id_Asc()
        {
            //arrange
            var queryString = "{" +
                                  "\"draw\":1," +
                                  "\"columns\":" +
                                  "[" +
                                    "{\"data\":\"id\",\"name\":\"\",\"searchable\":true,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}" +
                                    ",{\"data\":\"name\",\"name\":\"\",\"searchable\":true,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}," +
                                    "{\"data\":\"deadLine\",\"name\":\"\",\"searchable\":false,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}," +
                                    "{\"data\":\"lastUpdated\",\"name\":\"\",\"searchable\":false,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}," +
                                    "{\"data\":\"completed\",\"name\":\"\",\"searchable\":false,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}" +
                                  "]," +
                                  "\"order\":[{\"column\":0,\"dir\":\"asc\"}],\"start\":0,\"length\":10,\"search\":{\"value\":\"\",\"regex\":false}" +
                              "}";

            //act
            var result = TestSeed.GetItemsForTesting().AsQueryable().HandleDataTablesRequest(queryString);

            //assert
            result.Should().BeInAscendingOrder(i => i.Id);
        }

        [Fact]
        public void HandleDataTablesRequest_OderBy_Name_Desc_Should_Return_Items_Ordered_By_Name_Desc()
        {
            //arrange
            var queryString = "{" +
                                  "\"draw\":1," +
                                  "\"columns\":" +
                                  "[" +
                                    "{\"data\":\"id\",\"name\":\"\",\"searchable\":true,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}" +
                                    ",{\"data\":\"name\",\"name\":\"\",\"searchable\":true,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}," +
                                    "{\"data\":\"deadLine\",\"name\":\"\",\"searchable\":false,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}," +
                                    "{\"data\":\"lastUpdated\",\"name\":\"\",\"searchable\":false,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}," +
                                    "{\"data\":\"completed\",\"name\":\"\",\"searchable\":false,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}" +
                                  "]," +
                                  "\"order\":[{\"column\":1,\"dir\":\"desc\"}],\"start\":0,\"length\":10,\"search\":{\"value\":\"\",\"regex\":false}" +
                              "}";

            //act
            var result = TestSeed.GetItemsForTesting().AsQueryable().HandleDataTablesRequest(queryString);

            //assert
            result.Should().BeInDescendingOrder(i => i.Name);
        }

        [Fact]
        public void HandleDataTablesRequest_Search_Not_Empty_Should_Return_Items_With_Name_Containing_Search_Term()
        {
            //arrange
            var searchTerm = "Luke";
            var queryString = "{" +
                                  "\"draw\":2," +
                                  "\"columns\":" +
                                  "[" +
                                  "{\"data\":\"id\",\"name\":\"\",\"searchable\":false,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}," +
                                  "{\"data\":\"name\",\"name\":\"\",\"searchable\":true,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}," +
                                  "{\"data\":\"deadLine\",\"name\":\"\",\"searchable\":false,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}," +
                                  "{\"data\":\"lastUpdated\",\"name\":\"\",\"searchable\":false,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}," +
                                  "{\"data\":\"completed\",\"name\":\"\",\"searchable\":false,\"orderable\":true,\"search\":{\"value\":\"\",\"regex\":false}}" +
                                  "]," +
                                  "\"order\":[{\"column\":0,\"dir\":\"asc\"}],\"start\":0,\"length\":10,\"search\":{\"value\":\"Luke\",\"regex\":false}" +
                              "}";

            //act
            var result = TestSeed.GetItemsForTesting().AsQueryable().HandleDataTablesRequest(queryString);

            //assert
            result.Count.Should().BeGreaterOrEqualTo(1);
            result.First().Name.Should().Contain(searchTerm);
        }
    }
}
