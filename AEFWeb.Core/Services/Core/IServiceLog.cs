using System.Threading.Tasks;

namespace AEFWeb.Core.Services.Core
{
    public interface IServiceLog<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
    }
}
