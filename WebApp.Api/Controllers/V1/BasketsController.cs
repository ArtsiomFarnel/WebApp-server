using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Abstractions;
using WebApp.Application.Interfaces;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Baskets;
using WebApp.Application.Models.RequestFeatures.Baskets;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("v{version:apiVersion}/baskets")]
    public class BasketsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IDataShaper<BasketItemFullInfoDto> _dataShaper;
        private readonly UserManager<User> _userManager;

        public BasketsController(
            ILoggerManager logger,
            IRepositoryManager repository,
            IMapper mapper,
            IDataShaper<BasketItemFullInfoDto> dataShaper,
            UserManager<User> userManager)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _dataShaper = dataShaper;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets the basket of current user
        /// </summary>
        /// <returns>The basket items list</returns>
        [Authorize(Roles = "Client")]
        [HttpGet("get_basket")]
        public async Task<IActionResult> GetBasket([FromQuery] BasketParameters param)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var basket = await _repository.Baskets.GetAllBasketItemsAsync(param, false, user.Id);
                Response.Headers.Add("pagination", JsonConvert.SerializeObject(basket.MetaData));

                var result = _mapper.Map<IEnumerable<BasketItemFullInfoDto>>(basket);

                return Ok(_dataShaper.ShapeData(result, param.Fields));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetBasket)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
