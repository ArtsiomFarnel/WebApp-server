using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Abstractions;
using WebApp.Application.Interfaces;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Categories;
using WebApp.Application.Models.RequestFeatures.Category;

namespace WebApp.Api.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("v{version:apiVersion}/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IDataShaper<CategoryFullInfoDto> _dataShaper;

        public CategoriesController(
            ILoggerManager logger, 
            IRepositoryManager repository, 
            IMapper mapper, 
            IDataShaper<CategoryFullInfoDto> dataShaper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        /// <summary>
        /// Gets the list of all categories
        /// </summary>
        /// <returns>The categories list</returns>
        [HttpGet("get_all_categories")]
        public async Task<IActionResult> GetAllCategories([FromQuery] CategoryParameters param)
        {
            try
            {
                var categories = await _repository.Categories.GetAllCategoriesAsync(param, false);
                Response.Headers.Add("Pagination", JsonConvert.SerializeObject(categories.MetaData));

                var result = _mapper.Map<IEnumerable<CategoryFullInfoDto>>(categories);

                return Ok(_dataShaper.ShapeData(result, param.Fields));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllCategories)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
