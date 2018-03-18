using AEFWeb.Core.Repositories.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AEFWeb.Implementation.Repositories.Core
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        public virtual async Task<TEntity> Get(Guid id) =>
            await DbSet.FindAsync(id);

        public virtual async Task<IEnumerable<TEntity>> GetAll() =>
            await DbSet.ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate) =>
            await DbSet.Where(predicate).ToListAsync();

        public virtual async Task<TEntity> GetByCriteria(Expression<Func<TEntity, bool>> predicate) =>
            await DbSet.FirstOrDefaultAsync(predicate);

        public virtual async Task Add(TEntity entity) =>
            await DbSet.AddAsync(entity);

        public virtual async Task AddRange(IEnumerable<TEntity> entities) =>
            await DbSet.AddRangeAsync(entities);
    }
}
