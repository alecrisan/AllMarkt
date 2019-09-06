using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.ShopComments
{
    public class DeleteShopCommentCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteShopCommentCommandHandler : AsyncRequestHandler<DeleteShopCommentCommand>
    {
        private readonly AllMarktContext _allMarktContext;
        public DeleteShopCommentCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(DeleteShopCommentCommand request, CancellationToken cancellationToken)
        {
            var shopComment = await _allMarktContext
                .ShopComments
                .FindAsync(request.Id);

            if (shopComment != null)
            {
                _allMarktContext.ShopComments.Remove(shopComment);

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
