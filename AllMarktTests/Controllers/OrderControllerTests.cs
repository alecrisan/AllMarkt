
using AllMarkt.Controller;
using AllMarkt.Services.Interfaces;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AllMarktTests.Controllers
{
    class OrderControllerTests
    {
        private OrdersController _ordersController;
        private Mock<IOrdersService> _mockOrdersService;
        private Mock<IUsersService> _mockUsersService;
        private  Mock<IClaimsGetter> _mockClaimsGetter;

        [SetUp]
        public void Setup()
        {
            _mockOrdersService = new Mock<IOrdersService>();
            _mockUsersService = new Mock<IUsersService>();
            _mockClaimsGetter = new Mock<IClaimsGetter>();
            _ordersController = new OrdersController(_mockOrdersService.Object, _mockClaimsGetter.Object, _mockUsersService.Object);
        }

        [Test]
        public async Task GetAllAsync_Returns_OkResult()
        {
            //Arrange

            //Act
            var result = await _ordersController.GetAllAsync();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllAsync_Calls_OrdersService()
        {
            //Arrange

            //Act
            var result = await _ordersController.GetAllAsync();

            //Assert
            _mockOrdersService.Verify(x => x.GetAllAsync(), Times.Once);
        }
        [Test]
        public async Task GetAllAsync_Returns_Orders()
        {
            //Arrange
            _mockOrdersService
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(GetOrders(1, 1)));

            //Act
            var okResult = await _ordersController.GetAllAsync() as ObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<IEnumerable<OrderViewModel>>(okResult.Value);

            var items = okResult.Value as IEnumerable<OrderViewModel>;
            Assert.IsNotNull(items);
            Assert.AreEqual(1, items.Count());

        }

        [Test]
        public async Task GetShopOrdersAsync_Returns_OkResult()
        {
            //Arrange
            var shop = new ShopViewModel
            {
                Id = 1
            };

            _mockClaimsGetter
               .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
               .Returns(1);
            _mockUsersService.Setup(x => x.GetShopByUserIdAsync(It.IsAny<int>())).Returns(Task.FromResult(shop));

            //Act
            var orders = await _ordersController.GetShopOrdersAsync();

            //Assert
            orders.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task GetShopOrdersAsync_Calls_OrdersService()
        {
            //Arrange
            var shop = new ShopViewModel
            {
                Id = 1
            };
            _mockClaimsGetter
               .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
               .Returns(1);
            _mockUsersService.Setup(x => x.GetShopByUserIdAsync(It.IsAny<int>())).Returns(Task.FromResult(shop));


            //Act
            var orders = await _ordersController.GetShopOrdersAsync();

            //Assert
            _mockOrdersService.Verify(x => x.GetShopOrdersAsync(1), Times.Once);
        }

        [Test]
        public async Task GetShopOrdersAsync_Returns_Orders_WithCorrectShopId()
        {
            //Arrange
            var shop = new ShopViewModel
            {
                Id = 1
            };

            _mockClaimsGetter
               .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
               .Returns(1);
            _mockUsersService.Setup(x => x.GetShopByUserIdAsync(It.IsAny<int>())).Returns(Task.FromResult(shop));
            _mockOrdersService
          .Setup(x => x.GetShopOrdersAsync(shop.Id))
          .Returns(Task.FromResult(GetOrders(1, shop.Id)));
           
            //Act
            var okResult =await _ordersController.GetShopOrdersAsync() as ObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeOfType<OrderViewModel[]>();

            var items = okResult.Value as IEnumerable<OrderViewModel>;
            items.Should().NotBeNull();
            items.Should().HaveCount(1);
            items.First().ShopId.Should().Be(shop.Id);
        }

        [Test]
        public async Task GetCustomerOrdersAsync_Returns_Orders_WithCorrectCustomerId()
        {
            //Arrange
            var customer = new CustomerViewModel
            {
                Id = 1
            };

            _mockClaimsGetter
               .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
               .Returns(1);
            _mockUsersService.Setup(x => x.GetCustomerByUserIdAsync(It.IsAny<int>())).Returns(Task.FromResult(customer));
            _mockOrdersService
          .Setup(x => x.GetCustomerOrdersAsync(customer.Id))
          .Returns(Task.FromResult(GetOrders(1, customer.Id)));

            //Act
            var okResult = await _ordersController.GetCustomerOrdersAsync() as ObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeOfType<OrderViewModel[]>();

            var items = okResult.Value as IEnumerable<OrderViewModel>;
            items.Should().NotBeNull();
            items.Should().HaveCount(1);
            items.First().CustomerId.Should().Be(customer.Id);
        }

        [Test]
        public async Task GetCustomerOrdersAsync_Returns_OkResult()
        {
            //Arrange
            var customer= new CustomerViewModel
            {
                Id = 1
            };

            _mockClaimsGetter
               .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
               .Returns(1);
            _mockUsersService.Setup(x => x.GetCustomerByUserIdAsync(It.IsAny<int>())).Returns(Task.FromResult(customer));

            //Act
            var orders = await _ordersController.GetCustomerOrdersAsync();

            //Assert
            orders.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task GetCustomerOrdersAsync_Calls_OrdersService()
        {
            //Arrange
            var customer = new CustomerViewModel
            {
                Id = 1
            };

            _mockClaimsGetter
               .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
               .Returns(1);
            _mockUsersService.Setup(x => x.GetCustomerByUserIdAsync(It.IsAny<int>())).Returns(Task.FromResult(customer));

            //Act
            var orders = await _ordersController.GetCustomerOrdersAsync();

            //Assert
            _mockOrdersService.Verify(x => x.GetCustomerOrdersAsync(1), Times.Once);

        }

        [Test]
        public async Task AddAsync_Calls_OrdersService()
        {
            //Arrange
            var orderViewModel = new OrderViewModel
            {
                ShopName = "Test Name",
                CustomerName = "Test Name",
                DeliveryAddress = "Test Address",
                AWB = "Test AWB"
            };

            //Act
            var result = await _ordersController.AddAsync(orderViewModel);

            //Assert
            _mockOrdersService.Verify(x => x.SaveAsync(orderViewModel), Times.Once);

        }

        [Test]
        public async Task AddAsync_Returns_NoContent()
        {
            //Arrange
            var orderViewModel = new OrderViewModel
            {
                ShopName = "Test Name",
                CustomerName = "Test Name",
                DeliveryAddress = "Test Address",
                AWB = "Test AWB"
            };

            //Act
            var result = await _ordersController.AddAsync(orderViewModel);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task EditAsync_Calls_OrdersService()
        {
            //Arrange
            var orderViewModel = new OrderViewModel
            {
                ShopName = "Test Name",
                CustomerName = "Test Name",
                DeliveryAddress = "Test Address",
                AWB = "Test AWB"
            };

            //Act
            var result = await _ordersController.EditAsync(orderViewModel);

            //Assert
            _mockOrdersService.Verify(x =>
                x.SaveAsync(orderViewModel), Times.Once);
        }

        [Test]
        public async Task EditAsync_Returns_NoContent()
        {
            //Arrange
            var orderViewModel = new OrderViewModel
            {
                ShopName = "Test Name",
                CustomerName = "Test Name",
                DeliveryAddress = "Test Address",
                AWB = "Test AWB"
            };

            //Act
            var result = await _ordersController.EditAsync(orderViewModel);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteAsync_Calls_OrdersService()
        {
            //Arrange

            //Act
            var result = await _ordersController.DeleteAsync(1);

            //Assert
            _mockOrdersService.Verify(x => x.DeleteAsync(1), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _ordersController.DeleteAsync(1);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        private IEnumerable<OrderViewModel> GetOrders(int shopId, int customerId)
        {
            IEnumerable<OrderViewModel> orders = new[]
            {
                new OrderViewModel{Id =1, ShopId = shopId, ShopName = "Shop1",CustomerId=customerId,CustomerName="Customer1",
                                    DeliveryAddress = "Address1",DeliveryPhoneNumber="0123456789",TotalPrice =1,
                                    TimeOfOrder = DateTime.UtcNow, AdditionalNotes = "Notes1",OrderStatus = AllMarkt.Entities.Status.Registered,
                                    AWB = "AWB1"
                }
            };

            return orders;
        }

    }
}