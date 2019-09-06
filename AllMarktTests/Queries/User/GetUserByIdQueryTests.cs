using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Entities;
using AllMarkt.Queries.User;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AllMarktTests.Queries.User
{
    public class GetUserByIdQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetUserByIdQuery, UserGetViewModel> _getUserByIdQueryHandler;

        public GetUserByIdQueryTests()
        {
            _getUserByIdQueryHandler = new GetUserByIdQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetUserByIdQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getUserByIdQueryHandler.Handle(new GetUserByIdQuery(), CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserByIdQueryHandler_ReturnsExisting_UserById()
        {
            //Arrange
            var user = new AllMarkt.Entities.User
            {
                Email = "Email1@yahoo.com",
                Password = "123",
                DisplayName = "name1",
                UserRole = UserRole.Shop
            };

            await AllMarktContextIM.Users.AddAsync(user);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getUserByIdQueryHandler.Handle(new GetUserByIdQuery { Id = user.Id }, CancellationToken.None);

            //Assert
            result.Email.Should().Be("Email1@yahoo.com");
            result.DisplayName.Should().Be("name1");
            result.UserRole.Should().Be(nameof(UserRole.Shop));
        }
    }
}
