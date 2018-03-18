using AEFWeb.Core.Repositories.Core;
using AEFWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEFWeb.Core.Repositories
{
    public interface IPostTagRepository : IRepository<PostTag>
    {
        Task<PostTag> Get(Guid postId, Guid tagId);
        void RemoveRange(IEnumerable<PostTag> list);
    }
}
