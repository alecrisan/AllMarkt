using AllMarkt.Data;
using AllMarkt.Entities;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.Order
{
    public class EditOrderCommand : IRequest
    {
        public int Id { get; set; }

        public string DeliveryPhoneNumber { get; set; }

        public string DeliveryAddress { get; set; }

        public float TotalPrice { get; set; }

        public string AdditionalNotes { get; set; }

        public string AWB { get; set; }

        public Status OrderStatus { get; set; }

        public ICollection<OrderItemViewModel> OrderItems { get; set; }
    }

    public class EditOrderCommandHandler : AsyncRequestHandler<EditOrderCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public EditOrderCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(EditOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _allMarktContext
                .Orders.Include(o=>o.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (order != null)
            {
                order.DeliveryAddress = request.DeliveryAddress;
                order.DeliveryPhoneNumber = request.DeliveryPhoneNumber;
                order.AdditionalNotes = request.AdditionalNotes;
                order.AWB = request.AWB;
                order.TotalPrice = request.TotalPrice;
                order.OrderStatus = request.OrderStatus;

                if (request.OrderItems != null)
                {
                    var orderItemList = GetOrderItems(request.OrderItems, order);
                    _allMarktContext.OrderItems.RemoveRange(order.OrderItems.Except(orderItemList));
                    _allMarktContext.OrderItems.AddRange(orderItemList.Except(order.OrderItems));
                }
                
                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }

        private ICollection<OrderItem> GetOrderItems(ICollection<OrderItemViewModel> orderItems, Entities.Order order)
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
