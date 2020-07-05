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
    public class ProductsGetAll
    {
        public class Query : IRequest<List<ProductDto>>
        {

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
                var result = await _db.Products.ToListAsync();
                var a = _mapper.Map<List<Product>, List<ProductDto>>(result);
                return a;
            }
        }
    }
}
