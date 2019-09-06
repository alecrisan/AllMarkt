using AllMarkt.Data;
using AllMarkt.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.ProductComments
{
    public class AddProductCommentCommand : IRequest
    {
        public int Rating { get; set; }
        public string Text { get; set; }
        public int AddedByUserId { get; set; }
        public int ProductId { get; set; }
    }

    public class AddProductCommentCommandHandler : AsyncRequestHandler<AddProductCommentCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public AddProductCommentCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(AddProductCommentCommand request, CancellationToken cancellationToken)
        {
            var user = await _allMarktContext.Users.FindAsync(request.AddedByUserId);
            var product = await _allMarktContext.Products.FindAsync(request.ProductId);

            if (user != null && product != null)
            {
                _allMarktContext.ProductComments.Add(
                    new ProductComment
                    {
                        Rating = request.Rating,
                        Text = request.Text,
                        AddedBy = user,
                        Product = product,
                        DateAdded = DateTime.Now
                    });

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
