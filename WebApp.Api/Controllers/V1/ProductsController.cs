using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

namespace WebApp.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
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

        [HttpGet("get_all_products")]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductParameters param)
        {
            try
            {
                var products = await _repository.Products.GetAllProductsAsync(param, false);
                Response.Headers.Add("Pagination", JsonConvert.SerializeObject(products.MetaData));

                var result = _mapper.Map<IEnumerable<ProductFullInfoDto>>(products);

                return Ok(_dataShaper.ShapeData(result, param.Fields));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllProducts)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get_product")]
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

        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete_product")]
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
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteProduct)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("update_product")]
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
        
        [Authorize(Roles = "Administrator")]
        [HttpPatch("partially_update_product")]
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
