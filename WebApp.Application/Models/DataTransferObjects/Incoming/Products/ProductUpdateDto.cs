using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models.DataTransferObjects.Incoming.Products
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }
        public string ImagePath { get; set; }
    }
}
