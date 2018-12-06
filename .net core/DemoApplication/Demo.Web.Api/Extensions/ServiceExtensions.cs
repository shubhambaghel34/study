using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Demo.Web.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin() //WithOrigins("http://www.something.com")
                    .AllowAnyMethod()                   //WithMethods("POST", "GET")
                    .AllowAnyHeader()                   //WithHeaders("accept", "content-type")
                    .AllowCredentials());
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info()
                {
                    Title = "Demo.Web.Api",
                    Version = "v1",
                    Description = "Demo We.Api in .net core",
                    Contact = new Contact
                    {
                        Name = "Shailendra Patil",
                        Email = "spatil2@xpanxion.com",
                        //Url = "https://twitter.com/spboyer"
                    },
                });
            });
        }
    }
}
