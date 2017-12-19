using AEFWeb.Data.Entities.Core;
using System;

namespace AEFWeb.Data.Entities
{
    public class EventLog : Entity
    {
        public EventLog(Guid id, DateTime? creationDate, Guid? creatorUserId, DateTime? lastUpdateDate, Guid? lastUpdatedUserId, string data, string type) : base(id)
        {
            CreationDate = creationDate;
            CreatorUserId = creatorUserId;
            LastUpdateDate = lastUpdateDate;
            LastUpdatedUserId = lastUpdatedUserId;
            Data = data;
            Type = type;
        }

        public EventLog() : base(Guid.NewGuid()) { }

        public DateTime? CreationDate { get; private set; }
        public Guid? CreatorUserId { get; private set; }

        public DateTime? LastUpdateDate { get; private set; }
        public Guid? LastUpdatedUserId { get; private set; }

        public string Data { get; private set; }
        public string Type { get; set; }
    }
}
