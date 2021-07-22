using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Products;

namespace WebApp.Application.Models.DataTransferObjects.Outgoing.Providers
{
    public class ProviderFullInfoDto
    {
        public string Name { get; set; }
        public IEnumerable<ProductFullInfoDto> Products { get; set; }
    }
}
