using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.Product
{
    public class EditProductCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageURI { get; set; }
        public bool State { get; set; }
    }

    public class EditProductCommandHandler : AsyncRequestHandler<EditProductCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public EditProductCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _allMarktContext
                .Products
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product != null)
            {
                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                product.ImageURI = request.ImageURI;
                product.State = request.State;

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
