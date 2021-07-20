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
using WebApp.Application.Models.DataTransferObjects.Incoming.Providers;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Categories;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Providers;
using WebApp.Application.Models.RequestFeatures.Provider;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("v{version:apiVersion}/providers")]
    public class ProvidersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IDataShaper<ProviderFullInfoDto> _dataShaper;

        public ProvidersController(ILoggerManager logger,
            IRepositoryManager repository, 
            IMapper mapper, 
            IDataShaper<ProviderFullInfoDto> dataShaper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        [HttpGet("get_all_providers")]
        public async Task<IActionResult> GetAllProviders([FromQuery] ProviderParameters param)
        {
            try
            {
                var providers = await _repository.Providers.GetAllProvidersAsync(param, false);
                Response.Headers.Add("Pagination", JsonConvert.SerializeObject(providers.MetaData));

                var result = _mapper.Map<IEnumerable<ProviderFullInfoDto>>(providers);

                return Ok(_dataShaper.ShapeData(result, param.Fields));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllProviders)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get_provider")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Provider>))]
        public async Task<IActionResult> GetProvider(int? id, string fields)
        {
            try
            {
                var provider = await _repository.Providers.GetProviderByIdAsync((int)id, false);

                var result = _mapper.Map<ProviderFullInfoDto>(provider);

                return Ok(_dataShaper.ShapeData(result, fields));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetProvider)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("add_provider")]
        [ServiceFilter(typeof(ValidationActionFilter))]
        public async Task<IActionResult> AddProvider([FromBody] ProviderAddDto providerAdd)
        {
            try
            {
                var provider = _mapper.Map<Provider>(providerAdd);

                _repository.Providers.Create(provider);
                await _repository.SaveAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(AddProvider)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete_provider")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Provider>))]
        public async Task<IActionResult> DeleteProvider(int? id)
        {
            try
            {
                var provider = await _repository.Providers.GetProviderByIdAsync((int)id, true);

                _repository.Providers.Delete(provider);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteProvider)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("update_provider")]
        [ServiceFilter(typeof(ValidationActionFilter))]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Provider>))]
        public async Task<IActionResult> UpdateProvider(int? id, [FromBody] ProviderUpdateDto providerUp)
        {
            try
            {
                var provider = await _repository.Providers.GetProviderByIdAsync((int)id, true);

                _mapper.Map(providerUp, provider);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateProvider)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPatch("partially_update_provider")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Provider>))]
        public async Task<IActionResult> PartiallyUpdateProvider(int? id, [FromBody] JsonPatchDocument<ProviderUpdateDto> patchDoc)
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
                    var provider = await _repository.Providers.GetProviderByIdAsync((int)id, true);

                    var categoryToPatch = _mapper.Map<ProviderUpdateDto>(provider);
                    patchDoc.ApplyTo(categoryToPatch);
                    _mapper.Map(categoryToPatch, provider);
                    await _repository.SaveAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong in the {nameof(PartiallyUpdateProvider)} action {ex} ");
                    return StatusCode(500, "Internal server error");
                }
            }
        }
    }
}
