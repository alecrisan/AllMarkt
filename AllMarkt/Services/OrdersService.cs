using System.Collections.Generic;
using System.Threading.Tasks;
using AllMarkt.Commands.Order;
using AllMarkt.Queries.Orders;
using AllMarkt.Services.Interfaces;
using AllMarkt.ViewModels;
using MediatR;

namespace AllMarkt.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IMediator _mediator;

        public OrdersService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<IEnumerable<OrderViewModel>> GetAllAsync()
            => _mediator.Send(new GetAllOrdersQuery());

        public Task<IEnumerable<OrderViewModel>> GetShopOrdersAsync(int shopId)
            => _mediator.Send(new GetShopOrdersQuery() { ShopId = shopId });

        public Task<IEnumerable<OrderViewModel>> GetCustomerOrdersAsync(int customerId)
            => _mediator.Send(new GetCustomerOrdersQuery() { CustomerId = customerId });

        public Task SaveAsync(OrderViewModel viewModel)
            => viewModel.Id == 0 ? AddOrder(viewModel) : EditOrder(viewModel);

        private Task AddOrder(OrderViewModel viewModel)
            => _mediator.Send(new AddOrderCommand
            {
                ShopId = viewModel.ShopId,
                CustomerId = viewModel.CustomerId,
                DeliveryPhoneNumber = viewModel.DeliveryPhoneNumber,
                DeliveryAddress = viewModel.DeliveryAddress,
                TotalPrice = viewModel.TotalPrice,
                AdditionalNotes = viewModel.AdditionalNotes,
                AWB = viewModel.AWB,
                OrderItems = viewModel.OrderItems

            });

        private Task EditOrder(OrderViewModel viewModel)
            => _mediator.Send(new EditOrderCommand
            {
                Id = viewModel.Id,
                DeliveryPhoneNumber = viewModel.DeliveryPhoneNumber,
                DeliveryAddress = viewModel.DeliveryAddress,
                TotalPrice = viewModel.TotalPrice,
                AdditionalNotes = viewModel.AdditionalNotes,
                AWB = viewModel.AWB,
                OrderStatus = viewModel.OrderStatus,
                OrderItems = viewModel.OrderItems
            });

        public Task DeleteAsync(int id)
            => _mediator.Send(new DeleteOrderCommand
            {
                Id = id
            });
    }
}
