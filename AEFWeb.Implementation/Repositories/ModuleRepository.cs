using AEFWeb.Core.Repositories;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Repositories.Core;
using Microsoft.EntityFrameworkCore;

namespace AEFWeb.Implementation.Repositories
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        public ModuleRepository(DbContext context) : base(context)
        { }
    }
}
