using AEFWeb.Core.Repositories.Core;
using AEFWeb.Data.Entities;
using System;

namespace AEFWeb.Core.Repositories
{
    public interface IPostTagRepository : IRepository<PostTag>
    {
        PostTag Get(Guid postId, Guid tagId);
    }
}
