using System;

namespace AEFWeb.Core.ViewModels
{
    public class PostTagViewModel
    {
        public Guid PostId { get; set; }
        public PostViewModel Post { get; set; }

        public Guid TagId { get; set; }
        public TagViewModel Tag { get; set; }
    }
}
