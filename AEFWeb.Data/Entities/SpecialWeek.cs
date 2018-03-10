using AEFWeb.Data.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AEFWeb.Data.Entities
{
    public class SpecialWeek : Entity
    {
        public SpecialWeek(Guid id,
            string title,
            string description) : base(id)
        {
            Title = title;
            Description = description;
        }

        public SpecialWeek() : base(Guid.NewGuid())
        { }

        public string Title { get; private set; }
        public string Description { get; private set; }

        public ICollection<Lesson> Lessons { get; private set; } = new List<Lesson>();
    }
}
