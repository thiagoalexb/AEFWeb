using System;
using AEFWeb.Core.Repositories;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Repositories.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AEFWeb.Implementation.Repositories
{
    public class PostTagRepository : Repository<PostTag>, IPostTagRepository
    {
        public PostTagRepository(DbContext context) : base(context)
        { }

        public async Task<PostTag> Get(Guid postId, Guid tagId)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.PostId == postId && x.TagId == tagId);
        }

        public void RemoveRange(IEnumerable<PostTag> list)
        {
            DbSet.RemoveRange(list);
        }
    }
}
