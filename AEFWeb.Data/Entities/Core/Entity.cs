﻿using System;

namespace AEFWeb.Data.Entities.Core
{
    public abstract class Entity
    {
        public Entity(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
        public bool Deleted { get; private set; } = false;

        public void SetDeleted(bool value)
        {
            Deleted = value;
        }
    }
}
