using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Entities;
using AllMarkt.Queries.User;
using AllMarkt.Queries.User.Customer;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AllMarktTests.Queries.User
{
    public class GetCustomerByUserIdQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetCustomerByUserIdQuery, CustomerViewModel> _getCustomerByUserIdQueryHandler;

        public GetCustomerByUserIdQueryTests()
        {
            _getCustomerByUserIdQueryHandler = new GetCustomerByUserIdQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetCustomerByUserIdQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getCustomerByUserIdQueryHandler.Handle(new GetCustomerByUserIdQuery(), CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCustomerByUserIdQueryHandler_ReturnsExisting_UserById()
        {
            //Arrange
            var user1 = new AllMarkt.Entities.User
            {
                Email = "Email1@yahoo.com",
                Password = "123",
                DisplayName = "name1",
                UserRole = UserRole.Customer
            };
            var customer = new AllMarkt.Entities.Customer
            {
                Address = "address1",
                PhoneNumber = "0123654789",
                UserId = user1.Id,
                User = user1,
                Orders = null
            };

            await AllMarktContextIM.Users.AddAsync(user1);
            await AllMarktContextIM.Customers.AddAsync(customer);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getCustomerByUserIdQueryHandler.Handle(new GetCustomerByUserIdQuery { Id = user1.Id }, CancellationToken.None);

            //Assert
            result.UserDisplayName.Should().Be("name1");
            result.Address.Should().Be("address1");
            result.PhoneNumber.Should().Be("0123654789");
        }
    }
}
