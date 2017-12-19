using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AEFWeb.Api.Swagger
{
    public class SwaggerConfig
    {
        public static SwaggerGenOptions ConfigServiceSwagger(SwaggerGenOptions c)
        {
            c.SwaggerDoc("v1", new Info
            {
                Version = "v1",
                Title = "AEF Api Project",
                Description = "AEF Api",
                Contact = new Contact { Name = "Thiago Alex", Email = "thiago.alexb@gmail.com" },
                License = new License { Name = "GIT", Url = "git" }
            });
            c.AddSecurityDefinition("Bearer",
                new ApiKeyScheme()
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });
            return c;
        }

        public static SwaggerUIOptions ConfigUISwagger(SwaggerUIOptions c)
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "AEF Api V1");
            return c;
        }
    }
}
