using AllMarkt.Commands.Order;
using AllMarktTests.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AllMarkt.Entities;
using System.Linq;
using System.Threading;
using FluentAssertions;
using AllMarkt.ViewModels;

namespace AllMarktTests.Commands.Order
{
    public class EditOrderCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<EditOrderCommand> _editOrderCommandHandler;

        public EditOrderCommandTests()
        {
            _editOrderCommandHandler = new EditOrderCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task EditOrderCommandHandler_UpdatesExistingOrder()
        {
            //Arrange
            Shop seller = new Shop();
            Customer buyer = new Customer();
            AllMarktContextIM.Shops.Add(seller);
            AllMarktContextIM.Customers.Add(buyer);
            await AllMarktContextIM.SaveChangesAsync();

            var order = new AllMarkt.Entities.Order
            {
                Seller = seller,
                Buyer= buyer,
                AdditionalNotes = "Test Notes",
                DeliveryAddress = "Test Address",
                DeliveryPhoneNumber = "01234567890",
                TotalPrice = 1,
                TimeOfOrder = DateTime.Now,
                OrderStatus = Status.Registered,
                AWB = "Test AWB",
            };

            OrderItem orderItem = new OrderItem
            {
                Order = order
            };

            order.OrderItems = new List<OrderItem> { orderItem };

            AllMarktContextIM.Orders.Add(order);
            AllMarktContextIM.OrderItems.Add(orderItem);
            await AllMarktContextIM.SaveChangesAsync();

            var existingOrder = AllMarktContextIM.Orders.Where(o=>o.Id == order.Id).FirstOrDefault();

            var editOrderCommand = new EditOrderCommand
            {
                Id = existingOrder.Id,
                AdditionalNotes = "Test Notes",
                DeliveryAddress = "Edit Address",
                DeliveryPhoneNumber = "0123456789",
                TotalPrice = 1,
                OrderStatus = Status.Registered,
                AWB = "Edit AWB"
            };


            //Act
            await _editOrderCommandHandler.Handle(editOrderCommand, CancellationToken.None);

            //
            AllMarktContextIM.Orders.Should().Contain(x =>
                x.Id == editOrderCommand.Id);

            order.DeliveryAddress.Should().Be(editOrderCommand.DeliveryAddress);
            order.AWB.Should().Be(editOrderCommand.AWB);
        }
    }
}
