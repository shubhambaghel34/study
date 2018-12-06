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
namespace Coyote.Execution.Posting.ServiceLayer
{
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts.Storage;
    using log4net;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class ExternalServiceBase
    {
        #region Fields
        protected IPostingRepository PostingRepository { get; set; }
        protected Uri Uri { get; set; }
        protected Common.Coyote.Types.ExternalService ExternalService { get; private set; }
        protected ILog Log { get; private set; }
        #endregion

        #region Constructor
        public ExternalServiceBase(IPostingRepository postingRepository, Uri uri, Common.Coyote.Types.ExternalService externalService, ILog log)
        {
            uri.ThrowIfArgumentNull(nameof(uri));
            if (!uri.IsAbsoluteUri)
            {
                throw new ArgumentException("Invalid URL", nameof(uri));
            }
            Uri = uri;
            ExternalService = externalService;
            Log = log.ThrowIfArgumentNull(nameof(log));
            PostingRepository = postingRepository.ThrowIfArgumentNull(nameof(postingRepository));
        }
        #endregion

        #region " public methods "
        public async Task<bool> PingAsync()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(Uri);
                return result.IsSuccessStatusCode;
            }
        }

        public virtual async Task<HttpResponseMessage> PostContentAsync(string content, string controllerName, int userId = 0)
        {
            if (string.IsNullOrEmpty(controllerName)) return null;

            string contentType = "application/json";

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.RequestUri = new Uri($"{Uri.AbsoluteUri}{controllerName}");
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                    request.Headers.Add("CoyoteAuthorization", userId.ToString());
                    request.Method = HttpMethod.Post;
                    request.Content = new StringContent(content);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                    return await client.SendAsync(request).ConfigureAwait(false);
                }
            }
        }
        #endregion
    }
}