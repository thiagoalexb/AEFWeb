namespace AEFWeb.Core.Services.Core
{
    public interface IServiceLog<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
    }
}
