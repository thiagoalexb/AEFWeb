using AEFWeb.Data.Entities.Core;
using System;
using System.Collections.Generic;

namespace AEFWeb.Data.Entities
{
    public class Module : Entity
    {
        public Module(Guid id, 
            Guid faseId, 
            string name, 
            string description) : base(id)
        {
            FaseId = faseId;
            Name = name;
            Description = description;
        }

        public Module() : base(Guid.NewGuid()) { }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public Guid FaseId { get; private set; }
        public Fase Fase { get; private set; }

        public ICollection<Lesson> Lessons { get; private set; } = new List<Lesson>();
    }
}
