using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Implementation.Services.Core;
using System;
using System.Collections.Generic;
using System.Text;
using AEFWeb.Core.ViewModels;
using AEFWeb.Core.ViewModels.Core;
using System.Threading.Tasks;
using AEFWeb.Core.Notifications;
using AEFWeb.Core.UnitOfWork;
using AutoMapper;

namespace AEFWeb.Implementation.Services
{
    public class LessonService : Service<ILessonRepository>, ILessonService
    {
        private readonly IMapper _mapper;

        public LessonService(IMapper mapper,
                            IMediatorHandler bus,
                            IUnitOfWork unitOfWork) : base(bus, unitOfWork) => _mapper = mapper;

        public Task AddAsync(LessonViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LessonViewModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LessonViewModel> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginateResultBase<LessonViewModel>> GetPaginateAsync(PaginateFilterBase filter)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(LessonViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task RestoreAsync(LessonViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(LessonViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
