using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Models.DataTransferObjects.Shared;

namespace WebApp.Api.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("v{version:apiVersion}/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public OrdersController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            if (order != null)
            {
                /*
                Uri uri = new Uri("rabbitmq://localhost/orders");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(order);
                */
                await _publishEndpoint.Publish<Order>(order);
                return Ok();
            }
            return BadRequest();
        }

    }
}
