using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Api.Filters;
using WebApp.Application.Abstractions;
using WebApp.Application.Interfaces;
using WebApp.Application.Models.DataTransferObjects.Incoming.Products;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Products;
using WebApp.Application.Models.RequestFeatures.Product;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("v{version:apiVersion}/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IDataShaper<ProductFullInfoDto> _dataShaper;

        public ProductsController(
            ILoggerManager logger, 
            IRepositoryManager repository, 
            IMapper mapper, 
            IDataShaper<ProductFullInfoDto> dataShaper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        /// <summary>
        /// Gets the list of all products
        /// </summary>
        /// <returns>The products list</returns>
        [HttpGet("get_all_products")]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductParameters param)
        {
            try
            {
                var products = await _repository.Products.GetAllProductsAsync(param, false);
                Response.Headers.Add("pagination", JsonConvert.SerializeObject(products.MetaData));

                var result = _mapper.Map<IEnumerable<ProductFullInfoDto>>(products);

                return Ok(_dataShaper.ShapeData(result, param.Fields));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllProducts)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <returns>A product</returns>
        [HttpGet("get_product/{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Product>))]
        public async Task<IActionResult> GetProduct(int? id, string fields)
        {
            try
            {
                var product = await _repository.Products.GetProductByIdAsync((int)id, false);

                var result = _mapper.Map<ProductFullInfoDto>(product);

                return Ok(_dataShaper.ShapeData(result, fields));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetProduct)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creats a new product
        /// </summary>
        /// <param name="productAdd"></param>
        /// <returns>A newly product</returns>
        [HttpPost("add_product")]
        [ServiceFilter(typeof(ValidationActionFilter))]
        public async Task<IActionResult> AddProduct([FromBody] ProductAddDto productAdd)
        {
            try
            {
                var product = _mapper.Map<Product>(productAdd);

                _repository.Products.Create(product);
                await _repository.SaveAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(AddProduct)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deleted product</returns>
        [HttpDelete("delete_product/{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Product>))]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            try
            {
                var product = await _repository.Products.GetProductByIdAsync((int)id, true);

                _repository.Products.Delete(product);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteProduct)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productUp"></param>
        /// <returns>Updated product</returns>
        [HttpPut("update_product/{id}")]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Product>))]
        public async Task<IActionResult> UpdateProduct(int? id, [FromBody] ProductUpdateDto productUp)
        {
            try
            {
                var product = await _repository.Products.GetProductByIdAsync((int)id, true);

                _mapper.Map(productUp, product);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateProduct)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Partially updates a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns>Partialy updated product</returns>
        [HttpPatch("partially_update_product/{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Product>))]
        public async Task<IActionResult> PartiallyUpdateProduct(int? id, [FromBody] JsonPatchDocument<ProductUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            else
            {
                try
                {
                    var product = await _repository.Products.GetProductByIdAsync((int)id, true);

                    var productToPatch = _mapper.Map<ProductUpdateDto>(product);
                    patchDoc.ApplyTo(productToPatch);
                    _mapper.Map(productToPatch, product);
                    await _repository.SaveAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong in the {nameof(PartiallyUpdateProduct)} action {ex} ");
                    return StatusCode(500, "Internal server error");
                }
            }
        }
    }
}
//я пчёлка бзз бзззз