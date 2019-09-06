using AllMarkt.Commands.Order;
using AllMarkt.Entities;
using AllMarkt.ViewModels;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.Order
{
    public class AddOrderCommandTests :AllMarktContextTests
    {

        private readonly IRequestHandler<AddOrderCommand> _addOrderCommandHandler;

        public AddOrderCommandTests()
        {
            _addOrderCommandHandler = new AddOrderCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task AddOrderCommandHandle_AddsOrder()
        {
            //Arrange
            Shop seller = new Shop
            {
                Address = "aaaaa",
                Comments = null,
                CUI = "aaaaddd",
                IBAN = "aaaaa",
                Orders = null,
                PhoneNumber = "0123654789",
                ProductCategories = null,
                ShopCategoryLink = null,
                SocialCapital = 4
            };
            Customer buyer = new Customer();

            AllMarktContextIM.Customers.Add(buyer);
            AllMarktContextIM.Shops.Add(seller);
            await AllMarktContextIM.SaveChangesAsync();

            var addOrderCommand = new AddOrderCommand
            {
                ShopId = seller.Id,
                CustomerId = buyer.Id,
                DeliveryAddress = "Test Address",
                AWB = "Test AWB",
                OrderItems = new List<OrderItemViewModel>()
                {
                    new OrderItemViewModel
                    {
                        Id = 1,
                        Name = "Test Item",
                        Amount = 1
                    }
                }
            };
            
            //Act
            await _addOrderCommandHandler.Handle(addOrderCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Orders
                .Should()
                .Contain(order =>
                    order.Seller.Id == addOrderCommand.ShopId
                    && order.Buyer.Id == addOrderCommand.CustomerId
                    && order.DeliveryAddress == addOrderCommand.DeliveryAddress
                    && order.AWB == addOrderCommand.AWB);
        }
     
        [Fact]
        public async Task AddOrderCommandHandle_DoesntAddOrder_When_NoMatchingShop_ExistsInDatabase()
        {
            //Arrange
            Customer buyer = new Customer();

            AllMarktContextIM.Customers.Add(buyer);
            await AllMarktContextIM.SaveChangesAsync();

            var addOrderCommand = new AddOrderCommand
            {
                CustomerId = buyer.Id,
                DeliveryAddress = "Test Address",
                AWB = "Test AWB",
                OrderItems = new List<OrderItemViewModel>()
                {
                    new OrderItemViewModel
                    {
                        Id = 1,
                        Name = "Test Item",
                        Amount = 1
                    }
                }
            };

            //Act
            await _addOrderCommandHandler.Handle(addOrderCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Orders
                .Should()
                .NotContain(order =>
                    order.Seller.Id == addOrderCommand.ShopId
                    && order.Buyer.Id == addOrderCommand.CustomerId
                    && order.DeliveryAddress == addOrderCommand.DeliveryAddress
                    && order.AWB == addOrderCommand.AWB);
        }

        [Fact]
        public async Task AddOrderCommandHandle_DoesntAddOrder_When_NoMatchingCustomer_ExistsInDatabase()
        {
            //Arrange
            Shop seller = new Shop
            {
                Address = "aaaaa",
                Comments = null,
                CUI = "aaaaddd",
                IBAN = "aaaaa",
                Orders = null,
                PhoneNumber = "0123654789",
                ProductCategories = null,
                ShopCategoryLink = null,
                SocialCapital = 4
            };

            AllMarktContextIM.Shops.Add(seller);
            await AllMarktContextIM.SaveChangesAsync();

            var addOrderCommand = new AddOrderCommand
            {
                ShopId = seller.Id,
                DeliveryAddress = "Test Address",
                AWB = "Test AWB",
                OrderItems = new List<OrderItemViewModel>()
                {
                    new OrderItemViewModel
                    {
                        Id = 1,
                        Name = "Test Item",
                        Amount = 1
                    }
                }
            };

            //Act
            await _addOrderCommandHandler.Handle(addOrderCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Orders
                .Should()
                .NotContain(order =>
                    order.Seller.Id == addOrderCommand.ShopId
                    && order.Buyer.Id == addOrderCommand.CustomerId
                    && order.DeliveryAddress == addOrderCommand.DeliveryAddress
                    && order.AWB == addOrderCommand.AWB);
        }

        [Fact]
        public async Task AddOrderCommandHandler_DoesntAddOrder_When_NoOrderItems_ExistInCommand()
        {
            //Arrange
             Shop seller = new Shop
            {
                Address = "aaaaa",
                Comments = null,
                CUI = "aaaaddd",
                IBAN = "aaaaa",
                Orders = null,
                PhoneNumber = "0123654789",
                ProductCategories = null,
                ShopCategoryLink = null,
                SocialCapital = 4
            };
            Customer buyer = new Customer();

            AllMarktContextIM.Customers.Add(buyer);
            AllMarktContextIM.Shops.Add(seller);
            await AllMarktContextIM.SaveChangesAsync();

            var addOrderCommand = new AddOrderCommand
            {
                ShopId = seller.Id,
                CustomerId = buyer.Id,
                DeliveryAddress = "Test Address",
                AWB = "Test AWB", 
               
            };

            //Act
            await _addOrderCommandHandler.Handle(addOrderCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Orders
                .Should()
                .NotContain(order =>
                    order.Seller.Id == addOrderCommand.ShopId
                    && order.Buyer.Id == addOrderCommand.CustomerId
                    && order.DeliveryAddress == addOrderCommand.DeliveryAddress
                    && order.AWB == addOrderCommand.AWB);
        }
    }
}
