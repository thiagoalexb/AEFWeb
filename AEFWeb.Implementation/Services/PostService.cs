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
using System.Transactions;

namespace AEFWeb.Implementation.Services
{
    public class PostService : Service<IPostRepository>, IPostService
    {
        private readonly IMapper _mapper;
        private readonly IPostTagRepository _postTagRepository;

        public PostService(IUnitOfWork unitOfWork,
                        IMapper mapper,
                        IMediatorHandler bus,
                        IPostTagRepository postTagRepository) : base(bus, unitOfWork)
        {
            _mapper = mapper;
            _postTagRepository = unitOfWork.Repository<IPostTagRepository>();
        }

        public PostViewModel Get(Guid id) =>
            _mapper.Map<PostViewModel>(_repository.Get(id));

        public IEnumerable<PostViewModel> GetAll() =>
            _mapper.Map<IEnumerable<PostViewModel>>(_repository.GetAll());

        public void Add(PostViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();

            var _post = _mapper.Map<Post>(viewModel);

            _postTagRepository.AddRange(viewModel.Tags.Select(x => new PostTag()
            {
                Post = _post,
                TagId = x.Id
            }));

            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        }

        public void Update(PostViewModel viewModel)
        {
            var post = _repository.Get(viewModel.Id);

            if (post != null)
            {
                _unitOfWork.BeginTransaction();
                _postTagRepository.RemoveRange(post.PostTags);
                if (Commit())
                {
                    post = _mapper.Map(viewModel, post);
                    if (Commit())
                    {
                        _postTagRepository.AddRange(viewModel.Tags.Select(x => new PostTag()
                        {
                            Post = post,
                            TagId = x.Id
                        }));

                        if (Commit())
                            RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Update"));

                        _unitOfWork.CommitTransaction();
                        _unitOfWork.EndTransaction();
                    }
                    else
                    {
                        _unitOfWork.EndTransaction();
                        _bus.RaiseEvent(new Notification("defaultError", "Erro inesperado, tente novamente!"));
                    }
                }
                else
                {
                    _unitOfWork.EndTransaction();
                    _bus.RaiseEvent(new Notification("defaultError", "Erro inesperado, tente novamente!"));
                }
            }
            else
            {
                _bus.RaiseEvent(new Notification("defaultError", "Post não encontrado"));
            }

        }

        public void Remove(PostViewModel viewModel)
        {
            var post = _repository.Get(viewModel.Id);
            post.SetDeleted(true);

            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public void Restore(PostViewModel viewModel)
        {
            var post = _repository.Get(viewModel.Id);
            post.SetDeleted(false);

            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }
    }
}
