using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models.DataTransferObjects.Shared
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public int ProductId { get; set; }
    }
}
