using AEFWeb.Data.Entities.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AEFWeb.Data.Entities
{
    public class Lesson : Entity
    {
        public Lesson(Guid id, 
            string title, 
            string subTitle, 
            string description, 
            string code,
            Guid? moduleId,
            Guid? specialWeekId) : base(id)
        {
            Title = title;
            SubTitle = subTitle;
            Description = description;
            ModuleId = moduleId;
            SpecialWeekId = specialWeekId;
        }

        public Lesson() : base(Guid.NewGuid()) { }

        public string Title { get; private set; }
        public string SubTitle { get; private set; }
        public string Description { get; private set; }
        public int Code { get; private set; }

        public Guid? ModuleId { get; private set; }
        public Module Module { get; private set; }

        public Guid? SpecialWeekId { get; private set; }
        public SpecialWeek SpecialWeek { get; private set; }

        public ICollection<Event> Events { get; private set; } = new List<Event>();
    }
}
