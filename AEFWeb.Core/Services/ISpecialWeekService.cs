using AEFWeb.Core.Services.Core;
using AEFWeb.Core.ViewModels;
using AEFWeb.Core.ViewModels.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEFWeb.Core.Services
{
    public interface ISpecialWeekService : IService<SpecialWeekViewModel>
    {
        Task<List<AutoCompleteViewModel>> GetAutoCompleteAsync(string search);
    }
}
