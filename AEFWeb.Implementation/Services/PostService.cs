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
using System.Threading.Tasks;
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

        public async Task<PostViewModel> Get(Guid id)
        {
            var postdb = await _repository.Get(id);
            var post = _mapper.Map<PostViewModel>(postdb);
            if (post != null)
            {
                var tags = _mapper.Map<IEnumerable<TagViewModel>>(postdb.PostTags.Select(x => x.Tag));
                post.Tags.AddRange(tags);
            }

            return post;
        }
        public async Task<IEnumerable<PostViewModel>> GetAll()
        {

            var list = new List<PostViewModel>();
            foreach (var item in await _repository.GetAll())
            {
                var post = _mapper.Map<PostViewModel>(item);
                if (post != null)
                {
                    var tags = _mapper.Map<IEnumerable<TagViewModel>>(item.PostTags.Select(x => x.Tag));
                    post.Tags.AddRange(tags);
                }
                list.Add(post);
            }
            return list;
        }


        public async Task Add(PostViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();

            var _post = _mapper.Map<Post>(viewModel);

            foreach (var tag in GetNewTags(viewModel.Tags))
            {
                await _postTagRepository.Add(new PostTag()
                {
                    Post = _post,
                    Tag = tag
                });
            }

            foreach (var id in GetExistingTags(viewModel.Tags))
            {
                await _postTagRepository.Add(new PostTag()
                {
                    Post = _post,
                    TagId = id
                });
            }

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        }

        public async Task Update(PostViewModel viewModel)
        {
            var _post = await _repository.Get(viewModel.Id);

            if (_post != null)
            {
                _unitOfWork.BeginTransaction();
                _postTagRepository.RemoveRange(_post.PostTags);
                if (await Commit())
                {
                    _post = _mapper.Map(viewModel, _post);

                    foreach (var tag in GetNewTags(viewModel.Tags))
                    {
                        await _postTagRepository.Add(new PostTag()
                        {
                            Post = _post,
                            Tag = tag
                        });
                    }

                    foreach (var id in GetExistingTags(viewModel.Tags))
                    {
                        await _postTagRepository.Add(new PostTag()
                        {
                            Post = _post,
                            TagId = id
                        });
                    }

                    if (await Commit())
                        await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Update"));

                    _unitOfWork.CommitTransaction();
                    _unitOfWork.EndTransaction();
                }
                else
                {
                    _unitOfWork.EndTransaction();
                    await _bus.RaiseEvent(new Notification("defaultError", "Erro inesperado, tente novamente!"));
                }
            }
            else
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Post não encontrado"));
            }

        }

        public async Task Remove(PostViewModel viewModel)
        {
            var post = await _repository.Get(viewModel.Id);
            post.SetDeleted(true);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public async Task Restore(PostViewModel viewModel)
        {
            var post = await _repository.Get(viewModel.Id);
            post.SetDeleted(false);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }

        private IEnumerable<Tag> GetNewTags(List<TagViewModel> list)
        {
            var news = list.Where(x => x.Id == Guid.Empty);
            foreach (var item in news)
            {
                yield return new Tag(Guid.Empty, item.Name);
            }
        }

        private IEnumerable<Guid> GetExistingTags(List<TagViewModel> list)
        {
            var news = list.Where(x => x.Id != Guid.Empty);
            foreach (var item in news)
            {
                yield return item.Id;
            }
        }
    }
}
