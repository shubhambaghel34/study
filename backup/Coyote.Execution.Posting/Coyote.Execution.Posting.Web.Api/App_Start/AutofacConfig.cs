// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.Posting.Web.Api.AppStart
{
    using Autofac;
    using Autofac.Integration.WebApi;
    using Coyote.Execution.Posting.Storage;
    using log4net;
    using System.Configuration;
    using System.Reflection;

    public static class AutofacConfig
    {
        /// <summary>
        /// Get an initial container builder (container must be built and must still be set as the dependency resolver for controllers)
        /// </summary>
        /// <returns></returns>
        public static ContainerBuilder GetAutofacContainerBuilder()
        {
            var bazooka = ConfigurationManager.ConnectionStrings["Integrated.BazookaDbContext"]?.ConnectionString;
            var builder = new ContainerBuilder();

            // Register the Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterInstance<ILog>(LogManager.GetLogger(WebApiApplication.ServiceName));
            builder.RegisterModule(new SqlModule(bazooka));

            return builder;
        }
    }
}