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

namespace Coyote.Execution.CheckCall.Email.Endpoint.Email
{
    using Coyote.Common.Extensions;
    using Coyote.Execution.CheckCall.Contracts.Commands;
    using Coyote.Execution.CheckCall.Domain;
    using Coyote.Execution.CheckCall.Domain.Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Dynamic;
    using System.Linq;
    using System.Net.Mime;

    public class DailyCheckCall : EmailMessage
    {
        private readonly Carrier _carrier;
        readonly SendDailyCheckCallEmail _command;
        private readonly Rep _mainRep;

        private readonly string _baseLinkUrl;

        public override Dictionary<string, string> To => GetAddressCollection(_command.Email?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Where(p => !string.IsNullOrWhiteSpace(p)));

        public override Dictionary<string, string> CC => GetAddressCollection(_carrier.Reps.Select(s => s.EmailWork));

        public override string From => _mainRep?.EmailWork;

        public override string Subject => $"Dispatch and Confirm Coyote Loads for {_command.Date.ToString("MM/dd/yy")}";

        public override string TemplateName => "DailyCheckCallEmailTemplate";

        public override Dictionary<string, string> LinkedResourceContext => new Dictionary<string, string>()
        {
            { "logoCoyoteHorizontalBrandMark", MediaTypeNames.Image.Gif }
        };

        public DailyCheckCall(Carrier carrier, SendDailyCheckCallEmail command)
        {
            _carrier = carrier.ThrowIfNull(nameof(carrier));
            _command = command.ThrowIfNull(nameof(command));

            _baseLinkUrl = ConfigurationManager.AppSettings["CoyoteComBaseURL"];

            _mainRep = _carrier.Reps.FirstOrDefault(s => s.Main == true);
        }

        #region Private Function

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected override dynamic GetEmailData()
        {
            dynamic templateData = new ExpandoObject();
            bool IsPickupToday = false;

            if (_command.Date.Date.CompareTo(DateTime.Now.Date) == 0)
                IsPickupToday = true;

            templateData.card = new ExpandoObject();
            templateData.card.header = new ExpandoObject();
            templateData.card.header.title = $"Dispatch and Confirm Coyote Loads for {_command.Date.ToString("MM/dd")}";
            templateData.card.main = new ExpandoObject();
            templateData.card.main.title = "Hello!";
            templateData.card.main.text = $"Please see your schedule of Coyote loads for {_command.Date.ToString("MM/dd")}. Please contact your Coyote Carrier Rep immediately if you no longer have the capacity for any of the following loads.";
            templateData.card.loads = new ExpandoObject();
            templateData.card.loads.title = "Loads";
            templateData.card.loads.collection = _command.Loads;
            templateData.card.links = new ExpandoObject();
            templateData.card.links.confirm = new ExpandoObject();
            templateData.card.links.confirm.url = $"{_baseLinkUrl}/my-tasks#tracking/?pickupToday={IsPickupToday.ToString().ToLower()}&showOnlyCoveredLoads=true";
            templateData.card.links.confirm.text = "CLICK HERE TO DISPATCH OR ADD DRIVER INFORMATION";

            templateData.signature = new ExpandoObject();
            templateData.signature.valediction = "Thanks,";
            templateData.signature.firstName = _mainRep.FirstName;
            templateData.signature.lastName = _mainRep.LastName;
            templateData.signature.companyTitle = "COYOTE";
            templateData.signature.companySubtitle = "A UPS Company";
            templateData.signature.workPhoneNumberLabel = "T: ";
            templateData.signature.workPhoneNumber = PhoneNumberBuilder.CreatePhoneNumberModel(_mainRep.OfficePhoneNumber).Formatted;
            templateData.signature.mobilePhoneNumberLabel = "M: ";
            templateData.signature.mobilePhoneNumber = PhoneNumberBuilder.CreatePhoneNumberModel(_mainRep.MobilePhoneNumber).Formatted;
            templateData.signature.faxPhoneNumberLabel = "F: ";
            templateData.signature.faxPhoneNumber = PhoneNumberBuilder.CreatePhoneNumberModel(_mainRep.FaxNumber).Formatted;
            templateData.signature.workEmailLabel = "E: ";
            templateData.signature.workEmail = _mainRep.EmailWork;

            var addr = _mainRep.Addresses.FirstOrDefault(); // office address for Employee EntityType
            if (addr != null)
            {
                templateData.signature.address = string.IsNullOrWhiteSpace(addr.Address2) ? $"{addr.Address1}, {addr.CityName}, {addr.ZipCode}" : $"{addr.Address1}, {addr.Address2}, {addr.CityName}, {addr.ZipCode}";
            }
            templateData.signature.companyLinkUrl = "http://www.coyote.com/";
            templateData.signature.companyLinkText = "www.coyote.com";

            return templateData;
        }

        #endregion

        #region Static Functions

        private static Dictionary<string, string> GetAddressCollection(IEnumerable<string> addr)
        {
            var result = new Dictionary<string, string>();

            foreach (var add in addr)
            {
                var address = add.Trim();
                if (!result.Keys.Contains(address))
                {
                    result.Add(address, address);
                }
            }

            return result;
        }

        #endregion
    }
}
