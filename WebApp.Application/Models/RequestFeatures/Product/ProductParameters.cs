using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models.RequestFeatures.Product
{
    public class ProductParameters : RequestParameters
    {
        public ProductParameters()
        {
            OrderBy = "name";
        }

        public float MinCost { get; set; } = 0.0f;
        public float MaxCost { get; set; } = float.MaxValue;

        public string Currency { get; set; } = "EUR";
    }
}
