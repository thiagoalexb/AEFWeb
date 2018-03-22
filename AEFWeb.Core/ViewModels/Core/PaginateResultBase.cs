using System;
using System.Collections.Generic;
using System.Text;

namespace AEFWeb.Core.ViewModels.Core
{
    public class PaginateResultBase<TEntity>
    {
        public List<TEntity> Results { get; set; }
        public int Total { get; set; }
    }
}
