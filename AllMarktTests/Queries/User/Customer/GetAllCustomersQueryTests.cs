using AllMarkt.Queries.User.Customer;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.User.Customer
{
    public class GetAllCustomersQueryTests :AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerViewModel>>_getAllCustomersQueryHandler;

        public GetAllCustomersQueryTests()
        {
            _getAllCustomersQueryHandler = new GetAllCustomersQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllCustomersQueryHandler_isEmptyAsync()
        {
            //Arrange

            //Act
            var customers = await _getAllCustomersQueryHandler.Handle(new GetAllCustomersQuery(),CancellationToken.None);

            //Assert
            customers.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllCustomersQueryHandler_ReturnsCustomers()
        {
            //Arrange
            var user = new AllMarkt.Entities.User
            {
                Email = "email@asd.com",
                DisplayName = "tets"
            };
            AllMarktContextIM.Users.Add(user);
            await AllMarktContextIM.SaveChangesAsync();

            AllMarktContextIM.Customers.Add(new AllMarkt.Entities.Customer
            {
                User = user,
                UserId = user.Id,
                Address = "address",
                PhoneNumber = "0123456789",         
            });

            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var customers = await _getAllCustomersQueryHandler.Handle(new GetAllCustomersQuery(), CancellationToken.None);

            //assert
            customers.Count().Should().Be(1);

        }
    }
}
