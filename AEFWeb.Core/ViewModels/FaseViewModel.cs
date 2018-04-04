using AEFWeb.Core.ViewModels.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace AEFWeb.Core.ViewModels
{
    public class FaseViewModel : ViewModelBase
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "Título pode ter no máximo 100 caracteres")]
        [MinLength(3, ErrorMessage = "Título precisa conter no mínimo 3 caracteres")]
        public string Name { get; set; }
    }
}
