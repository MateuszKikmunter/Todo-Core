using Microsoft.AspNetCore.Mvc;

namespace ToDoItem.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
