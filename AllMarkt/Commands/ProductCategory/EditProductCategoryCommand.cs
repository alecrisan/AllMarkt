using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.ProductCategory
{
    public class EditProductCategoryCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class EditProductCategoryCommandHandler : AsyncRequestHandler<EditProductCategoryCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public EditProductCategoryCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(EditProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var productCategory = await _allMarktContext.ProductCategories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (productCategory != null)
            {
                productCategory.Name = request.Name;
                productCategory.Description = request.Description;

                await _allMarktContext.SaveChangesAsync();
            }
        }
    }
}
