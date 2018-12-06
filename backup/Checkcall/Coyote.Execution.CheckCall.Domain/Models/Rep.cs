// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2017
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

namespace Coyote.Execution.CheckCall.Domain.Models
{

    using System.Collections.Generic;
    public class Rep
    {
        public int Id { get; set; }
        public RepType? RepType { get; set; }
        public int EmployeeId { get; set; }        
        public bool? Main { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string EmailWork { get; set; }
        public string FaxNumber { get; set; }
        public ICollection<Address> Addresses { get; set; }

    }
}
