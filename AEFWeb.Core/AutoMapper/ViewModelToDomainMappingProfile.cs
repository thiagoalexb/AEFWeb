using AEFWeb.Core.ViewModels;
using AEFWeb.Data.Entities;
using AutoMapper;

namespace AEFWeb.Core.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UserViewModel, User>();
            CreateMap<TagViewModel, Tag>();
            CreateMap<PostViewModel, Post>();
            CreateMap<PostTagViewModel, PostTag>();
            CreateMap<BookViewModel, Book>();
            CreateMap<EventViewModel, Event>();
            CreateMap<LessonViewModel, Lesson>();
        }
    }
}
