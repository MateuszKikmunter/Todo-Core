using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoItem.Core.Abstract;
using ToDoItem.Infrastructure.Identity;
using ToDoItem.Web.Helpers;

namespace ToDoItem.Web.Controllers
{
    [Authorize]
    public class ToDoController : BaseController
    {
        private readonly IToDoItemRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDoController(IToDoItemRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("get-items")]
        public async Task<IActionResult> GetItems([FromQuery] string options)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var data = _repository.GetAll()
                .Where(i => i.UserId == new Guid(user.Id))
                .HandleDataTablesRequest(options);

            return DataTablesResult(data);
        }
    }
}