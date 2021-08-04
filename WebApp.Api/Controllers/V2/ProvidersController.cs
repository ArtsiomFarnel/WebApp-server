using AutoMapper;
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
using WebApp.Application.Models.DataTransferObjects.Outgoing.Providers;
using WebApp.Application.Models.RequestFeatures.Provider;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
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

        /// <summary>
        /// Gets the list of all providers
        /// </summary>
        /// <returns>The providers list</returns>
        [HttpGet("get_all_providers")]
        public async Task<IActionResult> GetAllProviders([FromQuery] ProviderParameters param)
        {
            try
            {
                var providers = await _repository.Providers.GetAllProvidersAsync(param, false);
                Response.Headers.Add("Pagination", JsonConvert.SerializeObject(providers.MetaData));

                var result = _mapper.Map<IEnumerable<ProviderFullInfoDto>>(providers);

                return Ok(new { providers = _dataShaper.ShapeData(result, param.Fields), pagination = providers.MetaData });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllProviders)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a provider
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <returns>A provider</returns>
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

        /// <summary>
        /// Creates a new provider
        /// </summary>
        /// <param name="providerAdd"></param>
        /// <returns>Created newly provider</returns>
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

        /// <summary>
        /// Deletes a provider
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deleted provider</returns>
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

        /// <summary>
        /// Updates a provider
        /// </summary>
        /// <param name="id"></param>
        /// <param name="providerUp"></param>
        /// <returns>Updated provider</returns>
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

        /// <summary>
        /// Partially updates a provider
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns>Partialy updated provider</returns>
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
