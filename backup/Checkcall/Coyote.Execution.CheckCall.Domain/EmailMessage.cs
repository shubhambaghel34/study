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

namespace Coyote.Execution.CheckCall.Domain
{
    using Coyote.Systems.Email.Contracts;
    using System.Collections.Generic;

    public abstract class EmailMessage
    {
        /// <summary>
        /// Gets To Addresses for Email Message.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "To", Justification = "Email system has property named To")]
        public virtual Dictionary<string, string> To { get; }

        /// <summary>
        /// Gets CC Addresses for Email Message.
        /// </summary>
        public virtual Dictionary<string, string> CC { get; }

        /// <summary>
        /// Gets Body for Email Message.
        /// </summary>
        public virtual string Body => string.Empty;

        /// <summary>
        /// Gets BodyEncoding for Email Message.
        /// </summary>
        public virtual string BodyEncoding => System.Text.Encoding.UTF8.WebName;

        /// <summary>
        /// Gets From for Email Message.
        /// </summary>
        public virtual string From { get; }

        /// <summary>
        /// Gets if Email is HTML.
        /// </summary>
        public virtual bool IsBodyHtml => true;

        /// <summary>
        /// Gets Template name for Email Message.
        /// </summary>
        public virtual string TemplateName { get; }

        /// <summary>
        /// Gets Template data for Email Message.
        /// </summary>
        public dynamic TemplateData => GetEmailData();

        /// <summary>
        /// Gets Subject for Email Message.
        /// </summary>
        public virtual string Subject { get; }

        /// <summary>
        /// Gets Subject Encoding.
        /// </summary>
        public virtual string SubjectEncoding => System.Text.Encoding.UTF8.WebName;

        /// <summary>
        /// Get Linked Resource Context.
        /// </summary>
        public virtual Dictionary<string, string> LinkedResourceContext { get; }

        protected abstract dynamic GetEmailData();

        public virtual EmailType EmailType { get; }
    }
}
