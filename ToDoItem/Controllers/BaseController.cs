using Microsoft.AspNetCore.Mvc;
using ToDoItem.Web.Helpers.DataTablesServerSide;

namespace ToDoItem.Web.Controllers
{
    public class BaseController : Controller
    {
        protected OkObjectResult DataTablesResult<T>(PagedList<T> items)
        {
            return Ok(new
            {
                recordsTotal = items.TotalCount,
                recordsFiltered = items.TotalCount,
                data = items
            });
        }
    }
}