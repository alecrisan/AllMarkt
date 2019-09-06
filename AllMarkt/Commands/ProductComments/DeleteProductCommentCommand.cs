using AllMarkt.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.ProductComments
{
    public class DeleteProductCommentCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteProductCommentCommandHandler : AsyncRequestHandler<DeleteProductCommentCommand>
    {
        private readonly AllMarktContext _allMarktContext;
        public DeleteProductCommentCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(DeleteProductCommentCommand request, CancellationToken cancellationToken)
        {
            var productComment = await _allMarktContext
                .ProductComments
                .FindAsync(request.Id);

            if (productComment != null)
            {
                _allMarktContext.ProductComments.Remove(productComment);

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
