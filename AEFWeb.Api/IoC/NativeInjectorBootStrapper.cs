using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Core.ViewModels;
using AEFWeb.Data.Context;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Implementation.Repositories;
using AEFWeb.Implementation.Services;
using AEFWeb.Implementation.UnitOfWork;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AEFWeb.Api.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services, IMapper mapper)
        {
            //Auto Mapper
            services.AddSingleton(mapper);

            //Database
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DbContext, AEFContext>();

            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<IEventLogService, EventLogService>();

            //Repository
            services.AddScoped<IEventLogRepository, EventLogRepository>();
            services.AddScoped<IPostTagRepository, PostTagRepository>();


            //Notifications
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            services.AddScoped<INotificationHandler<Notification>, NotificationService>();
        }
    }
}
