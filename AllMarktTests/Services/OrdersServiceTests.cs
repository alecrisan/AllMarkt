using AllMarkt.Commands.Order;
using AllMarkt.Queries.Orders;
using AllMarkt.Services;
using AllMarkt.ViewModels;
using MediatR;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarktTests.Services
{
    class OrdersServiceTests
    {
        private OrdersService _ordersService;
        private Mock<IMediator> _mockMediator;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _ordersService = new OrdersService(_mockMediator.Object);
        }

        [Test]
        public async Task GetAllAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _ordersService.GetAllAsync();

            //Assert
            _mockMediator.Verify(x =>
                 x.Send(It.IsAny<GetAllOrdersQuery>(), default(CancellationToken)), Times.Once());
        }

        [Test]
        public async Task GetShopOrdersAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _ordersService.GetShopOrdersAsync(1);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny <GetShopOrdersQuery>() , default(CancellationToken)), Times.Once);
        }

        [Test]
        public async Task GetCustomerOrdersAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _ordersService.GetCustomerOrdersAsync(1);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<GetCustomerOrdersQuery>(), default(CancellationToken)), Times.Once);
        }

        [Test]
        public async Task SaveAsync_Calls_Mediator_ForAdd_WhenId_IsZero()
        {
            //Arrange
            var orderViewModel = new OrderViewModel {
                ShopName = "Test Name",
                CustomerName = "Test Name",
                DeliveryAddress = "Test Address",
                AWB = "Test AWB"
            };

            //Act
            await _ordersService.SaveAsync(orderViewModel);

            //Assert
            _mockMediator.Verify(x =>
                 x.Send(It.IsAny<AddOrderCommand>(), default(CancellationToken)), Times.Once);
        }

        [Test]
        public async Task SaveAsync_Calls_Mediator_ForEdit_WhenId_IsNotZero()
        {
            //Arrange
            var orderViewModel = new OrderViewModel
            {
                Id = 1,
                ShopName = "Test Name",
                CustomerName = "Test Name",
                DeliveryAddress = "Test Address",
                AWB = "Test AWB"
            };

            //Act
            await _ordersService.SaveAsync(orderViewModel);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<EditOrderCommand>(), default(CancellationToken)), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _ordersService.DeleteAsync(1);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<DeleteOrderCommand>(), default(CancellationToken)), Times.Once);
        }
    }
}
