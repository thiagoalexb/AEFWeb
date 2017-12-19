using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Core.ViewModels;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Implementation.Services.Core;
using AutoMapper;
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

        public void Add(PostViewModel post)
        {
            var _post = _mapper.Map<Post>(post);

            _postTagRepository.AddRange(post.Tags.Select(x => new PostTag()
            {
                Post = _post,
                TagId = x.Id
            }));

            Commit();
        }

        public void Update(PostViewModel post)
        {
            var _post = _repository.Get(post.Id);


            if (_post != null)
            {
                _unitOfWork.BeginTransaction();
                _postTagRepository.RemoveRange(_post.PostTags);
                if (Commit())
                {
                    _post = _mapper.Map(post, _post);
                    if (Commit())
                    {
                        _postTagRepository.AddRange(post.Tags.Select(x => new PostTag()
                        {
                            Post = _post,
                            TagId = x.Id
                        }));

                        Commit();
                        _unitOfWork.CommitTransaction();
                        _unitOfWork.EndTransaction();
                    }
                    else
                    {
                        _unitOfWork.EndTransaction();
                    }
                }
                else
                {
                    _unitOfWork.EndTransaction();
                }
            }
            else
            {
                _bus.RaiseEvent(new Notification("Post não encontrado"));
            }

        }

        public void Remove(PostViewModel viewModel)
        {
            var post = _repository.Get(viewModel.Id);
            var postTag = _postTagRepository.Find(x => x.PostId == viewModel.Id);
            _postTagRepository.RemoveRange(postTag);
            _repository.Remove(post);
            Commit();
        }
    }
}
