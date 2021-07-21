using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Abstractions;
using WebApp.Application.Interfaces;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Providers;
using WebApp.Application.Models.RequestFeatures.Provider;

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

                return Ok(_dataShaper.ShapeData(result, param.Fields));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllProviders)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
