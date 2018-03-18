using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEFWeb.Core.Services.Core
{
    public interface IService<TViewModel> where TViewModel : class
    {
        Task<TViewModel> Get(Guid id);
        Task<IEnumerable<TViewModel>> GetAll();

        Task Add(TViewModel viewModel);
        Task Update(TViewModel viewModel);
        Task Remove(TViewModel viewModel);
        Task Restore(TViewModel viewModel);
    }
}
