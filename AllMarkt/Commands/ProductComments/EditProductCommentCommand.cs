using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.ProductComments
{
    public class EditProductCommentCommand : IRequest
    {
        public int Id { get; set; }

        public string Text { get; set; }
    }

    public class EditProductCommentCommandHandler : AsyncRequestHandler<EditProductCommentCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public EditProductCommentCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(EditProductCommentCommand request, CancellationToken cancellationToken)
        {
            var productComment = await _allMarktContext
                .ProductComments
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (productComment != null)
            {
                productComment.Text = request.Text;
                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
