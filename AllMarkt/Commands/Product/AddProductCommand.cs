using AllMarkt.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.Product
{
    public class AddProductCommand : IRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string ImageURI { get; set; }

        public bool State { get; set; }

        public int ProductCategoryId { get; set; }
    }

    public class AddProductCommandHandler : AsyncRequestHandler<AddProductCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public AddProductCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var productCategory = _allMarktContext.ProductCategories.FindAsync(request.ProductCategoryId);

            if (productCategory != null)
            {
                _allMarktContext.Products.Add(
                    new Entities.Product
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Price = request.Price,
                        ImageURI = request.ImageURI,
                        State = request.State,
                        ProductCategory = await productCategory
                    }
                );

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
