using AEFWeb.Data.Entities.Core;
using System;
using System.Collections.Generic;

namespace AEFWeb.Data.Entities
{
    public class Fase : Entity
    {
        public Fase(Guid id, string name) : base(id)
            => Name = name;

        public Fase() : base(Guid.NewGuid()) { }

        public string Name { get; private set; }

        public ICollection<Module> Modules { get; private set; } = new List<Module>();
    }
}
