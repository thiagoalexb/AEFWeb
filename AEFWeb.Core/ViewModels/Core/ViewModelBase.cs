using System;

namespace AEFWeb.Core.ViewModels.Core
{
    public class ViewModelBase
    {
        public DateTime? CreationDate { get; set; }
        public Guid? CreatorUserId { get; set; }

        public DateTime? LastUpdateDate { get; set; }
        public Guid? LastUpdatedUserId { get; set; }
    }
}
