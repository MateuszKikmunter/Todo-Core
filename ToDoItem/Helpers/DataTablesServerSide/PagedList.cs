using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoItem.Web.Helpers.DataTablesServerSide
{
    public class PagedList<T> : List<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PagesCount { get; set; }

        private PagedList(IQueryable<T> queryable, DataTablesOptions paginationOptions)
        {
            TotalCount = queryable.Count();
            PageNumber = (int)Math.Ceiling(paginationOptions.Start / (double)paginationOptions.Length) + 1;
            PageSize = paginationOptions.Length;
            PagesCount = (int)Math.Ceiling(TotalCount / (double)PageSize);

            AddRange(queryable.Skip(paginationOptions.Start).Take(PageSize).ToList());
        }

        public static PagedList<T> Create(IQueryable<T> source, DataTablesOptions paginationData)
        {
            return new PagedList<T>(source, paginationData);
        }
    }
}
