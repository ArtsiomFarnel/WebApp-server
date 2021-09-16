using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageBrokerShared;
using Microsoft.AspNetCore.Authorization;
using WebApp.Application.Abstractions;

namespace WebApp.Api.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("v{version:apiVersion}/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepositoryManager _repository;

        public OrdersController(IPublishEndpoint publishEndpoint, IRepositoryManager repository)
        {
            _publishEndpoint = publishEndpoint;
            _repository = repository;
        }

        [HttpPost]
        [Authorize(Roles = ("Client"))]
        public async Task<IActionResult> CreateOrder(OrderFullInfo order)
        {
            var product = await _repository.Products.GetProductByIdAsync(order.ProductId, trackChanges: false);
            if (product == null)
                return BadRequest($"Product with id: {order.ProductId} doesn't exist in database.");

            if (order != null)
            {
                await _publishEndpoint.Publish<OrderFullInfo>(order);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = ("Client"))]
        public async Task<IActionResult> UpdateOrderStatus(OrderFullInfo order)
        {
            if (order.Status != "Processed" && order.Status != "On the way" && order.Status != "Delivered")
                return BadRequest("Status must has value: 'Processed','On the way' or 'Delivered'");

            await _publishEndpoint.Publish<OrderFullInfo>(order);

            return Ok("The query is accepted for processing.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = ("Client"))]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = new OrderFullInfo() 
            { 
                Id = id, 
                Function = FunctionType.DELETE 
            };

            await _publishEndpoint.Publish<OrderFullInfo>(order);

            return Ok("The query is accepted for processing.");
        }
    }
}
