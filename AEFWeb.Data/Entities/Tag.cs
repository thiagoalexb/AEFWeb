using AEFWeb.Data.Entities.Core;
using System;
using System.Collections.Generic;

namespace AEFWeb.Data.Entities
{
    public class Tag : Entity
    {
        public Tag(Guid id, string name) : base(id)
        {
            Name = name;
        }

        public Tag() : base(Guid.NewGuid()) { }

        public string Name { get; private set; }

        public ICollection<PostTag> PostTags { get; } = new List<PostTag>();
    }
}
