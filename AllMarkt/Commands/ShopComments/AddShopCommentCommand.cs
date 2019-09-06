using AllMarkt.Data;
using AllMarkt.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.ShopComments
{
    public class AddShopCommentCommand : IRequest
    {
        public int Rating { get; set; }
        public string Text { get; set; }
        public int AddedByUserId { get; set; }
        public int ShopId { get; set; }
    }

    public class AddShopCommentCommandHandler : AsyncRequestHandler<AddShopCommentCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public AddShopCommentCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(AddShopCommentCommand request, CancellationToken cancellationToken)
        {
            var user = await _allMarktContext.Users.FindAsync(request.AddedByUserId);
            var shop = await _allMarktContext.Shops.FindAsync(request.ShopId);

            if (user != null && shop != null)
            {
                _allMarktContext.ShopComments.Add(
                    new ShopComment
                    {
                        Rating = request.Rating,
                        Text = request.Text,
                        AddedBy = user,
                        Shop = shop,
                        DateAdded = DateTime.Now
                    });

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
