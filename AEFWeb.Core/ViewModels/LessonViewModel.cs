using System;

namespace AEFWeb.Core.ViewModels
{
    public class LessonViewModel
    {
        public Guid Id { get; set; }
        public DateTime Schedule { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }

        public Guid EventId { get; set; }
        public EventViewModel Event { get; set; }
    }
}
