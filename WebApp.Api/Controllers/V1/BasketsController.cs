using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Api.Filters;
using WebApp.Application.Abstractions;
using WebApp.Application.Interfaces;
using WebApp.Application.Models.DataTransferObjects.Incoming.Baskets;
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

        /// <summary>
        /// Adds new item to basket items list
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ok</returns>
        [Authorize(Roles = "Client")]
        [HttpGet("add_to_basket/{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Product>))]
        public async Task<IActionResult> AddToBasket(int? id)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var basketItem = await _repository.Baskets.GetBasketItemByProductIdAsync((int)id, true, user.Id);
                if (basketItem != null) 
                {
                    basketItem.Amount++;
                }
                else
                {
                    var newbasketItem = new Basket
                    {
                        UserId = user.Id,
                        ProductId = (int)id,
                        Amount = 1
                    };

                    _repository.Baskets.Create(newbasketItem);
                }
                await _repository.SaveAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(AddToBasket)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Removes item from basket
        /// </summary>
        /// <param name="id"></param>
        /// <returns>No content</returns>
        [Authorize(Roles = "Client")]
        [HttpDelete("remove_from_basket/{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Basket>))]
        public async Task<IActionResult> RemoveFromBasket(int? id)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var basketItem = await _repository.Baskets.GetBasketItemByIdAsync((int)id, true, user.Id);

                _repository.Baskets.Delete(basketItem);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(RemoveFromBasket)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Change item amount
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amountUp"></param>
        /// <returns>Ok</returns>
        [Authorize(Roles = "Client")]
        [HttpPut("change_amount/{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsActionFilter<Basket>))]
        public async Task<IActionResult> ChangeAmount(int? id, [FromBody] AmountUpdateDto amountUp)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var basketItem = await _repository.Baskets.GetBasketItemByIdAsync((int)id, true, user.Id);

                basketItem.Amount = amountUp.Amount;
                await _repository.SaveAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(ChangeAmount)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
