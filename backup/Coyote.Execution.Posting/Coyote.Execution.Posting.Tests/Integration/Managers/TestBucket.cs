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
namespace Coyote.Execution.Posting.Tests.Integration.Managers
{
    using System.Collections.Generic;

    public class TestBucket
    {
        #region " Private Fields"
        private Dictionary<int, int> _externalLoadPostIds = null;
        private string _connectionString;
        #endregion

        #region " Public Methods "
        public TestBucket(string connectionString)
        {
            _externalLoadPostIds = new Dictionary<int, int>();
            _connectionString = connectionString;
        }

        public void Close()
        {
            foreach (var pair in _externalLoadPostIds)
            {
               ExternalLoadPostRepositoryManager.RemoveExternalLoadPost(pair.Key, pair.Value, _connectionString);
            }
        }

        public void TakeExternalLoadPostOwnership(int newExternalLoadPostId, int oldExternalLoadPostId)
        {
            _externalLoadPostIds.Add(newExternalLoadPostId, oldExternalLoadPostId);
        }
        #endregion
    }
}