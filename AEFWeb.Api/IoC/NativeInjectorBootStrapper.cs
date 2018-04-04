using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Data.Context;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Implementation.Repositories;
using AEFWeb.Implementation.Services;
using AEFWeb.Implementation.UnitOfWork;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AEFWeb.Api.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services, IMapper mapper)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Auto Mapper
            services.AddSingleton(mapper);

            //Database
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DbContext, AEFContext>();

            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<IEventLogService, EventLogService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IFaseService, FaseService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<ISpecialWeekService, SpecialWeekService>();
            services.AddScoped<ILessonService, LessonService>();

            //Repository
            services.AddScoped<IEventLogRepository, EventLogRepository>();
            services.AddScoped<IPostTagRepository, PostTagRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();


            //Notifications
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            services.AddScoped<INotificationHandler<Notification>, NotificationService>();
        }
    }
}
