namespace Demo.Web.Api.Controllers.ServiceMonitoring
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Service Monitor Ping
    /// </summary>
    public class PingController : ControllerBase
    {
        /// <summary>
        /// Used by service monitoring to determine if service is running or not.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool Get()
        {
            return true;
        }
    }
}