using AEFWeb.Core.Repositories;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Repositories.Core;
using Microsoft.EntityFrameworkCore;

namespace AEFWeb.Implementation.Repositories
{
    public class EventLogRepository : Repository<EventLog>, IEventLogRepository
    {
        public EventLogRepository(DbContext context) : base(context)
        { }
    }
}
