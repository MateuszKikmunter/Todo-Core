using System;

namespace ToDoItem.Web.ViewModels
{
    public class ItemViewModel
    {
        public Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime Deadline { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool Completed { get; set; }
    }
}
