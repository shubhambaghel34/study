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

namespace Coyote.Execution.CheckCall.Services
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using log4net;
    using Newtonsoft.Json;
    using Coyote.Common.Extensions;

    public abstract class ServiceBase
    {
        protected Uri BaseServiceUri { get; private set; }
        protected ILog Logger { get; private set; }

        /// <summary>
        /// C-Tor
        /// </summary>
        /// <param name="baseUriAppSetting">The key to the app setting that will contain the base URi the HTTP Client Should be initialized to</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        protected ServiceBase(ILog logger, string baseUriAppSetting)
        {
            Logger = logger.ThrowIfNull("logger");
            baseUriAppSetting.ThrowIfNull("baseUriAppSetting");

            if (ConfigurationManager.AppSettings.AllKeys.Where(p => p == baseUriAppSetting).Count() != 1)
            {
                throw new ConfigurationErrorsException(string.Format("Configuration App Setting \"{0}\" not Found.", baseUriAppSetting));
            }

            BaseServiceUri = new Uri(ConfigurationManager.AppSettings[baseUriAppSetting]);
        }

        /// <summary>
        /// Post the indicated content to the given endpoint
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="endpoint">uri fragment to be appended to the baseURi to get the full location of the method you are trying to invoke</param>
        /// <param name="content">JSON to post to endpoint</param>
        /// <returns></returns>
        protected TResult PostContentWithResult<TResult>(int auditUserId, string endpoint, string content)
        {
            return ParseResult<TResult>(PostContentStringResult(auditUserId, endpoint, content));
        }

        /// <summary>
        /// Post with no expected Result
        /// </summary>
        /// <param name="endpoint">uri fragment to be appended to the baseURi to get the full location of the method you are trying to invoke</param>
        /// <param name="content">JSON to post to endpoint</param>
        /// <returns></returns>
        protected void PostContent(int auditUserId, string endpoint, string content)
        {
            PostContentStringResult(auditUserId, endpoint, content);
        }

        private string PostContentStringResult(int auditUserId, string endpoint, string content)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseServiceUri;

                using (HttpRequestMessage request = new HttpRequestMessage())
                {
                    request.RequestUri = new Uri(BaseServiceUri.OriginalString + endpoint);
                    request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    AddAuthHeaderToRequest(request, auditUserId);

                    request.Method = HttpMethod.Post;
                    request.Content = new StringContent(content);
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var response = client.SendAsync(request).Result;
                    string returnContent = response.Content.ReadAsStringAsync().Result;
                  
                    response.EnsureSuccessStatusCode();

                    return returnContent;
                }
            }
        }

        /// <summary>
        /// Conduct Http Get on the endpoint and return result;
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="endpoint">uri fragment to be appended to the baseURi to get the full location of the method you are trying to invoke</param>
        /// <returns></returns>
        protected TResult Get<TResult>(int auditUserId, string endpoint)
        {
            return ParseResult<TResult>(GetRaw(auditUserId, endpoint));
        }

        protected string GetRaw(int auditUserId, string endpoint)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseServiceUri;

                using (HttpRequestMessage request = new HttpRequestMessage())
                {
                    request.RequestUri = new Uri(BaseServiceUri.OriginalString + endpoint);
                    request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    AddAuthHeaderToRequest(request, auditUserId);

                    request.Method = HttpMethod.Get;

                    var response = client.SendAsync(request).Result;

                    string content = response.Content.ReadAsStringAsync().Result;

                    response.EnsureSuccessStatusCode();

                    return content;
                }
            }
        }

        private TResult ParseResult<TResult>(string content)
        {
            return JsonConvert.DeserializeObject<TResult>(content);
        }

        private static void AddAuthHeaderToRequest(HttpRequestMessage request, int auditUserId)
        {
            request.Headers.Add("CoyoteAuthorization", auditUserId.ToString());
        }

    }
}
