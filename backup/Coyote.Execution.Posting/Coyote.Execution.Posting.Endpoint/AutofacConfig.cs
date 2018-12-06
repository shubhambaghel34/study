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
namespace Coyote.Execution.Posting.Endpoint
{
    using Autofac;
    using Coyote.Execution.Posting.Contracts.Managers;
    using Coyote.Execution.Posting.Contracts.ServiceLayer;
    using Coyote.Execution.Posting.Domain.Managers;
    using Coyote.Execution.Posting.ServiceLayer;
    using Coyote.Execution.Posting.ServiceLayer.DAT;
    using Coyote.Execution.Posting.ServiceLayer.InternetTruckStop;
    using Coyote.Execution.Posting.ServiceLayer.PostEverywhere;
    using Coyote.Execution.Posting.ServiceLayer.Realtime;
    using Coyote.Execution.Posting.Storage;
    using log4net.AutoFac;
    using NServiceBus.Logging;
    using System;
    using System.Configuration;

    public static class AutofacConfig
    {
        #region " Constants "
        internal const string ServiceName = "Coyote.Execution.Posting";
        private const string POSTEVERYWHEREURL = "ExternalService.PostEverywhere.Url";
        private const string INTERNETTRUCKSTOPURL = "ExternalService.InternetTruckStop.Url";
        private const string DATABASECONNECTIONSTRING = "Integrated.BazookaDbContext";
        #endregion

        #region " Static methods "
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public static IContainer GetAutofacContainerBuilder()
        {
            var bazooka = ConfigurationManager.ConnectionStrings[DATABASECONNECTIONSTRING]?.ConnectionString;
            var PostEveryWhereUri = new Uri(ConfigurationManager.AppSettings.Get(POSTEVERYWHEREURL));
            var InternetTruckStopUri = new Uri(ConfigurationManager.AppSettings.Get(INTERNETTRUCKSTOPURL));

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(LogManager.GetLogger("Coyote.Execution.Posting"));

            containerBuilder.RegisterModule(new SqlModule(bazooka));
            containerBuilder.RegisterModule<LoggingModule>();

            containerBuilder.RegisterType<PostEverywhereExternalService>().As<IPostEverywhereExternalService>().SingleInstance().WithParameter("uri", PostEveryWhereUri);
            containerBuilder.RegisterType<ExternalService>().As<IExternalService>().SingleInstance();
            containerBuilder.RegisterType<LoadPostManager>().As<ILoadPostManager>().SingleInstance();
            containerBuilder.RegisterType<InternetTruckStopExternalService>().As<IInternetTruckStopExternalService>().SingleInstance().WithParameter("uri", InternetTruckStopUri);
            containerBuilder.RegisterType<DATWrapperService>().As<IDATWrapperService>().SingleInstance();
            containerBuilder.RegisterType<LoadUnpostManager>().As<ILoadUnpostManager>().SingleInstance();
            containerBuilder.RegisterType<RealtimeService>().As<IRealtimeService>().SingleInstance();
            containerBuilder.RegisterType<UpdateMaxPayService>().As<IUpdateMaxPayService>().SingleInstance();

            var container = containerBuilder.Build();
            return container;
        }
        #endregion
    }
}