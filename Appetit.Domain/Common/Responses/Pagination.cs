using Appetit.Domain.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appetit.Domain.Common.Responses
{
    public class Pagination(int page, int totalItems)
    {

        public int Page { get; set; } = page;
        public int TotalItems { get; set; } = totalItems;
        public int TotalPages { get; set; } = (int)Math.Ceiling((double)totalItems / Paging.DefaultPageSize);
    }
}
