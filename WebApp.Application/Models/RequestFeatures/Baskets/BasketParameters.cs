using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models.RequestFeatures.Baskets
{
    public class BasketParameters : RequestParameters
    {
        public BasketParameters()
        {
            OrderBy = "name";
        }
    }
}
