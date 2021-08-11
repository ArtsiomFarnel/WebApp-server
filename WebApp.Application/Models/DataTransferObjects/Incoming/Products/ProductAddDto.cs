using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models.DataTransferObjects.Incoming.Products
{
    public class ProductAddDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
        public int ProviderId { get; set; }
    }
}
