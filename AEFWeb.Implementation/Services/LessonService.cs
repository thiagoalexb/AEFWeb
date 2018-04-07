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
using System.Linq;
using AEFWeb.Implementation.Notifications;

namespace AEFWeb.Implementation.Services
{
    public class LessonService : Service<ILessonRepository>, ILessonService
    {
        private readonly IMapper _mapper;
        private readonly IModuleService _moduleService;
        private readonly IFaseService _faseService;
        private readonly ISpecialWeekService _specialWeekService;

        public LessonService(IMapper mapper,
                            IMediatorHandler bus,
                            IUnitOfWork unitOfWork,
                            IModuleService moduleService,
                            IFaseService faseService,
                            ISpecialWeekService specialWeekService) : base(bus, unitOfWork)
        {
            _mapper = mapper;
            _moduleService = moduleService;
            _faseService = faseService;
            _specialWeekService = specialWeekService;
        }

        public async Task<LessonViewModel> GetAsync(Guid id) =>
            _mapper.Map<LessonViewModel>(await _repository.GetAsync(id));

        public async Task<IEnumerable<LessonViewModel>> GetAllAsync() =>
            _mapper.Map<IEnumerable<LessonViewModel>>(await _repository.GetAllAsync());

        public async Task AddAsync(LessonViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();
            var lesson = _mapper.Map<Lesson>(viewModel);
            await _repository.AddAsync(lesson);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        }

        public async Task UpdateAsync(LessonViewModel viewModel)
        {
            var lesson = await _repository.GetAsync(viewModel.Id);

            if (lesson != null)
            {
                _mapper.Map(viewModel, lesson);

                if (await Commit())
                    await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Update"));
            }
            else
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Título não encontrado"));
            }
        }

        public async Task<IEnumerable<object>> GetAllLessonsAsync () =>
            new List<object> ()
                //fases
                .Concat(await _faseService.GetAllAsync())
                //special weeks
                .Concat(await _specialWeekService.GetAllAsync())
                //not linked lessons
                .Concat(await _repository.GetQueryableByCriteria(l =>
                    !l.ModuleId.HasValue &&
                    !l.SpecialWeekId.HasValue
                ));

        public Task<PaginateResultBase<LessonViewModel>> GetPaginateAsync(PaginateFilterBase filter)
        {
            throw new NotImplementedException();
        }

        public async Task AddLessonsAsync(LessonsViewModel viewModel)
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

        public async Task UpdateLessonsAsync(LessonsViewModel viewModel)
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

        public Task RemoveAsync(LessonViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task RestoreAsync(LessonViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        private async Task<Guid> GetModule(LessonViewModel viewModel)
        {
            if (viewModel.ModuleId.HasValue) return viewModel.ModuleId.Value;

            viewModel.Module.Id = Guid.NewGuid();

            await _moduleService.AddAsync(viewModel.Module);
            return viewModel.Module.Id;
        }
    }
}
