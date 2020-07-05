using CQRStest.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRStest.Commands
{
    public class ProductDelete
    {
        public class Command: IRequest
        {
            public int Id { get; set; }
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
                var getProduct = await _db.Products.FindAsync(request.Id);
                if(getProduct != null)
                {
                    _db.Remove(getProduct);

                    var success = await _db.SaveChangesAsync() > 0;
                    if (success) return Unit.Value;
                }

                throw new Exception("Problem saving changes");
            }
        }
    }
}
