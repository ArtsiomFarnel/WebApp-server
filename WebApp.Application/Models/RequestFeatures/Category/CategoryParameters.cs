using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models.RequestFeatures.Category
{
    public class CategoryParameters : RequestParameters
    {
        public CategoryParameters()
        {
            OrderBy = "name";
        }
    }
}
