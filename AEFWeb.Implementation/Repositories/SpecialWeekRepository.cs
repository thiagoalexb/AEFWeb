using AEFWeb.Core.Repositories;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Repositories.Core;
using Microsoft.EntityFrameworkCore;

namespace AEFWeb.Implementation.Repositories
{
    public class SpecialWeekRepository : Repository<SpecialWeek>, ISpecialWeekRepository
    {
        public SpecialWeekRepository(DbContext context) : base(context)
        { }
    }
}
