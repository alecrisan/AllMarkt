using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.ProductCategory
{
    public class DeleteProductCategoryCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteProductCategoryCommandHandler : AsyncRequestHandler<DeleteProductCategoryCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public DeleteProductCategoryCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }
        protected override async Task Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var productCategory = await _allMarktContext
                .ProductCategories
                .Include(pc=>pc.Products)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            foreach(var product in productCategory.Products)
            {
                _allMarktContext.Products.Remove(product);
            }
            await _allMarktContext.SaveChangesAsync(cancellationToken);

            if (productCategory != null)
            {
                _allMarktContext.ProductCategories.Remove(productCategory);
                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
