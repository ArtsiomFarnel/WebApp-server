using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models.DataTransferObjects.Outgoing.Baskets
{
    public class BasketItemFullInfoDto
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string ProductName { get; set; }
        public string ProductImagePath { get; set; }
        public float ProductCost { get; set; }
    }
}
