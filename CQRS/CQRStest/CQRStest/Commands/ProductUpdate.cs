using CQRStest.Data;
using CQRStest.Model;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRStest.Commands
{
    public class ProductUpdate
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime? Date { get; set; }
            public double? Price { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Category).NotEmpty();
                RuleFor(x => x.Date).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Price).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _db;
            public Handler(DataContext db)
            {
                _db = db;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var getProduct =await _db.Products.FindAsync(request.Id);
                if(getProduct != null)
                {
                    getProduct.Date = request.Date ?? getProduct.Date;
                    getProduct.Title = request.Title ?? getProduct.Title;
                    getProduct.Description = request.Description ?? getProduct.Description;
                    getProduct.Category = request.Category ?? getProduct.Category;
                    getProduct.Price = request.Price ?? getProduct.Price;

                    var success = await _db.SaveChangesAsync() > 0;
                    if (success) return Unit.Value;
                    throw new Exception("Problem saving changes");
                }
                throw new Exception("------------");
            }

        }
    }
}
