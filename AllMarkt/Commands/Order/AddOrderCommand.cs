using AllMarkt.Data;
using AllMarkt.Entities;
using AllMarkt.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.Order
{
    public class AddOrderCommand : IRequest
    {
        public int ShopId { get; set; }

        public int CustomerId { get; set; }

        public string DeliveryPhoneNumber { get; set; }

        public string DeliveryAddress { get; set; }

        public float TotalPrice { get; set; }

        public string AdditionalNotes{ get; set; }

        public string AWB { get; set; }

        public ICollection<OrderItemViewModel> OrderItems { get; set; }
    }

    public class AddOrderCommandHandler : AsyncRequestHandler<AddOrderCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public AddOrderCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.OrderItems != null)
            {
                Entities.Order order = new Entities.Order();
                ICollection<OrderItem> orderItemList = GetOrderItems(request.OrderItems, order);

                Shop seller = _allMarktContext.Shops.Find(request.ShopId);
                Customer buyer = _allMarktContext.Customers.Find(request.CustomerId);

                if (seller != null && buyer != null)
                {
                    order.Seller = seller;
                    order.Buyer = buyer;
                    order.DeliveryPhoneNumber = request.DeliveryPhoneNumber;
                    order.DeliveryAddress = request.DeliveryAddress;
                    order.TotalPrice = request.TotalPrice;
                    order.TimeOfOrder = DateTime.Now;
                    order.AdditionalNotes = request.AdditionalNotes;
                    order.OrderStatus = Status.Registered;
                    order.AWB = request.AWB;
                    order.OrderItems = orderItemList;

                    _allMarktContext.Orders.Add(order);
                    _allMarktContext.OrderItems.AddRange(orderItemList);
                    await _allMarktContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        private ICollection<OrderItem> GetOrderItems(ICollection<OrderItemViewModel> orderItems, AllMarkt.Entities.Order order)
        {
            List<OrderItem> result = new List<OrderItem>();

            foreach (var orderItemVM in orderItems)
            {
                var orderItem = new OrderItem();
                orderItem.Id = orderItemVM.Id;
                orderItem.Name = orderItemVM.Name;
                orderItem.Amount = orderItemVM.Amount;
                orderItem.Order = order;
                result.Add(orderItem);
            }
            return result;
        }
    }
}
