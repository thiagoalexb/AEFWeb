using AEFWeb.Core.ViewModels.Core;
using System;

namespace AEFWeb.Core.ViewModels
{
    public class TagViewModel : ViewModelBase
    {
        public TagViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
