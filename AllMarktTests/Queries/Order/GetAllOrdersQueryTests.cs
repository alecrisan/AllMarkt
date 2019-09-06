using AllMarkt.Entities;
using AllMarkt.Queries.Orders;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace AllMarktTests.Queries.Order
{
    public class GetAllOrdersQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderViewModel>> _getAllOrdersQueryHandler;

        public GetAllOrdersQueryTests()
        {
            _getAllOrdersQueryHandler = new GetAllOrdersQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllOrdersQueryHandle_Returns_Empty()
        {
            //Arrange

            //Act
            var result = await _getAllOrdersQueryHandler.Handle(new GetAllOrdersQuery(), CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllOrdersQueryHandler_Returns_ExistingOrders()
        {
            //Arrange
            AllMarkt.Entities.Order order = new AllMarkt.Entities.Order();
            AllMarktContextIM.Orders.Add(order);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getAllOrdersQueryHandler.Handle(new GetAllOrdersQuery(), CancellationToken.None);

            //Assert
            result.Count().Should().Be(1);
        }


    }
}
