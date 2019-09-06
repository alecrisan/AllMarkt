using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.ShopCategory
{
    public class DeleteShopCategoryLinkCommand : IRequest
    {
        public int ShopId { get; set; }
        public int CategoryId { get; set; }
    }
    public class DeleteShopCategoryLinkCommandHandler : AsyncRequestHandler<DeleteShopCategoryLinkCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public DeleteShopCategoryLinkCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(DeleteShopCategoryLinkCommand request, CancellationToken cancellationToken)
        {
           var shopCategory = await _allMarktContext
                .ShopCategories
                .Where(sc => sc.ShopId == request.ShopId)
                .Where(sc => sc.CategoryId == request.CategoryId)
                .FirstOrDefaultAsync();

            _allMarktContext.ShopCategories.Remove(shopCategory);
            await _allMarktContext.SaveChangesAsync();
        }
    }
}
