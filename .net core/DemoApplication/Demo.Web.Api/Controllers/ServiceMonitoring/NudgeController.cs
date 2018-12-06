namespace Demo.Web.Api.Controllers.ServiceMonitoring
{
    using Demo.Contract.Interfaces.Services;
    using Demo.Storage.Dapper;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Service Monitor Nudge
    /// </summary>
    [Route("api/Nudge")]
    [ApiController]
    public class NudgeController : ControllerBase
    {
        private DatabaseConnectionTest _database { get; set; }
        private ILogger _logger { get; set; }

        public NudgeController(DatabaseConnectionTest Database, ILogger Logger)
        {
            _database = Database ?? throw new ArgumentNullException(nameof(Database));
            _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
        }

        /// <summary>
        /// Used by service monitoring to determine if service is running and connected to the database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> Get()
        {
            try
            {
                _logger.LogInfo("Nudge hit.");
                await _database.TryConnect();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                return false;
            }
        }
    }
}