using AEFWeb.Core.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AEFWeb.Core.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "A data é obrigatória")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime Date { get; set; }

        [Required]
        public List<LessonViewModel> Lessons { get; set; }
    }
}
