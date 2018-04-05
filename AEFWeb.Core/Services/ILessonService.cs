using AEFWeb.Core.Services.Core;
using AEFWeb.Core.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AEFWeb.Core.Services
{
    public interface ILessonService : IService<LessonViewModel>
    {
        Task<IEnumerable<object>> GetAllLessonsAsync ();
        Task AddLessonsAsync(LessonsViewModel viewModel);
        Task UpdateLessonsAsync(LessonsViewModel viewModel);
    }
}
