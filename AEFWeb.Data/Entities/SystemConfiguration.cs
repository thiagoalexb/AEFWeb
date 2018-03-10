using AEFWeb.Data.Entities.Core;
using System;

namespace AEFWeb.Data.Entities
{
    public class SystemConfiguration : Entity
    {
        public SystemConfiguration(Guid id, string key, string value) : base(id)
        {
            Key = key;
            Value = value;
        }

        public SystemConfiguration() : base(Guid.NewGuid()) { }

        public string Key { get; private set; }
        public string Value { get; private set; }
    }
}
