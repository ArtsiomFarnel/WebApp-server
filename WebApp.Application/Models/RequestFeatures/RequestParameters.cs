using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models.RequestFeatures
{
    public abstract class RequestParameters
    {
        const int maxPageSize = int.MaxValue;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 8;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public string SearchTerm { get; set; }

        public string OrderBy { get; set; }

        public string Fields { get; set; }
    }
}
