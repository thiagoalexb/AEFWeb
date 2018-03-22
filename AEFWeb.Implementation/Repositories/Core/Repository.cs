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

        public virtual async Task<TEntity> GetAsync(Guid id) =>
            await DbSet.FindAsync(id);

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await DbSet.ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) =>
            await DbSet.Where(predicate).ToListAsync();

        public virtual async Task<TEntity> GetByCriteriaAsync(Expression<Func<TEntity, bool>> predicate) =>
            await DbSet.FirstOrDefaultAsync(predicate);

        public async Task<IQueryable<TEntity>> GetQueryable() =>
            await Task.FromResult(DbSet.AsQueryable());

        public async Task<IQueryable<TEntity>> GetQueryableByCriteria(Expression<Func<TEntity, bool>> predicate) => 
            await Task.FromResult(DbSet.Where(predicate).AsQueryable());

        public virtual async Task AddAsync(TEntity entity) =>
            await DbSet.AddAsync(entity);

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) =>
            await DbSet.AddRangeAsync(entities);

        
    }
}
