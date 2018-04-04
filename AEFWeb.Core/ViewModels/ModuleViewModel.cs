using AEFWeb.Core.ViewModels.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace AEFWeb.Core.ViewModels
{
    public class ModuleViewModel : ViewModelBase
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome pode ter no máximo 100 caracteres")]
        [MinLength(3, ErrorMessage = "Nome precisa conter no mínimo 3 caracteres")]
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid FaseId { get; set; }
        public FaseViewModel Fase { get; set; }
    }
}
