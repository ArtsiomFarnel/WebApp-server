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
    }
}
//я пчёлка бзз бзззз