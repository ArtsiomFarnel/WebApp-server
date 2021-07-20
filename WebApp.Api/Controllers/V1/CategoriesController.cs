using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Api.Filters;
using WebApp.Application.Abstractions;
using WebApp.Application.Interfaces;
using WebApp.Application.Models.DataTransferObjects.Incoming.Categories;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Categories;
using WebApp.Application.Models.RequestFeatures.Category;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
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

        [HttpGet("get_category")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Category>))]
        public async Task<IActionResult> GetCategory(int? id, string fields)
        {
            try
            {
                var category = await _repository.Categories.GetCategoryByIdAsync((int)id, false);

                var result = _mapper.Map<CategoryFullInfoDto>(category);

                return Ok(_dataShaper.ShapeData(result, fields));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetCategory)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("add_category")]
        [ServiceFilter(typeof(ValidationActionFilter))]
        public async Task<IActionResult> AddCategory([FromBody] CategoryAddDto categoryAdd)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryAdd);

                _repository.Categories.Create(category);
                await _repository.SaveAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(AddCategory)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete_category")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Category>))]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            try
            {
                var category = await _repository.Categories.GetCategoryByIdAsync((int)id, true);

                _repository.Categories.Delete(category);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteCategory)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("update_category")]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Category>))]
        public async Task<IActionResult> UpdateCategory(int? id, [FromBody] CategoryUpdateDto categoryUp)
        {
            try
            {
                var category = await _repository.Categories.GetCategoryByIdAsync((int)id, true);

                _mapper.Map(categoryUp, category);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateCategory)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPatch("partially_update_category")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Category>))]
        public async Task<IActionResult> PartiallyUpdateCategory(int? id, [FromBody] JsonPatchDocument<CategoryUpdateDto> patchDoc)
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
                    var category = await _repository.Categories.GetCategoryByIdAsync((int)id, true);

                    var categoryToPatch = _mapper.Map<CategoryUpdateDto>(category);
                    patchDoc.ApplyTo(categoryToPatch);
                    _mapper.Map(categoryToPatch, category);
                    await _repository.SaveAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong in the {nameof(PartiallyUpdateCategory)} action {ex} ");
                    return StatusCode(500, "Internal server error");
                }
            }
        }
    }
}
