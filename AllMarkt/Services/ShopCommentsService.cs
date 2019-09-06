using AllMarkt.Commands.ShopComments;
using AllMarkt.Queries.ShopComments;
using AllMarkt.Services.Interfaces;
using AllMarkt.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services
{
    public class ShopCommentsService : IShopCommentsService
    {
        private readonly IMediator _mediator;

        public ShopCommentsService(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task<IEnumerable<ShopCommentViewModel>> GetAllAsync()
            => _mediator.Send(new GetAllShopCommentsQuery());

        public Task<IEnumerable<ShopCommentViewModel>> GetShopCommentsByShopId(int id)
            => _mediator.Send(new GetShopCommentsByShopQuery(id));

        public Task SaveAsync(ShopCommentViewModel viewModel)
            => viewModel.Id == 0 ? AddShopComment(viewModel) : EditShopComment(viewModel);

        private Task AddShopComment(ShopCommentViewModel viewModel)
            => _mediator.Send(new AddShopCommentCommand
            {
                Rating = viewModel.Rating,
                Text = viewModel.Text,
                AddedByUserId = viewModel.AddedById,
                ShopId = viewModel.ShopId,
            });

        private Task EditShopComment(ShopCommentViewModel viewModel)
            => _mediator.Send(new EditShopCommentCommand
            {
                Id = viewModel.Id,
                Text = viewModel.Text
            });

        public Task DeleteAsync(int id)
            => _mediator.Send(new DeleteShopCommentCommand
            {
                Id = id
            });
    }
}
