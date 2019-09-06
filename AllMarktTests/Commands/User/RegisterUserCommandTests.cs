using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Commands.User;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AllMarktTests.Commands.User
{
    public class RegisterUserCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<RegisterUserCommand> _registerUserCommandHandler;

        public RegisterUserCommandTests()
        {
            _registerUserCommandHandler = new RegisterUserCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task RegisterUserCommandHandler_AddsUser_With_Role_Shop()
        {
            //Arrange.
            var registerUserCommand = RegisterUserCommand();
            registerUserCommand.UserRole = "Shop";

            //Act
            await _registerUserCommandHandler.Handle(registerUserCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Users.Should()
                .Contain(user =>
                user.Email == registerUserCommand.Email
                && user.Password == registerUserCommand.Password
                && user.DisplayName == registerUserCommand.DisplayName);

            AllMarktContextIM.Shops.Should().HaveCount(1);
        }

        [Fact]
        public async Task RegisterUserCommandHandler_AddsUser_With_Role_Customer()
        {
            //Arrange.
            var registerUserCommand = RegisterUserCommand();
            registerUserCommand.UserRole = "Customer";

            //Act
            await _registerUserCommandHandler.Handle(registerUserCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Users.Should()
                .Contain(user =>
                user.Email == registerUserCommand.Email
                && user.Password == registerUserCommand.Password
                && user.DisplayName == registerUserCommand.DisplayName);

            AllMarktContextIM.Customers.Should().HaveCount(1);
        }

        private RegisterUserCommand RegisterUserCommand()
        {
           return new RegisterUserCommand
            {
                Email = "TestEmail@yahoo.com",
                Password = "123456",
                DisplayName = "TestDisplayName"
            };
        }
    }
}
