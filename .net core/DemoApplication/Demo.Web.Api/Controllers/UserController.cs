namespace Demo.Web.Api.Controllers
{
    using Demo.Common.Extensions;
    using Demo.Contract.Interfaces.Services;
    using Demo.Contract.Interfaces.Storage.Repositories;
    using Demo.Contract.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("CorsPolicy")]
    public class UserController : ControllerBase
    {
        private ILogger _logger { get; set; }

        private IRepositoryFactory _repositoryFactory { get; set; }

        public UserController(ILogger logger, IRepositoryFactory repositoryFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repositoryFactory = repositoryFactory.ThrowIfArgumentNull(nameof(repositoryFactory));
        }

        [HttpGet]
        [Route("getallusersasync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<User>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = null)]
        public async Task<IActionResult> GetALLUsersAsync()
        {
            try
            {
                _logger.LogInfo("GetALLUsersAsync()");
                return Ok(await _repositoryFactory.UserRepository.GetALLUsersAsync());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }            
        }

        [HttpGet]
        [Route("getuserbycodeasync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = null)]
        public async Task<ActionResult<User>> GetUserByCodeAsync(string Code)
        {
            try
            {
                _logger.LogInfo("GetUserByCodeAsync()");
                return await _repositoryFactory.UserRepository.GetUserByCodeAsync(Code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("adduserasync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = null)]
        public async Task<ActionResult<int>> AddUserAsync([FromBody] User User)
        {
            try
            {
                User.ThrowIfArgumentNull(nameof(User));
                User.Code.ThrowIfArgumentNull(nameof(User.Code));
                _logger.LogInfo("AddUserAsync()");
                return await _repositoryFactory.UserRepository.AddUserAsync(User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}