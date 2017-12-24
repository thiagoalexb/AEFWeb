using AEFWeb.Api.Extensions;
using AEFWeb.Api.IoC;
using AEFWeb.Api.Security;
using AEFWeb.Api.Swagger;
using AEFWeb.Core.AutoMapper;
using AEFWeb.Implementation.EmailSettings;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace AEFWeb.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => 
            Configuration = configuration;
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(c =>  { ConfigServiceSwagger(c); });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(Configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
                
                paramsValidation.ValidateIssuerSigningKey = true;
                
                paramsValidation.ValidateLifetime = true;
                
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            //AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });

            services.AddMediatR(typeof(Startup));

            RegisterServices(services, config.CreateMapper());

            services.Configure<EmailSetting>(Configuration.GetSection("EmailSettings"));
            services.Configure<BaseUrl>(Configuration.GetSection("BaseUrl"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseErrorHandlingMiddleware();

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c => { ConfigUISwagger(c); });
        }

        private static void RegisterServices(IServiceCollection services, IMapper mapper) => 
            NativeInjectorBootStrapper.RegisterServices(services, mapper);

        private static SwaggerGenOptions ConfigServiceSwagger(SwaggerGenOptions c) => 
            SwaggerConfig.ConfigServiceSwagger(c);

        private static SwaggerUIOptions ConfigUISwagger(SwaggerUIOptions c) =>
            SwaggerConfig.ConfigUISwagger(c);
    }
}
