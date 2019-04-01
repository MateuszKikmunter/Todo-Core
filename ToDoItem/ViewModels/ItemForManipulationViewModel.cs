using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoItem.Web.ViewModels
{
    public class ItemForManipulationViewModel : ItemViewModel
    {
        [Required]
        [MaxLength(155)]
        public override string Name { get; set; }

        [MaxLength(255)]
        public string AdditionalInformation { get; set; }

        [Required]
        public override DateTime Deadline { get; set; } = DateTime.Now;
    }
}
