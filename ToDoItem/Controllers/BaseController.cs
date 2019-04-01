using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoItem.Web.Helpers.DataTablesServerSide;

namespace ToDoItem.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected OkObjectResult DataTablesResult<T, TMappingResult>(PagedList<T> items)
        {
            return Ok(new
            {
                recordsTotal = items.TotalCount,
                recordsFiltered = items.TotalCount,
                data = _mapper.Map<IList<TMappingResult>>(items)
            });
        }
    }
}