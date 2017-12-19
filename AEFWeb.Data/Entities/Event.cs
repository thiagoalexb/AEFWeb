using AEFWeb.Data.Entities.Core;
using System;
using System.Collections.Generic;

namespace AEFWeb.Data.Entities
{
    public class Event : Entity
    {
        public Event(Guid id, DateTime date) : base(id)
        {
            Date = date;
        }

        public Event() : base(Guid.NewGuid()) { }

        public DateTime Date { get; private set; }

        public ICollection<Lesson> Lessons { get; set; }
    }
}
