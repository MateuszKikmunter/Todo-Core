﻿using System;

namespace ToDoItem.Core.Entities
{
    public class Item
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string AdditionalInformation { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public bool Completed { get; set; }

        public Guid UserId { get; set; }
    }
}
