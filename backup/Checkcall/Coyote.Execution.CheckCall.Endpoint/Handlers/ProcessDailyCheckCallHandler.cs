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

namespace Coyote.Execution.CheckCall.Endpoint.Processors
{
    using Coyote.Common.Extensions;
    using Coyote.Execution.CheckCall.Contracts;
    using Coyote.Execution.CheckCall.Contracts.Commands;
    using Coyote.Execution.CheckCall.Domain.Models;
    using log4net;
    using NServiceBus;
    using Storage.Repositories;
    using System;
    using System.Collections.ObjectModel;
    using System.Data.SqlTypes;
    using System.Linq;

    public class ProcessDailyCheckCallHandler : IHandleMessages<ProcessDailyCheckCallEmails>, IHandleMessages<ProcessPriorDayCheckCallEmails>
    {
        private ILog Log { get; set; }
        private IBus Bus { get; set; }
        private ICarrierRepository CarrierRepository { get; set; }

        public ProcessDailyCheckCallHandler(ILog log, IBus bus, ICarrierRepository carrierRepository)
        {
            Log = log.ThrowIfNull("log");
            Bus = bus.ThrowIfNull("bus");
            CarrierRepository = carrierRepository.ThrowIfNull("carrierRepository");
        }

        public void Handle(ProcessDailyCheckCallEmails message)
        {
            if (message == null) { throw new ArgumentNullException("message"); }
            Log.InfoFormat("ProcessDailyCheckCallHandler - Processing Command to Begin Generating Daily Check Call Emails");

            SendDailyCheckCallEmails(true);
        }

        public void Handle(ProcessPriorDayCheckCallEmails message)
        {
            if (message == null) { throw new ArgumentNullException("message"); }
            Log.InfoFormat("ProcessDailyCheckCallHandler - Processing Command to Begin Generating Prior Day Check Call Emails");

            SendDailyCheckCallEmails(false);
        }

        private void SendDailyCheckCallEmails(bool isLoadDateToday)
        {
            var notificationLoadRecords = CarrierRepository.GetCarrierCheckCallNotificationRecords(isLoadDateToday).Result;

            int notificationCount = 0;
            if (notificationLoadRecords.Count > 0)
            {
                int currentCarrierId = notificationLoadRecords[0].CarrierId;
                Collection<LoadInfo> loads = new Collection<LoadInfo>();

                for (int i = 0; i < notificationLoadRecords.Count + 1; i++)
                {
                    if (i < notificationLoadRecords.Count && notificationLoadRecords[i].CarrierId != currentCarrierId)
                    {
                        Bus.Send(GetCheckCallEmail(notificationLoadRecords[i - 1], loads));
                        Log.InfoFormat("ProcessDailyCheckCallHandler - Send Check Call Command Issued for CarrierId : {0}", notificationLoadRecords[i - 1].CarrierId);
                        notificationCount++;
                        currentCarrierId = notificationLoadRecords[i].CarrierId;
                        loads = new Collection<LoadInfo>();
                    }
                    else
                    {
                        if (i == notificationLoadRecords.Count)
                        {

                            Bus.Send(GetCheckCallEmail(notificationLoadRecords[i - 1], loads));
                            Log.InfoFormat("ProcessDailyCheckCallHandler - Send Check Call Command Issued for CarrierId : {0}", notificationLoadRecords[i - 1].CarrierId);
                            notificationCount++;
                        }
                    }
                    if (i < notificationLoadRecords.Count)
                    {
                        loads.AddLoad(notificationLoadRecords[i]);
                        SetPickUpTime(loads.Last(), notificationLoadRecords[i]);
                    }
                }
            }
            Log.InfoFormat("ProcessDailyCheckCallHandler - {0} Check Call Notification Commands Sent.", notificationCount);
        }

        private static void SetPickUpTime(LoadInfo load, CheckCallNotificationRecord record)
        {
            load.PickUp = "Appointment Pending";

            if (record.ScheduleOpenTime.HasValue && record.ScheduleOpenTime != (DateTime)SqlDateTime.MinValue)
            {
                load.PickUp = ((DateTime)record.ScheduleOpenTime).ToString("HH:mm");

                if (!load.PickUp.Equals(((DateTime)record.ScheduleCloseTime).ToString("HH:mm")))
                    load.PickUp += $" - {((DateTime)record.ScheduleCloseTime).ToString("HH:mm")}";
            }
        }

        private static SendDailyCheckCallEmail GetCheckCallEmail(CheckCallNotificationRecord record, Collection<LoadInfo> loads)
        {
            var email = new SendDailyCheckCallEmail(record, loads);
            if (loads == null || loads.Any(load => load.LoadId != record.LoadId))
            {
                SetPickUpTime(email.Loads.Last(), record);
            }
            return email;
        }
    }

    internal static class Extensions
    {
        internal static void AddLoad(this Collection<LoadInfo> loads, CheckCallNotificationRecord record)
        {
            loads.Add(new LoadInfo
            {
                LoadId = record.LoadId,
                OriginCityName = record.OriginCityName,
                OriginState = record.OriginState,
                DestinationCityName = record.DestCityName,
                DestinationState = record.DestState
            });
        }
    }
}
