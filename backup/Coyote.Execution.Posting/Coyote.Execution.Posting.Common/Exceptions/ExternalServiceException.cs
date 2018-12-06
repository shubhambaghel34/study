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
namespace Coyote.Execution.Posting.Common.Exceptions
{
    using global::Coyote.Execution.Posting.Common.Coyote.Types;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Specialized constructor for the extra properties")]
    public class ExternalServiceException : Exception
    {
        #region " Privat fields "
        private readonly ExternalService _service; 
        #endregion

        #region " Constructor "
        public ExternalServiceException(string message, ExternalService service)
            : base(message)
        {
            _service = service;
        }

        public ExternalServiceException()
        {
        }
        #endregion

        #region " Public properties "
        public ExternalService Service
        {
            get { return _service; }
        }

        public override string Message
        {
            get
            {
                return string.Format("{0}: {1}", _service.ToString(), base.Message);
            }
        }

        public string Error
        {
            get
            {
                return base.Message;
            }
        }
        #endregion

        #region " Public methods "
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Service", (int)_service);
        } 
        #endregion
    }
}