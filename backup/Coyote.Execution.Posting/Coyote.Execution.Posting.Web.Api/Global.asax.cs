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
namespace Coyote.Execution.Posting.Web.Api
{
    using Autofac;
    using Autofac.Integration.WebApi;
    using Coyote.Execution.Posting.Web.Api.AppStart;
    using log4net.Config;
    using NServiceBus;
    using System.Web.Http;

    public class WebApiApplication : System.Web.HttpApplication
    {
        internal const string ServiceName = "Coyote.Execution.Posting.Api";

        private IEndpointInstance _endpointInstance;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //configure logger
            XmlConfigurator.Configure();

            //Setup NServiceBus
            _endpointInstance = NServiceBusconfig.SetupEndpoint();

            //AutoFac
            var autofacBuilder = AutofacConfig.GetAutofacContainerBuilder();
            autofacBuilder.RegisterInstance(_endpointInstance);

            ILifetimeScope container = autofacBuilder.Build();

            // Create the dependency resolver.
            var resolver = new AutofacWebApiDependencyResolver(container);

            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Application_End()
        {
            _endpointInstance?.Stop().GetAwaiter().GetResult();
        }
    }
}