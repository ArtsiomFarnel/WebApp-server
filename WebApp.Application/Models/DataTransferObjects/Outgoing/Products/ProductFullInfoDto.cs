using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models.DataTransferObjects.Outgoing.Products
{
    public class ProductFullInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }
        public string ImagePath { get; set; }
        public string Provider { get; set; }
        public string Category { get; set; }
    }
}
