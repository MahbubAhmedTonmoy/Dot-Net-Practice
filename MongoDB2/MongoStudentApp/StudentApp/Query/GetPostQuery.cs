using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.Query
{
    public class GetPostQuery: IRequest<object>
    {
        public string UserEmail { get; set; }
    }
}
