using System;
using System.ComponentModel.DataAnnotations;

namespace AEFWeb.Core.ViewModels
{
    public class LessonViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O horário é obrigatória")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime Schedule { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [MaxLength(100, ErrorMessage = "Título pode ter no máximo 100 caracteres")]
        [MinLength(3, ErrorMessage = "Título precisa conter no mínimo 3 caracteres")]
        public string Title { get; set; }

        public string SubTitle { get; set; }
        public string Description { get; set; }

        public Guid EventId { get; set; }
    }
}
