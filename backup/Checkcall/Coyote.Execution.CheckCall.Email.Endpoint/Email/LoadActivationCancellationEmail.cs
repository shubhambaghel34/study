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
namespace Coyote.Execution.CheckCall.Email.Endpoint.Email
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using Coyote.Common.Extensions;
    using Coyote.Execution.CheckCall.Contracts.Commands;
    using Coyote.Execution.CheckCall.Domain;
    using Coyote.Systems.Email.Contracts;
    using DomainModels = Coyote.Execution.CheckCall.Domain.Models;

    public class LoadActivationCancellationEmail : EmailMessage
    {

        private LoadActivationCancellationEmailCommand _load;

        private string _emailText;
        public override Dictionary<string, string> To => GetAddressCollection(_load.CarrierRepEmailAddresses);

        public override Dictionary<string, string> CC => null;
        public override string From => _load.DoNotReplyFromAddress;

        public override string Subject => _emailText;

        public override string TemplateName => "LoadActivationCancellationEmailTemplate";

        public override EmailType EmailType => EmailType.LoadActivationCancellation;

        public LoadActivationCancellationEmail(LoadActivationCancellationEmailCommand load)
        {
            _load = load.ThrowIfNull("load");
            string strCancelledOrActivated = _load.LoadState == DomainModels.LoadStateType.Canceled ? "cancelled" : "activated";
            bool showCountryCode = (_load.Division == DomainModels.Division.EU) && (_load.Mode == DomainModels.LoadModeType.TL || _load.Mode == DomainModels.LoadModeType.LTL);

            string originLocation = $"{_load.OriginCity}, {_load.OriginStateCode}{(showCountryCode ? $", {_load.OriginCountryCode}" : "")}";
            string destinationLocation = $"{_load.DestinationCity}, {_load.DestinationStateCode}{(showCountryCode ? $", {_load.DestinationCountryCode}" : "")}";
            string loadDate = GetLoadDate(_load.Division, _load.LoadDate);

            _emailText = $"LD{_load.LoadId} {_load.CustomerName} {loadDate} - {originLocation} to {destinationLocation} has been {strCancelledOrActivated}.";
        }

        #region Private Function    
        protected override dynamic GetEmailData()
        {
            dynamic templateData = new ExpandoObject();
            templateData.body = _emailText;
            return templateData;
        }
        #endregion

        #region Static Functions
        private static Dictionary<string, string> GetAddressCollection(IEnumerable<string> addr)
        {
            var result = new Dictionary<string, string>();

            foreach (var add in addr)
            {
                if (!result.Keys.Contains(add))
                {
                    result.Add(add, add);
                }
            }
            return result;
        }

        private static string GetLoadDate(DomainModels.Division division, DateTime loadDate)
        {
            if(division == DomainModels.Division.EU)
            {
                return loadDate.Date.ToString("dd/MM/yyyy");
            }
            else
            {
                return loadDate.Date.ToString("MM/dd/yyyy");
            }
        }
        #endregion
    }
}
