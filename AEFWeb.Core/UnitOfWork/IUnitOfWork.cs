using AEFWeb.Core.Repositories.Core;
using System;

namespace AEFWeb.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        bool Complete();
        TEntity Repository<TEntity>() where TEntity : class;
        void BeginTransaction();
        void EndTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
