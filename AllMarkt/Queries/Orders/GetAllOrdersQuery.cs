using AllMarkt.Data;
using AllMarkt.Entities;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.Orders
{
    public class GetAllOrdersQuery : IRequest<IEnumerable<OrderViewModel>> { }

    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContex;

        public GetAllOrdersQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContex = allMarktQueryContext;
        }

        public async Task<IEnumerable<OrderViewModel>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _allMarktQueryContex
                 .Orders
                 .ToListAsync(cancellationToken);

            List<OrderViewModel> result = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                var orderVM = new OrderViewModel();
                orderVM.Id = order.Id;
                orderVM.ShopId = order.Seller == null ? 0 : order.Seller.Id;
                orderVM.ShopName = order.Seller == null ? "-" : (order.Seller.User == null ? "-" : order.Seller.User.DisplayName);
                orderVM.CustomerId = order.Buyer == null ? 0 : order.Buyer.Id;
                orderVM.CustomerName = order.Buyer == null ? "-" : (order.Buyer.User == null ? "-" : order.Buyer.User.DisplayName);
                orderVM.DeliveryPhoneNumber = order.DeliveryPhoneNumber;
                orderVM.DeliveryAddress = order.DeliveryAddress;
                orderVM.TotalPrice = order.TotalPrice;
                orderVM.TimeOfOrder = order.TimeOfOrder;
                orderVM.AdditionalNotes = order.AdditionalNotes;
                orderVM.OrderStatus = order.OrderStatus;
                orderVM.AWB = order.AWB;
                orderVM.OrderItems = GetOrderItemViewModels(order.OrderItems);
                result.Add(orderVM);
            }

            return result;
        }

        private ICollection<OrderItemViewModel> GetOrderItemViewModels(ICollection<OrderItem> orderItems)
        {
            List<OrderItemViewModel> result = new List<OrderItemViewModel>();

            foreach (var orderItem in orderItems)
            {
                var orderItemVM = new OrderItemViewModel();
                orderItemVM.Id = orderItem.Id;
                orderItemVM.Name = orderItem.Name;
                orderItemVM.Amount = orderItem.Amount;
                result.Add(orderItemVM);
            }

            return result;
        }
    }
}
