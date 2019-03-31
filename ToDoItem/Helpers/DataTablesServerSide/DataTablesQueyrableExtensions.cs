using System.Linq;
using Newtonsoft.Json;
using ToDoItem.Core.Entities;
using System.Linq.Dynamic.Core;
using ToDoItem.Web.Helpers.DataTablesServerSide;

namespace ToDoItem.Web.Helpers
{
    public static class DataTablesQueyrableExtensions
    {
        private static string DEFAULT_SORT_COLUMN => "Id";
        private static string DEFAULT_SORT_ORDER => "ASC";

        public static PagedList<Item> HandleDataTablesRequest(this IQueryable<Item> source, string requestOptions)
        {
            var dtOptions = JsonConvert.DeserializeObject<DataTablesOptions>(requestOptions);
            var sortColumn = dtOptions.Columns[dtOptions.Order.First().Column].Data;
            var sortColumnDirection = dtOptions.Order.First(o => !string.IsNullOrWhiteSpace(o.Dir)).Dir;
            var searchValue = dtOptions.Search.Value;

            //Search  
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                source = source.Where(m => m.Name.IndexOf(searchValue, System.StringComparison.OrdinalIgnoreCase) >= 0);
            }

            //Sorting  
            source = source.OrderBy($"{ sortColumn ?? DEFAULT_SORT_COLUMN } { sortColumnDirection ?? DEFAULT_SORT_ORDER}");
            return PagedList<Item>.Create(source, dtOptions);
        }
    }
}
