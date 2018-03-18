using AEFWeb.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace AEFWeb.Implementation.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction transaction;
        private readonly DbContext _context;
        private Hashtable repositories;

        public UnitOfWork(DbContext context) =>
            _context = context;

        public TEntity Repository<TEntity>() where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Hashtable();
            }

            var type = typeof(TEntity);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);

            foreach (var item in types)
            {
                if (!repositories.ContainsKey(item))
                {
                    repositories.Add(type, Activator.CreateInstance(item, _context));
                }
            }

            return (TEntity)repositories[type];
        }

        public async Task<bool> Complete()
        {
            var isSaved = await _context.SaveChangesAsync() > 0;
            return isSaved;
        }

        public void Dispose() =>
            _context.Dispose();

        public void BeginTransaction() => 
            transaction = _context.Database.BeginTransaction();

        public void CommitTransaction() => 
            _context.Database.CommitTransaction();

        public void RollBack() => 
            _context.Database.RollbackTransaction();

        public void EndTransaction() => 
            _context.Database.CloseConnection();
    }
}
