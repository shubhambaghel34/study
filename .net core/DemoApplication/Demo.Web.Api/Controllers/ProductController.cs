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
    public class ProductController : ControllerBase
    {
        private ILogger _logger { get; set; }

        //private IProductRepository productRepository;

        //public IProductRepository ProductRepository
        //{
        //    get { return productRepository; }
        //    set { productRepository = value; }
        //}


        private IRepositoryFactory _repositoryFactory { get; set; }

        public ProductController(ILogger logger, IRepositoryFactory repositoryFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repositoryFactory = repositoryFactory.ThrowIfArgumentNull(nameof(repositoryFactory));
        }

        [HttpGet]
        [Route("getallproductsasync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = null)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInfo("GetAllProductsAsync()");
                return Ok(await _repositoryFactory.ProductRepository.GetAllProductsAsync());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }            
        }

        [HttpGet]
        [Route("getproductbyidasync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = null)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> GetProductByIdAsync(int ProductId)
        {
            try
            {
                _logger.LogInfo("GetProductById()");
                return await _repositoryFactory.ProductRepository.GetProductByIdAsync(ProductId);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            //return await Task.Run(() => _productRepository.GetProductByIdAsync(1).Result);
        }

        [HttpGet]
        [Route("getproductbycodeasync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = null)]
        public async Task<ActionResult<Product>> GetProductByCodeAsync(string Code)
        {
            try
            {
                _logger.LogInfo("GetProductByCodeAsync()");
                 return await _repositoryFactory.ProductRepository.GetProductByCodeAsync(Code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("addproductasync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = null)]
        public async Task<ActionResult<int>> AddProductAsync([FromBody] Product Product)
        {
            try
            {
                Product.ThrowIfArgumentNull(nameof(Product));
                Product.Code.ThrowIfArgumentNull(nameof(Product.Code));
                _logger.LogInfo("AddProductAsync()");
                return await _repositoryFactory.ProductRepository.AddProductAsync(Product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut]
        [Route("updateproductbycodeasync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = null)]
        public async Task<ActionResult<int>> UpdateProductByCodeAsync([FromBody] Product Product)
        {
            try
            {
                Product.ThrowIfArgumentNull(nameof(Product));
                Product.Code.ThrowIfArgumentNull(nameof(Product.Code));
                _logger.LogInfo("UpdateProductByCodeAsync()");
                return await _repositoryFactory.ProductRepository.UpdateProductByCodeAsync(Product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpDelete]
        [Route("deleteproductbycodeasync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = null)]
        public async Task<ActionResult<int>> DeleteProductByCodeAsync(string Code)
        {
            try
            {
                Code.ThrowIfArgumentNull(nameof(Code));
                _logger.LogInfo("DeleteProductByCodeAsync()");
                return await _repositoryFactory.ProductRepository.DeleteProductByCodeAsync(Code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpDelete]
        [Route("deleteproductbyidasync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = null)]
        public async Task<ActionResult<int>> DeleteProductByIdAsync(int ProductId)
        {
            try
            {
                _logger.LogInfo($"DeleteProductByCodeAsync(){ProductId}");
                return await _repositoryFactory.ProductRepository.DeleteProductByIdAsync(ProductId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut]
        [Route("updateproductbyidasync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = null)]
        public async Task<ActionResult<int>> UpdateProductByIdAsync([FromBody] Product Product)
        {
            try
            {
                Product.ThrowIfArgumentNull(nameof(Product));
                Product.ProductId.ThrowIfArgumentEqualTo(nameof(Product.ProductId), 0);
                _logger.LogInfo("UpdateProductByCodeAsync()");
                return await _repositoryFactory.ProductRepository.UpdateProductByIdAsync(Product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}