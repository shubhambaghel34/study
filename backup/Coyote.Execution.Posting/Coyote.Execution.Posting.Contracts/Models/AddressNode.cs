// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2018 - 2018
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
    using System;
    using Coyote.Systems.Bazooka.Realtime.Contracts.Tracers;

    public class AddressNode : IAddressNode
    {
        private object _objNode;

        public object NodeAsInterface
        {
            get { return _objNode; }
            set { _objNode = value; }
        }

        private string _strCommID;

        public string CommID
        {
            get { return _strCommID; }
            set { _strCommID = value; }
        }

        public AddressNode()
        {
            _objNode = "Posting";
            _strCommID = Guid.NewGuid().ToString();
        }

    }
}