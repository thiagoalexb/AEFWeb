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
using AEFWeb.Data.Entities;
using Newtonsoft.Json;

namespace AEFWeb.Implementation.Services
{
    public class LessonService : Service<ILessonRepository>, ILessonService
    {
        private readonly IMapper _mapper;
        private readonly IModuleService _moduleService;

        public LessonService(IMapper mapper,
                            IMediatorHandler bus,
                            IUnitOfWork unitOfWork,
                            IModuleService moduleService) : base(bus, unitOfWork)
        {
            _mapper = mapper;
            _moduleService = moduleService;
        }

        public Task<IEnumerable<LessonsViewModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LessonsViewModel> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginateResultBase<LessonsViewModel>> GetPaginateAsync(PaginateFilterBase filter)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(LessonsViewModel viewModel)
        {
            foreach (var item in viewModel.Lessons)
            {
                item.Id = Guid.NewGuid();
                item.ModuleId = viewModel.ModuleId;
                item.SpecialWeekId = viewModel.SpecialWeekId;
                var lesson = _mapper.Map<Lesson>(item);
                await _repository.AddAsync(lesson);
            }

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        }

        public async Task UpdateAsync(LessonsViewModel viewModel)
        {
            foreach (var item in viewModel.Lessons)
            {
                item.Id = Guid.NewGuid();
                item.ModuleId = viewModel.ModuleId;
                item.SpecialWeekId = viewModel.SpecialWeekId;
                var lesson = _mapper.Map<Lesson>(item);
                await _repository.AddAsync(lesson);
            }

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Update"));
        }

        public Task RemoveAsync(LessonsViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task RestoreAsync(LessonsViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        private async Task<Guid> GetModule(LessonsViewModel viewModel)
        {
            if (viewModel.ModuleId.HasValue) return viewModel.ModuleId.Value;

            viewModel.Module.Id = Guid.NewGuid();

            await _moduleService.AddAsync(viewModel.Module);
            return viewModel.Module.Id;
        }
    }
}
