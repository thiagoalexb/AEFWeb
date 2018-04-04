using AEFWeb.Core.ViewModels.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace AEFWeb.Core.ViewModels
{
    public class LessonViewModel : ViewModelBase
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [MaxLength(100, ErrorMessage = "Título pode ter no máximo 100 caracteres")]
        [MinLength(3, ErrorMessage = "Título precisa conter no mínimo 3 caracteres")]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Código é obrigatório")]
        public int Code { get; set; }

        public Guid? ModuleId { get; set; }
        public ModuleViewModel Module { get; set; }

        public Guid? SpecialWeekId { get; set; }
        public SpecialWeekViewModel SpecialWeek { get; set; }
    }
}
