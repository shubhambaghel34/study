// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2017
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.Posting.Contracts.Models
{
    public class ExternalLoadPostCredential
    {
        #region " Public properties "
        public int InternalEmployeeId { get; set; }
        public int? ITSIntegrationId { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public string ITSLogin { get; set; }
        public string ITSPassword { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public string DATLogin { get; set; }
        public string DATPassword { get; set; }
        public string DATThirdPartyId { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public string PostEverywhereLogin { get; set; }
        public string PostEverywherePassword { get; set; }
        public string PostEverywhereCustomerKey { get; set; }
        public string PostEverywhereServiceKey { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Phonenumber")]
        public string PostEverywherePhonenumber { get; set; }
        #endregion

        #region " Public methods "
        public bool CanPostToITS()
        {
            return !string.IsNullOrEmpty(ITSLogin) && !string.IsNullOrEmpty(ITSPassword) && (ITSIntegrationId != null);
        }

        public bool CanPostToDAT()
        {
            return !string.IsNullOrEmpty(DATLogin) && !string.IsNullOrEmpty(DATPassword);
        }

        public bool CanPostToPostEverywhere()
        {
            return !string.IsNullOrEmpty(PostEverywhereCustomerKey) && !string.IsNullOrEmpty(PostEverywhereServiceKey) && !string.IsNullOrEmpty(PostEverywherePhonenumber);
        } 
        #endregion
    }
}