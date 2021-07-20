using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models.RequestFeatures.Provider
{
    public class ProviderParameters : RequestParameters
    {
        public ProviderParameters()
        {
            OrderBy = "name";
        }
    }
}
