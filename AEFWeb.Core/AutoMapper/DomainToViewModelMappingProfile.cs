using AEFWeb.Core.ViewModels;
using AEFWeb.Data.Entities;
using AutoMapper;

namespace AEFWeb.Core.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<User, UserUpdatePasswordViewModel>();
            CreateMap<Tag, TagViewModel>();
            CreateMap<Post, PostViewModel>();
            CreateMap<PostTag, PostTagViewModel>();
            CreateMap<Book, BookViewModel>();
            CreateMap<Event, EventViewModel>();
            CreateMap<Lesson, LessonViewModel>();
            CreateMap<Module, ModuleViewModel>();
            CreateMap<Fase, FaseViewModel>();
            CreateMap<SpecialWeek, SpecialWeekViewModel>();
        }
    }
}
