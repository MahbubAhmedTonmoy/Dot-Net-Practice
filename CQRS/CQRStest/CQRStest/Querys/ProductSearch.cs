using AutoMapper;
using CQRStest.Data;
using CQRStest.DTO;
using CQRStest.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRStest.Querys
{
    public class ProductSearch
    {
        public class Query : IRequest<List<ProductDto>>
        {
            public string queryPerameter { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<ProductDto>>
        {
            private readonly DataContext _db;
            private readonly IMapper _mapper;
            public Handler(DataContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }
            public async Task<List<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!string.IsNullOrEmpty(request.queryPerameter))
                {
                    var product =await _db.Products.Where(x => x.Title.Contains(request.queryPerameter)).ToListAsync();
                    if(product != null)
                    {
                        var a = _mapper.Map<List<Product>, List<ProductDto>>(product);
                        return a;
                    }
                }
                throw new Exception("not found");
                
            }
        }
    }
}
