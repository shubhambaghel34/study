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
using Coyote.Execution.CheckCall.Domain.Models;
using System.Threading.Tasks;

namespace Coyote.Execution.CheckCall.Storage.Repositories
{
    public interface ISystemSettingRepository
    {
        Task<SystemSettings> GetBySettingName(string settingName);        
    }
}