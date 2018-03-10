using AEFWeb.Data.Entities.Core;
using System;

namespace AEFWeb.Data.Entities
{
    public class Event : Entity
    {
        public Event(Guid id,
            Guid lessonId,
            DateTime startDate, 
            DateTime endDate) : base(id)
        {
            StartDate = startDate;
            EndDate = endDate;
            LessonId = lessonId;
        }

        public Event() : base(Guid.NewGuid()) { }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Guid LessonId { get; private set; }
        public Lesson Lesson { get; private set; }
    }
}
