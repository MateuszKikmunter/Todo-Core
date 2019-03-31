using System;

namespace ToDoItem.Web.Models
{
    public class ItemViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string AdditionalInformation { get; set; }

        public DateTime DeadLine { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool Completed { get; set; }

        public Guid UserId { get; set; }
    }
}
