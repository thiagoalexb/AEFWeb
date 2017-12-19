using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Core.ViewModels;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Implementation.Services.Core;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AEFWeb.Implementation.Services
{
    public class TagService : Service<ITagRepository>, ITagService
    {
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork,
                        IMapper mapper,
                        IMediatorHandler bus
                        ) : base(bus, unitOfWork) => _mapper = mapper;


        public TagViewModel Get(Guid id) => 
            _mapper.Map<TagViewModel>(_repository.Get(id));

        public IEnumerable<TagViewModel> GetAll() => 
            _mapper.Map<IEnumerable<TagViewModel>>(_repository.GetAll());

        public void Add(TagViewModel viewModel)
        {
            if (_repository.Find(x => x.Name.ToLower() == viewModel.Name.ToLower()).Count() > 0)
            {
                _bus.RaiseEvent(new Notification("Esta tag já está cadastrada"));
                return;
            }

            var tag = _mapper.Map<Tag>(viewModel);
            _repository.Add(tag);

            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(tag), Type));
        }

        public void Update(TagViewModel viewModel)
        {
            var tag = _repository.Get(viewModel.Id);

            if (tag != null)
            {
                if (tag.Name != viewModel.Name && _repository.Find(x => x.Name.ToLower() == viewModel.Name.ToLower()).Count() > 0)
                {
                    _bus.RaiseEvent(new Notification("Esta tag já está cadastrada"));
                    return;
                }

                _mapper.Map(viewModel, tag);
                if (Commit())
                    RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(tag), Type));
            }
            else
            {
                _bus.RaiseEvent(new Notification("Tag não encontrada"));
            }
        }

        public void Remove(TagViewModel viewModel)
        {
            var tag = _repository.Get(viewModel.Id);
            _repository.Remove(tag);

            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(tag), Type));
        }
    }
}
