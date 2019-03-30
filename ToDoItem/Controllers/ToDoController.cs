using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoItem.Core.Abstract;

namespace ToDoItem.Web.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly IToDoItemRepository _repository;

        public ToDoController(IToDoItemRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}