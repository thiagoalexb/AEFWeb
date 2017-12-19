using System;
using AEFWeb.Core.Repositories;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Repositories.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AEFWeb.Implementation.Repositories
{
    public class PostTagRepository : Repository<PostTag>, IPostTagRepository
    {
        public PostTagRepository(DbContext context) : base(context)
        { }

        public PostTag Get(Guid postId, Guid tagId)
        {
            return DbSet.FirstOrDefault(x => x.PostId == postId && x.TagId == tagId);
        }
    }
}
