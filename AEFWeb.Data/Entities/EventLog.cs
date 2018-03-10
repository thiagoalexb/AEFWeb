using AEFWeb.Data.Entities.Core;
using System;

namespace AEFWeb.Data.Entities
{
    public class EventLog : Entity
    {
        public EventLog(Guid id, DateTime? creationDate, Guid? creatorUserId, DateTime? updateDate, Guid? updatedUserId, string data, string type, string action) : base(id)
        {
            CreationDate = creationDate;
            CreatorUserId = creatorUserId;
            UpdateDate = updateDate;
            UpdatedUserId = updatedUserId;
            Data = data;
            Type = type;
            Action = action;
        }

        public EventLog() : base(Guid.NewGuid()) { }

        public DateTime? CreationDate { get; private set; }
        public Guid? CreatorUserId { get; private set; }

        public DateTime? UpdateDate { get; private set; }
        public Guid? UpdatedUserId { get; private set; }

        public string Data { get; private set; }
        public string Type { get; private set; }
        public string Action { get; private set; }
    }
}
