using AEFWeb.Core.ViewModels.Core;
using System;

namespace AEFWeb.Core.ViewModels
{
    public class BookViewModel : ViewModelBase
    {
        public Guid Id { get; set; }
        public string PublishingCompany { get; set; }
        public string Edition { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public bool IsSale { get; set; }
        public decimal Value { get; set; }
    }
}
