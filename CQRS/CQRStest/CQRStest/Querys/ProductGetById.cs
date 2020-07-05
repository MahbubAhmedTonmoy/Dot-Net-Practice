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
    public class ProductGetById
    {
        public class Query : IRequest<ProductDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ProductDto>
        {
            private readonly DataContext _db;
            private readonly IMapper _mapper;
            public Handler(DataContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }
            public async Task<ProductDto> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.Id != 0)
                {
                    var product =await _db.Products.FirstOrDefaultAsync(i => i.Id == request.Id);
                    if (product != null)
                    {
                        var a = _mapper.Map<Product, ProductDto>(product);
                        return a;
                    }
                }
                throw new Exception("not found");

            }
        }
    }
}
