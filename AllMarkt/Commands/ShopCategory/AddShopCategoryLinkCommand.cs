using AllMarkt.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.ShopCategory
{
    public class AddShopCategoryLinkCommand : IRequest
    {
        public int ShopId { get; set; }
        public int CategoryId { get; set; }
    }

    public class AddShopCategoryLinkCommandHandler : AsyncRequestHandler<AddShopCategoryLinkCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public AddShopCategoryLinkCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(AddShopCategoryLinkCommand request, CancellationToken cancellationToken)
        {
            _allMarktContext.ShopCategories.Add(
              new Entities.ShopCategory
              {
                  ShopId = request.ShopId,
                  CategoryId = request.CategoryId
              }
          );

            await _allMarktContext.SaveChangesAsync(cancellationToken);
        }
    }
}
