using AEFWeb.Core.ViewModels.Core;
using System;
using System.Collections.Generic;

namespace AEFWeb.Core.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public List<LessonViewModel> Lessons { get; set; }
    }
}
