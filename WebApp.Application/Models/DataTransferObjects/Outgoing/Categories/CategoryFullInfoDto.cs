using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Data.Entities;

namespace WebApp.Application.Models.DataTransferObjects.Outgoing.Categories
{
    public class CategoryFullInfoDto
    {
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
