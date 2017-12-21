using AEFWeb.Core.ViewModels.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace AEFWeb.Core.ViewModels
{
    public class BookViewModel : ViewModelBase
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Editora é obrigatório")]
        [MaxLength(100, ErrorMessage = "Editora pode ter no máximo 100 caracteres")]
        [MinLength(3, ErrorMessage = "Editora precisa conter no mínimo 3 caracteres")]
        public string PublishingCompany { get; set; }

        public string Edition { get; set; }

        [Required(ErrorMessage = "Autor é obrigatório")]
        [MaxLength(100, ErrorMessage = "Autor pode ter no máximo 100 caracteres")]
        [MinLength(3, ErrorMessage = "Autor precisa conter no mínimo 3 caracteres")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [MaxLength(100, ErrorMessage = "Título pode ter no máximo 100 caracteres")]
        [MinLength(3, ErrorMessage = "Título precisa conter no mínimo 3 caracteres")]
        public string Title { get; set; }

        public bool IsSale { get; set; }
        public decimal Value { get; set; }
    }
}
