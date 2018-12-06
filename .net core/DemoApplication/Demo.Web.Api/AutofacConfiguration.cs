
namespace Demo.Web.Api
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Demo.Common.Constants;
    using Demo.Storage.Dapper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;
    using System.Reflection;

    public static class AutofacConfiguration
    {
        public static IContainer ApplicationContainer { get; private set; }

        public static IContainer GetAutofacContainer(IConfiguration Configuration, IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            // ConfigureServices is where you register dependencies. This gets
            // called by the runtime before the Configure method, below.
            // Add services to the collection.

            // Create the container builder.
            // Register dependencies, populate the services from
            // the collection, and build the container.
            // Note that Populate is basically a foreach to add things
            // into Autofac that are in the collection. If you register
            // things in Autofac BEFORE Populate then the stuff in the
            // ServiceCollection can override those things; if you register
            // AFTER Populate those registrations can override things
            // in the ServiceCollection. Mix and match as needed.
            builder.Populate(services);
            //builder.RegisterType<ProductRepository>().As<IProductRepository>().SingleInstance().AsImplementedInterfaces();
            //var value = Configuration.GetValue<string>("testing");

            var connectionString = Configuration.GetConnectionString(StringConstants.CONNECTIONSTRINGNAME);
            builder.RegisterType<DatabaseConnectionTest>().AsSelf();
            builder.RegisterAssemblyTypes(Assembly.Load(StringConstants.STORAGEASSEMBLYNAME))
                   .Where(t => t.Namespace.Contains("Repositories"))
                   .AsImplementedInterfaces()
                   .SingleInstance()
                   .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                   .WithParameter("ConnectionString", connectionString);

            //builder.RegisterType<ProductController>()
            //       .SingleInstance()
            //       .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
            //       .WithParameter("ConnectionString", connectionString);

            ApplicationContainer = builder.Build();

            return ApplicationContainer;
        }
    }
}
