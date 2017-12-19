using System;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Repositories.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using AEFWeb.Core.Repositories;
using System.Linq.Expressions;

namespace AEFWeb.Implementation.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(DbContext context) : base(context)
        { }

        public override IEnumerable<Post> GetAll() =>
            DbSet.Include(x => x.User)
                                .Include(x => x.PostTags)
                                    .ThenInclude(x => x.Tag)
                                    .ToList();

        public override Post Get(Guid id) =>
            DbSet.Include(x => x.User)
                                .Include(x => x.PostTags)
                                    .ThenInclude(x => x.Tag)
                                    .FirstOrDefault(x => x.Id == id);

        public override IEnumerable<Post> Find(Expression<Func<Post, bool>> predicate) =>
           DbSet.Include(x => x.User)
                                .Include(x => x.PostTags)
                                    .ThenInclude(x => x.Tag)
                                    .Where(predicate);

        public override Post GetByCriteria(Expression<Func<Post, bool>> predicate) =>
            DbSet.Include(x => x.User)
                                    .Include(x => x.PostTags)
                                        .ThenInclude(x => x.Tag)
                                        .FirstOrDefault(predicate);
    }
}
