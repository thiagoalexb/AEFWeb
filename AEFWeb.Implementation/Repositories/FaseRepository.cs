using AEFWeb.Core.Repositories;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Repositories.Core;
using Microsoft.EntityFrameworkCore;

namespace AEFWeb.Implementation.Repositories
{
    public class FaseRepository : Repository<Fase>, IFaseRepository
    {
        public FaseRepository(DbContext context) : base(context)
        { }
    }
}
