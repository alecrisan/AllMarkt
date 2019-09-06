using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllMarkt.Commands.Category
{
    public class EditCategoryCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class EditCategoryCommandHandler : AsyncRequestHandler<EditCategoryCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public EditCategoryCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _allMarktContext
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category != null)
            {
                category.Name = request.Name;
                category.Description = request.Description;

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
