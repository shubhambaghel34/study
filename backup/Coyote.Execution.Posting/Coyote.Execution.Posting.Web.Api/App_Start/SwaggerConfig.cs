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
using System.Web.Http;
using WebActivatorEx;
using Coyote.Execution.Posting.Web.Api;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Coyote.Execution.Posting.Web.Api
{
    public static class SwaggerConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Register()
        {

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Coyote.Execution.Posting.Web.Api");
                    c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}\bin\Coyote.Execution.Posting.Web.Api.xml");
                    c.UseFullTypeNameInSchemaIds();
                })
                .EnableSwaggerUi(c => { });
        }
    }
}
