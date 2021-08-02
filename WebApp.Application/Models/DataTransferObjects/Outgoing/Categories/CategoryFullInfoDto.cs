using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Products;

namespace WebApp.Application.Models.DataTransferObjects.Outgoing.Categories
{
    public class CategoryFullInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductFullInfoDto> Products { get; set; }
    }
}
