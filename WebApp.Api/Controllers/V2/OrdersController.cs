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
        private readonly IBus _bus;

        public OrdersController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            if (order != null)
            {
                Uri uri = new Uri("rabbitmq://localhost/orders");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(order);
                return Ok();
            }
            return BadRequest();
        }

    }
}
