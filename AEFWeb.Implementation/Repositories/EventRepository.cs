using AEFWeb.Core.Repositories;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Repositories.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AEFWeb.Implementation.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(DbContext context) : base(context)
        { }

        //public override IEnumerable<Event> GetAll() =>
        //    DbSet.Include(x => x.Lessons).ToList();

        //public override Event Get(Guid id) =>
        //    DbSet.Include(x => x.Lessons).FirstOrDefault(x => x.Id == id);

        //public override IEnumerable<Event> Find(Expression<Func<Event, bool>> predicate) =>
        //   DbSet.Include(x => x.Lessons).Where(predicate);

        //public override Event GetByCriteria(Expression<Func<Event, bool>> predicate) =>
        //    DbSet.Include(x => x.Lessons).FirstOrDefault(predicate);
    }
}
