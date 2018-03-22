using System;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Repositories.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using AEFWeb.Core.Repositories;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AEFWeb.Implementation.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(DbContext context) : base(context)
        { }

        public override async Task<IEnumerable<Post>> GetAllAsync() =>
            await DbSet.Include(x => x.User)
                                .Include(x => x.PostTags)
                                    .ThenInclude(x => x.Tag)
                                    .ToListAsync();

        public override async Task<Post> GetAsync(Guid id) =>
            await DbSet.Include(x => x.User)
                                .Include(x => x.PostTags)
                                    .ThenInclude(x => x.Tag)
                                    .FirstOrDefaultAsync(x => x.Id == id);

        public override async Task<IEnumerable<Post>> FindAsync(Expression<Func<Post, bool>> predicate) =>
           await DbSet.Include(x => x.User)
                                .Include(x => x.PostTags)
                                    .ThenInclude(x => x.Tag)
                                    .Where(predicate).ToListAsync();

        public override async Task<Post> GetByCriteriaAsync(Expression<Func<Post, bool>> predicate) =>
            await DbSet.Include(x => x.User)
                                    .Include(x => x.PostTags)
                                        .ThenInclude(x => x.Tag)
                                        .FirstOrDefaultAsync(predicate);
    }
}
