using AEFWeb.Data.Entities.Core;
using System;

namespace AEFWeb.Data.Entities
{
    public class Lesson : Entity
    {
        public Lesson(Guid id, DateTime schedule, string title, string subTitle, Guid eventId, Event @event) : base(id)
        {
            Schedule = schedule;
            Title = title;
            SubTitle = subTitle;
            EventId = eventId;
            Event = @event;
        }

        public Lesson() : base(Guid.NewGuid()) { }

        public DateTime Schedule { get; private set; }
        public string Title { get; private set; }
        public string SubTitle { get; private set; }

        public Guid EventId { get; private set; }
        public Event Event { get; private set; }
    }
}
