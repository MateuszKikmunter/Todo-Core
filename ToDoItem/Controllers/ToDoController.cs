using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoItem.Core.Abstract;
using ToDoItem.Core.Entities;
using ToDoItem.Infrastructure.Identity;
using ToDoItem.Web.Helpers.DataTablesServerSide;
using ToDoItem.Web.Helpers.Extensions;
using ToDoItem.Web.ViewModels;

namespace ToDoItem.Web.Controllers
{
    [Authorize]
    public class ToDoController : BaseController
    {
        private readonly IToDoItemRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDoController(IToDoItemRepository repository, UserManager<ApplicationUser> userManager, IMapper mapper) : base(mapper)
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
            
            return DataTablesResult<Item, ItemViewModel>(data);
        }

        [HttpGet("{id}", Name = "get-item")]
        public async Task<IActionResult> GetItem([FromQuery] Guid id)
        {
            var item = await _repository.GetSingleAsync(id);
            if (item == null)
            {
                return PartialView("_ManipulateItemPartial", new ItemForManipulationViewModel());
            }

            return PartialView("_ManipulateItemPartial", _mapper.Map<ItemForManipulationViewModel>(item));
        }

        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var itemToDelete = await _repository.GetSingleAsync(id);
            if (itemToDelete == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return Ok();
        }

        [HttpPost]
        [Route("create-item")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem([FromBody] ItemForManipulationViewModel item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var itemToAdd = _mapper.Map<Item>(item);
            itemToAdd.UserId = new Guid(user.Id);

            await _repository.CreateAsync(itemToAdd);
            return CreatedAtRoute("get-item", new { id = itemToAdd.Id }, itemToAdd);
        }

        [HttpPut("{id}")]
        [Route("change-status")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeItemStatus(Guid id)
        {
            var item = await _repository.GetSingleAsync(id);
            if (item == null)
            {
                return BadRequest();
            }

            await _repository.ChangeItemStatusAsync(item);
            return Ok();
        }

        [HttpPut("update-item")]
     //   [Route("update-item")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody] ItemForManipulationViewModel item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var itemToUpdate = await _repository.GetSingleAsync(item.Id);
            if (itemToUpdate == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            _mapper.Map(item, itemToUpdate);
            await _repository.UpdateAsync(itemToUpdate);
            return NoContent();
        }
    }
}