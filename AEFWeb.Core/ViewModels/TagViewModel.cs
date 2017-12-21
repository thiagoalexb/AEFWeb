using AEFWeb.Core.ViewModels.Core;
using System;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Tag é obrigatório")]
        [MaxLength(100, ErrorMessage = "Tag pode ter no máximo 100 caracteres")]
        [MinLength(3, ErrorMessage = "Tag precisa conter no mínimo 3 caracteres")]
        public string Name { get; set; }
    }
}
