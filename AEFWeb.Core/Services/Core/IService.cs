using System;
using System.Collections.Generic;

namespace AEFWeb.Core.Services.Core
{
    public interface IService<TViewModel> where TViewModel : class
    {
        TViewModel Get(Guid id);
        IEnumerable<TViewModel> GetAll();

        void Add(TViewModel viewModel);
        void Update(TViewModel viewModel);
        void Remove(TViewModel viewModel);
        void Restore(TViewModel viewModel);
    }
}
