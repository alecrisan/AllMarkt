using AllMarkt.Entities;
using AllMarkt.Queries.User.Customer;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.User
{
    public class GetCustomerByIdQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetCustomerByIdQuery, CustomerViewModel> _getCustomerByIdQueryHandler;

        public GetCustomerByIdQueryTests()
        {
            _getCustomerByIdQueryHandler = new GetCustomerByIdQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetCustomerByIdQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getCustomerByIdQueryHandler.Handle(new GetCustomerByIdQuery(), CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCustomerByIdQueryHandler_ReturnsExistingAddress_CustomerById()
        {
            //Arrange
            var user = new AllMarkt.Entities.User
            {
                Email = "Email1@yahoo.com",
                Password = "123",
                DisplayName = "name1",
                UserRole = UserRole.Shop
            };
            var customer = new AllMarkt.Entities.Customer
            {
                Address = "address1",
                PhoneNumber = "0123456789",
                Orders = null,
                User = user,
                UserId = user.Id
            };

            await AllMarktContextIM.Users.AddAsync(user);
            await AllMarktContextIM.Customers.AddAsync(customer);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getCustomerByIdQueryHandler.Handle(new GetCustomerByIdQuery { Id = customer.Id }, CancellationToken.None);

            //Assert
            result.Address.Should().Be(customer.Address);
        }

        [Fact]
        public async Task GetShopByIdQueryHandler_ReturnsExistingPhoneNumber_CustomerById()
        {
            //Arrange
            var user = new AllMarkt.Entities.User
            {
                Email = "Email1@yahoo.com",
                Password = "123",
                DisplayName = "name1",
                UserRole = UserRole.Shop
            };
            var customer = new AllMarkt.Entities.Customer
            {
                Address = "address1",
                PhoneNumber = "0123456789",
                Orders = null,
                User = user,
                UserId = user.Id
            };

            await AllMarktContextIM.Users.AddAsync(user);
            await AllMarktContextIM.Customers.AddAsync(customer);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getCustomerByIdQueryHandler.Handle(new GetCustomerByIdQuery { Id = customer.Id }, CancellationToken.None);

            //Assert
            result.PhoneNumber.Should().Be(customer.PhoneNumber);
        }
    }
}
