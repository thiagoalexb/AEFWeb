using AEFWeb.Core.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEFWeb.Core.Services.Core
{
    public interface IService<TViewModel> where TViewModel : class
    {
        Task<TViewModel> GetAsync(Guid id);
        Task<IEnumerable<TViewModel>> GetAllAsync();
        Task<PaginateResultBase<TViewModel>> GetPaginateAsync(PaginateFilterBase filter);

        Task AddAsync(TViewModel viewModel);
        Task UpdateAsync(TViewModel viewModel);
        Task RemoveAsync(TViewModel viewModel);
        Task RestoreAsync(TViewModel viewModel);
    }
}
