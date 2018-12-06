namespace Demo.Web.Api.Extensions
{
    using Demo.Contract.Interfaces.Services;
    using Demo.Services.Logger;
    using Microsoft.Extensions.DependencyInjection;

    public static class RegisterDependencies
    {
        public static void RegisterLogger(this IServiceCollection services)
        {
            //services.AddLogging()
            services.AddSingleton<ILogger, Logger>();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            //services.AddTransient()
            //services.AddSingleton<IProductRepository, ProductRepository>();
        }

    }
}
