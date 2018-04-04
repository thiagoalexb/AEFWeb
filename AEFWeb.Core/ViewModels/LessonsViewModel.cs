using AEFWeb.Core.ViewModels.Core;
using System;
using System.Collections.Generic;

namespace AEFWeb.Core.ViewModels
{
    public class LessonsViewModel : ViewModelBase
    {
        public Guid? ModuleId { get; set; }
        public ModuleViewModel Module { get; set; }

        public Guid? SpecialWeekId { get; set; }
        public SpecialWeekViewModel SpecialWeek { get; set; }

        public List<LessonViewModel> Lessons { get; set; }
    }
}
