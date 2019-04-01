using AutoMapper;
using ToDoItem.Core.Entities;
using ToDoItem.Web.ViewModels;

namespace ToDoItem.Web.Config
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Item, ItemViewModel>().ReverseMap();
            CreateMap<Item, ItemForManipulationViewModel>().ReverseMap();
        }
    }
}
