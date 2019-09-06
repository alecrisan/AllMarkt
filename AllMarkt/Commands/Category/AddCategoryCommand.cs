using AllMarkt.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.Category
{
    public class AddCategoryCommand : IRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class AddCategoryCommandHandler : AsyncRequestHandler<AddCategoryCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public AddCategoryCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            _allMarktContext.Categories.Add(
                new Entities.Category
                {
                    Name = request.Name,
                    Description = request.Description
                }
            );

            await _allMarktContext.SaveChangesAsync(cancellationToken);
        }
    }
}
