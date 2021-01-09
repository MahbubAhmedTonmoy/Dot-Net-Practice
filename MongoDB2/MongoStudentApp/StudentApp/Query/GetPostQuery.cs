using Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.Query
{
    public class GetPostQuery: BaseQuery
    {
        public string UserEmail { get; set; }
    }
    public class BaseQuery : IRequest<object>
    {
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
