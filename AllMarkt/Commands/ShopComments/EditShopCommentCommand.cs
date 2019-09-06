using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.ShopComments
{
    public class EditShopCommentCommand : IRequest
    {
        public int Id { get; set; }

        public string Text { get; set; }
    }

    public class EditShopCommentCommandHandler : AsyncRequestHandler<EditShopCommentCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public EditShopCommentCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(EditShopCommentCommand request, CancellationToken cancellationToken)
        {
            var shopComment = await _allMarktContext
                .ShopComments
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (shopComment != null)
            {
                shopComment.Text = request.Text;
                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
