using AllMarkt.Commands.Order;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.Order
{
    public class DeleteOrderCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<DeleteOrderCommand> _deleteOrderCommandHandler;

        public DeleteOrderCommandTests()
        {
            _deleteOrderCommandHandler = new DeleteOrderCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task DeleteOrderCommandHandler_DeletesExistingOrder()
        {
            //Arrange
            var order = new AllMarkt.Entities.Order
            {
                DeliveryAddress = "Test Address",
                AWB = "Test AWB"
            };
            AllMarktContextIM.Orders.Add(order);
            await AllMarktContextIM.SaveChangesAsync();
            var existingOrder = AllMarktContextIM.Orders.First();
            var deleteOrderCommand = new DeleteOrderCommand
            {
                Id = existingOrder.Id
            };

            //Act
            await _deleteOrderCommandHandler.Handle(deleteOrderCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Orders
                .Should().NotContain(x => x.Id == existingOrder.Id);
        }
    }
}
