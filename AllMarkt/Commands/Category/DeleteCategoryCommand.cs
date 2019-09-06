using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllMarkt.Commands.Category
{
    public class DeleteCategoryCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteCategoryCommandHandler : AsyncRequestHandler<DeleteCategoryCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public DeleteCategoryCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {   
            var category = await _allMarktContext
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category != null)
            {
                _allMarktContext.Categories.Remove(category);

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
