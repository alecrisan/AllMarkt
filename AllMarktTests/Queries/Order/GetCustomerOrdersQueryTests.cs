using AllMarkt.Entities;
using AllMarkt.Queries.Orders;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.Order
{
    public class GetCustomerOrdersQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetCustomerOrdersQuery, IEnumerable<OrderViewModel>> _getCustomerOrdersQueryHandler;

        public GetCustomerOrdersQueryTests()
        {
            _getCustomerOrdersQueryHandler = new GetCustomerOrdersQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetCustomersOrderQueryHandle_Returns_Empty()
        {
            //Arrange
            int customerId = 1;

            //Act
            var result = await _getCustomerOrdersQueryHandler
                .Handle(new GetCustomerOrdersQuery() { CustomerId = customerId }, CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCustomersOrderQueryHandle_Returns_ExistingOrders()
        {
            //Arrange
            Customer buyer = new Customer();
            AllMarktContextIM.Customers.Add(buyer);
            await AllMarktContextIM.SaveChangesAsync();

            AllMarktContextIM.Orders.Add(new AllMarkt.Entities.Order()
            {
                Buyer = buyer
            });
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getCustomerOrdersQueryHandler
                .Handle(new GetCustomerOrdersQuery() { CustomerId = buyer.Id }, CancellationToken.None);

            //Assert
            result.Count().Should().Be(1);
        }
    }
}
