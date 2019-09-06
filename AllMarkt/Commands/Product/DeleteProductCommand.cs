using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.Product
{
    public class DeleteProductCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteProductCommandHandler : AsyncRequestHandler<DeleteProductCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public DeleteProductCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _allMarktContext
                .Products
                .Include(p=>p.Comments)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            foreach(var comment in product.Comments)
            {
                _allMarktContext.ProductComments.Remove(comment);
            }
            await _allMarktContext.SaveChangesAsync(cancellationToken);

            if (product != null)
            {
                _allMarktContext.Products.Remove(product);

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
