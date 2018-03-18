using AEFWeb.Core.Repositories.Core;
using System;
using System.Threading.Tasks;

namespace AEFWeb.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Complete();
        TEntity Repository<TEntity>() where TEntity : class;
        void BeginTransaction();
        void EndTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
