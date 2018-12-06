// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

namespace Coyote.Execution.CheckCall.JobScheduler
{
    using System;
    using System.Configuration;
    using Autofac;
    using Autofac.Extras.Quartz;
    using Common.Logging;
    using Quartz;
    using Quartz.Spi;
    using Topshelf;
    using Topshelf.Autofac;
    using Topshelf.Quartz;
    using Topshelf.ServiceConfigurators;
    using Coyote.Execution.CheckCall.Domain.Models;

    internal class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));
        private static IContainer _container;

        public static void Main(string[] args)
        {
            try
            {
                _container = ConfigureContainer(new ContainerBuilder()).Build();

                HostFactory.Run(conf =>
                {
                    conf.SetServiceName(typeof(Program).Assembly.GetName().Name);
                    conf.SetDisplayName("Coyote.Execution.CheckCall.DailyCheckCallJobService");
                    conf.UseLog4Net();
                    conf.UseAutofacContainer(_container);

                    conf.Service<DailyCheckCallEmailService>(svc =>
                    {
                        svc.ConstructUsingAutofacContainer();
                        svc.WhenStarted(o => o.Start());
                        svc.WhenStopped(o =>
                        {
                            o.Stop();
                            _container.Dispose();
                        });
                        ConfigureJobs(svc);
                    });

                });

                log4net.LogManager.Shutdown();
            }
            catch (Exception ex)
            {
                _logger.Error("Exception during startup", ex);
                log4net.LogManager.Shutdown();
            }
        }

        private static void ConfigureJobs(ServiceConfigurator<DailyCheckCallEmailService> svc)
        {
            svc.UsingQuartzJobFactory(() => _container.Resolve<IJobFactory>());

            var groupKey = typeof(DailyCheckCallEmailService).Name;

            var dailyCheckCallKey = new JobKey(typeof(DailyCheckCallJob).Name, groupKey);

            var checkCallJobMinute = int.Parse(ConfigurationManager.AppSettings["CheckCallJobMinute"]);
            var checkCallJobHour = int.Parse(ConfigurationManager.AppSettings["CheckCallJobHour"]);

            var priorDayCheckCallJobMinute = int.Parse(ConfigurationManager.AppSettings["PriorDayCheckCallJobMinute"]);
            var priorDayCheckCallJobHour = int.Parse(ConfigurationManager.AppSettings["PriorDayCheckCallJobHour"]);

            svc.ScheduleQuartzJob(q =>
            {
                q.WithJob(JobBuilder.Create<DailyCheckCallJob>()
                    .WithIdentity(dailyCheckCallKey)
                    .Build);

                q.AddTrigger(() => TriggerBuilder.Create()
                    .WithIdentity(CheckCallTrigger.DailyCheckCallTrigger.ToString())
                    .StartAt(DateTimeOffset.Now.AddHours(-1)) // Setting staring time one hour earlier so that it can be triggered immediately with config change and start
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(checkCallJobHour, checkCallJobMinute).WithMisfireHandlingInstructionFireAndProceed())
                    .Build());

                q.AddTrigger(() => TriggerBuilder.Create()
                   .WithIdentity(CheckCallTrigger.PriorDayCheckCallTrigger.ToString())
                   .StartAt(DateTimeOffset.Now.AddHours(-1)) // Setting staring time one hour earlier so that it can be triggered immediately with config change and start
                   .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(priorDayCheckCallJobHour, priorDayCheckCallJobMinute).WithMisfireHandlingInstructionFireAndProceed())
                   .Build());
            });
        }

        private static ContainerBuilder ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new QuartzAutofacFactoryModule());
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(DailyCheckCallJob).Assembly));
            builder.RegisterType<DailyCheckCallEmailService>().AsSelf();
            builder.RegisterModule(new InjectionModule());
            return builder;
        }
    }
}
