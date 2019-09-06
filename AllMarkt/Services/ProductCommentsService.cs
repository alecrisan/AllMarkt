using AllMarkt.Commands.ProductComments;
using AllMarkt.Queries.ProductComments;
using AllMarkt.Services.Interfaces;
using AllMarkt.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services
{
    public class ProductCommentsService : IProductCommentsService
    {
        private readonly IMediator _mediator;

        public ProductCommentsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<IEnumerable<ProductCommentViewModel>> GetAllAsync()
            => _mediator.Send(new GetAllProductCommentsQuery());

        public Task<IEnumerable<ProductCommentViewModel>> GetProductCommentsByProductIdAsync(int id)
            => _mediator.Send(new GetProductCommentsByProductIdQuery(id));

        public Task SaveAsync(ProductCommentViewModel viewModel)
            => viewModel.Id == 0 ? AddProductComment(viewModel) : EditProductComment(viewModel);

        private Task AddProductComment(ProductCommentViewModel viewModel)
            => _mediator.Send(new AddProductCommentCommand
            {
                Rating = viewModel.Rating,
                Text = viewModel.Text,
                AddedByUserId = viewModel.AddedById,
                ProductId = viewModel.ProductId,
            });

        private Task EditProductComment(ProductCommentViewModel viewModel)
            => _mediator.Send(new EditProductCommentCommand
            {
                Id = viewModel.Id,
                Text = viewModel.Text
            });

        public Task DeleteAsync(int id)
            => _mediator.Send(new DeleteProductCommentCommand
            {
                Id = id
            });
    }
}
