using AllMarkt.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.ProductCategory
{
    public class AddProductCategoryCommand : IRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int ShopId { get; set; }
    }

    public class AddProductCategoryCommandHandler : AsyncRequestHandler<AddProductCategoryCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public AddProductCategoryCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }


        protected override async Task Handle(AddProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var shop = await _allMarktContext.Shops.FindAsync(request.ShopId);

            if (shop != null)
            {
                _allMarktContext.ProductCategories.Add(new Entities.ProductCategory
                {
                    Name = request.Name,
                    Description = request.Description,
                    Shop = shop
                });

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
