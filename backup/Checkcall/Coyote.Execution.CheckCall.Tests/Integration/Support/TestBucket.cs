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

namespace Coyote.Execution.CheckCall.Tests.Integration.Support
{
    using System.Collections.ObjectModel;
    using Coyote.Execution.CheckCall.Domain.Models;

    public class TestBucket
    {
        private Collection<Load> _loads;
        private Collection<Carrier> _carriers;
        private Collection<Customer> _customers;
        private Collection<Rep> _reps;

        public TestBucket()
        {
            _loads = new Collection<Load>();
            _carriers = new Collection<Carrier>();
            _customers = new Collection<Customer>();
            _reps = new Collection<Rep>();
        }

        public void Close()
        {
            foreach (var rep in _reps)
            {
                UserManagement.RemoveCarrierRep(rep);
            }
            foreach (var load in _loads)
            {
                LoadManagement.RemoveLoad(load);
            }
            foreach (var carrier in _carriers)
            {
                CarrierManagement.RemoveCarrier(carrier);
            }
            foreach (var customer in _customers)
            {
                CustomerManagement.RemoveCustomer(customer);
            }
        }

        public void TakeOwnership(Load load)
        {
            _loads.Add(load);
        }

        public void TakeOwnership(Carrier carrier)
        {
            _carriers.Add(carrier);
        }

        public void TakeOwnership(Customer customer)
        {
            _customers.Add(customer);
        }

        public void TakeOwnership(Rep rep)
        {
            _reps.Add(rep);
        }

    }
}
