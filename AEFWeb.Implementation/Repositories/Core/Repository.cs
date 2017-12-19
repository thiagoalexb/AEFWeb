using AEFWeb.Core.Repositories.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        public virtual TEntity Get(Guid id) =>
            DbSet.Find(id);

        public virtual IEnumerable<TEntity> GetAll() =>
            DbSet.ToList();

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) =>
            DbSet.Where(predicate);

        public virtual TEntity GetByCriteria(Expression<Func<TEntity, bool>> predicate) =>
            DbSet.FirstOrDefault(predicate);

        public virtual void Add(TEntity entity) =>
            DbSet.Add(entity);

        public virtual void AddRange(IEnumerable<TEntity> entities) =>
            DbSet.AddRange(entities);

        public virtual void Remove(TEntity entity) =>
            DbSet.Remove(entity);

        public virtual void RemoveRange(IEnumerable<TEntity> entities) =>
            DbSet.RemoveRange(entities);
    }
}
