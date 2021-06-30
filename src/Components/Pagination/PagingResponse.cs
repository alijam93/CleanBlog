using CleanBlog.Shared.Features.Pagination;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Components.Pagination
{
    public class PagingResponse<T> where T : class
    {
        public List<T> Items { get; set; }
        public Paging Paging { get; set; }
    }
}
