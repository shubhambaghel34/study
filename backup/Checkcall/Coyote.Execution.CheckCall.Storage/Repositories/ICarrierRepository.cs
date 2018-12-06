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

namespace Coyote.Execution.CheckCall.Storage.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Coyote.Execution.CheckCall.Domain.Models;

    public interface ICarrierRepository
    {
        Task<Carrier> GetById(int id);   
        Task<List<CheckCallNotificationRecord>> GetCarrierCheckCallNotificationRecords(bool isLoadDateToday);        
    }
}
