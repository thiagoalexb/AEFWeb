using AEFWeb.Core.ViewModels;
using AEFWeb.Data.Entities;
using AutoMapper;

namespace AEFWeb.Core.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UserViewModel, User>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<TagViewModel, Tag>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<PostViewModel, Post>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<PostTagViewModel, PostTag>();
            CreateMap<BookViewModel, Book>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<EventViewModel, Event>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<LessonViewModel, Lesson>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
