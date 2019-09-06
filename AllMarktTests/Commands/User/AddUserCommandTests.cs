using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Commands.User;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AllMarktTests.Commands.User
{
    public class AddUserCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<AddUserCommand> _addUserCommandHandler;

        public AddUserCommandTests()
        {
            _addUserCommandHandler = new AddUserCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task AddUserCommandHandler_AddsUser_With_Role_Admin()
        {
            //Arrange.
            var addUserCommand = CreateAddUserCommand();
            addUserCommand.UserRole = "Admin";

            //Act
            await _addUserCommandHandler.Handle(addUserCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Users.Should()
                .Contain(user =>
                user.Email == addUserCommand.Email
                && user.Password == addUserCommand.Password
                && user.DisplayName == addUserCommand.DisplayName);

            AllMarktContextIM.Admins.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddUserCommandHandler_AddsUser_With_Role_Moderator()
        {
            //Arrange.
            var addUserCommand = CreateAddUserCommand();
            addUserCommand.UserRole = "Moderator";

            //Act
            await _addUserCommandHandler.Handle(addUserCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Users.Should()
                .Contain(user =>
                user.Email == addUserCommand.Email
                && user.Password == addUserCommand.Password
                && user.DisplayName == addUserCommand.DisplayName);

            AllMarktContextIM.Moderators.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddUserCommandHandler_AddsUser_With_Role_Shop()
        {
            //Arrange.
            var addUserCommand = CreateAddUserCommand();
            addUserCommand.UserRole = "Shop";

            //Act
            await _addUserCommandHandler.Handle(addUserCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Users.Should()
                .Contain(user =>
                user.Email == addUserCommand.Email
                && user.Password == addUserCommand.Password
                && user.DisplayName == addUserCommand.DisplayName);

            AllMarktContextIM.Shops.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddUserCommandHandler_AddsUser_With_Role_Customer()
        {
            //Arrange.
            var addUserCommand = CreateAddUserCommand();
            addUserCommand.UserRole = "Customer";

            //Act
            await _addUserCommandHandler.Handle(addUserCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Users.Should()
                .Contain(user =>
                user.Email == addUserCommand.Email
                && user.Password == addUserCommand.Password
                && user.DisplayName == addUserCommand.DisplayName);

            AllMarktContextIM.Customers.Should().HaveCount(1);
        }

        private AddUserCommand CreateAddUserCommand()
        {
           return new AddUserCommand
            {
                Email = "TestEmail@yahoo.com",
                Password = "123456",
                DisplayName = "TestDisplayName"
            };
        }
    }
}
