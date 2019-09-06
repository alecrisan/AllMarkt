using AllMarkt.Commands.User;
using AllMarkt.Entities;
using AllMarkt.Queries.Shop;
using AllMarkt.Queries.User;
using AllMarkt.Queries.User.Customer;
using AllMarkt.Queries.User.Shop;
using AllMarkt.Services;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Services
{
    public class UsersServiceTests
    {
        private UsersService _usersService;
        private Mock<IMediator> _mockMediator;
        private IOptions<AppSettings> _appSettings;
        private Mock<ITokenGenerator> _mockTokenGenerator;

        public UsersServiceTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockTokenGenerator = new Mock<ITokenGenerator>();
            _appSettings = Options.Create(new AppSettings
            {
                Secret = "STRINGU ALA BLANAOSSTRINGU ALA BLANAOS BLANAOSSTRINGU ALA BLANAOS BLANAOSSTRINGU ALA BLANAOS BLANAOSSTRINGU ALA BLANAOS",
                TokenLifetimeDays = 7
            });
            _usersService = new UsersService(_mockMediator.Object, _appSettings, _mockTokenGenerator.Object);
        }

        [Fact]
        public async Task GetAllAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _usersService.GetAllAsync();

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<GetAllUsersQuery>(), default), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _usersService.GetByIdAsync(1);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<GetUserByIdQuery>(), default), Times.Once());
        }

        [Fact]
        public async Task GetShopByIdAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _usersService.GetShopByIdAsync(1);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetShopByIdQuery>(), default), Times.Once());
        }

        [Fact]
        public async Task GetCustomerByIdAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _usersService.GetCustomerByIdAsync(1);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetCustomerByIdQuery>(), default), Times.Once());
        }

        [Fact]
        public async Task GetShopByUserIdAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _usersService.GetShopByUserIdAsync(1);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetShopByUserIdQuery>(), default), Times.Once());
        }

        [Fact]
        public async Task GetCustomerByUserIdAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _usersService.GetCustomerByUserIdAsync(1);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetCustomerByUserIdQuery>(), default), Times.Once());
        }

        [Fact]
        public async Task SaveAsync_Calls_Mediator_ForAdd()
        {
            //Arrange
            var userInputViewModel = GetUserInputViewModel();

            //Act
            await _usersService.SaveAsync(userInputViewModel);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<AddUserCommand>(), default), Times.Once());
        }

        [Fact]
        public async Task SaveAsync_Should_Send_HashedPassword()
        {
            //Arrange
            var userInputViewModel = GetUserInputViewModel();

            //Act
            await _usersService.SaveAsync(userInputViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.Is<AddUserCommand>
                (mo => mo.Password == "5dc088487fb505024591604c00eadbd8607ea049dc46857eb803b45e205640f6")
                , default), Times.Once());
        }

        [Fact]
        public async Task EditAsync_Calls_Mediator_ForEditCommand()
        {
            //Arrange
            var userEditViewModel = MakeUserEditViewModel();

            //Act
            await _usersService.EditAsync(userEditViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<EditUserCommand>(), default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _usersService.DeleteAsync(2);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<DeleteUserCommand>(), default), Times.Once());
        }

        [Fact]
        public async Task DisableUserByIdAsync_Calls_Mediator()
        {
            //Arrange
            var userEditViewModel = MakeUserEditViewModel();

            //Act
            await _usersService.DisableUserByIdAsync(userEditViewModel);

            //Assert
            _mockMediator.Verify(x =>
            x.Send(It.IsAny<DisableUserCommand>(), default), Times.Once());
        }

        [Fact]
        public async Task GetAllShopsAsync_CallsMediator()
        {
            //Arrange

            //Act
            await _usersService.GetAllShopsAsync();

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<GetAllShopsQuery>(), default), Times.Once());
        }

        [Fact]
        public async Task GetAllCustomersAsync_CallsMediator()
        {
            //Arrange

            //Act
            await _usersService.GetAllCustomersAsync();

            //Assert
            _mockMediator.Verify(x =>
            x.Send(It.IsAny<GetAllCustomersQuery>(), default), Times.Once());
        }

        [Fact]
        public async Task GetShopsByCategoryIdAsync_CallsMediator()
        {
            //Arrange

            //Act
            await _usersService.GetShopsByCategoryIdAsync(1);

            //Assert
            _mockMediator.Verify(x =>
            x.Send(It.IsAny<GetShopsByCategoryIdQuery>(), default), Times.Once);
        }

        [Fact]
        public async Task Authenticate_WithGivenEmailAndPassword_ReturnsUser()
        {
            //Arrange
            _mockTokenGenerator
                .Setup(x => x.Generate(It.IsAny<AppSettings>(), It.IsAny<UserTokenDataModel>()))
                .Returns(GetTokenString());

            _mockMediator
                .Setup(x => x.Send(It.IsAny<GetUserByEmailAndPasswordQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(GetUserGetViewModel()));

            //Act
            var user = await _usersService.Authenticate("TestEmail@yahoo.com", "1234566");

            //Assert
            user.Should().NotBeNull();
        }

        [Fact]
        public async Task Authenticate_WithGivenEmailAndPassword_WithDisableStatus_ReturnsNull()
        {
            //Arrange
            _mockTokenGenerator
                .Setup(x => x.Generate(It.IsAny<AppSettings>(), It.IsAny<UserTokenDataModel>()))
                .Returns(GetTokenString());

            _mockMediator
                .Setup(x => x.Send(It.IsAny<GetUserByEmailAndPasswordQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(GetUserGetViewModel(false)));

            //Act
            var user = await _usersService.Authenticate("TestEmail@yahoo.com", "1234566");

            //Assert
            user.Should().BeNull();
        }

        [Fact]
        public async Task Authenticate_WithGivenEmailAndPassword_ReturnsNull()
        {
            //Arrange
            _mockMediator
                .Setup(x => x.Send(new GetUserByEmailAndPasswordQuery(), CancellationToken.None))
                .Returns(Task.FromResult<UserGetViewModel>(null));

            //Act
            var user = await _usersService.Authenticate("TestEmail@yahoo.com", "wrongpassword");

            //Assert
            user.Should().BeNull();
        }

        [Fact]
        public async Task Authenticate_WithGivenEmailAndPassword_CallsMediator()
        {
            //Arrange
            _mockMediator
                .Setup(x => x.Send(It.IsAny<GetUserByEmailAndPasswordQuery>(), CancellationToken.None))
                .Returns(Task.FromResult(GetUserGetViewModel()));

            //Act
            await _usersService.Authenticate("TestEmail@yahoo.com", "1234566");

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<GetUserByEmailAndPasswordQuery>(), default), Times.Once());
        }

        [Fact]
        public async Task EditCustomerAsync_Calls_Mediator_ForEditCustomerCommand()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel
            {
                Address = "address",
                UserId = 1,
                UserDisplayName = "user1",
                PhoneNumber = "0123654799",
                Id = 1
            };

            //Act
            await _usersService.EditCustomerAsync(customerViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<EditCustomerCommand>(), default), Times.Once);
        }

        [Fact]
        public async Task EditShopAsync_Calls_Mediator_ForEditShopCommand()
        {
            //Arrange
            var shopViewModel = new ShopViewModel
            {
                Id = 1,
                Address = "address",
                CUI = "cui",
                IBAN = "SE3550000000054910000003",
                PhoneNumber = "0123654789",
                SocialCapital = 3,
                UserDisplayName = "user1",
                UserId = 1
            };

            //Act
            await _usersService.EditShopAsync(shopViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<EditShopCommand>(), default), Times.Once);
        }



        [Fact]
        public async Task RegisterAsync_Calls_Mediator_ForAdd()
        {
            //Arrange
            var userInputViewModel = GetUserInputViewModel();

            //Act
            await _usersService.RegisterAsync(userInputViewModel);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<RegisterUserCommand>(), default), Times.Once());
        }

        [Fact]
        public async Task RegisterAsync_Should_Send_HashedPassword()
        {
            //Arrange
            var userInputViewModel = GetUserInputViewModel();

            //Act
            await _usersService.RegisterAsync(userInputViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.Is<RegisterUserCommand>
                (mo => mo.Password == "5dc088487fb505024591604c00eadbd8607ea049dc46857eb803b45e205640f6")
                , default), Times.Once());
        }

        private UserInputViewModel GetUserInputViewModel()
        {
            var userInputViewModel = new UserInputViewModel
            {
                Email = "TestEmail@yahoo.com",
                Password = "1234566",
                DisplayName = "UserTest",
                UserRole = "testRole"
            };
            return userInputViewModel;
        }

        private UserGetViewModel GetUserGetViewModel(bool change = true)
        {
            var userGetViewModel = new UserGetViewModel
            {
                Email = "test@test.com",
                Password = "123456",
                DisplayName = "testttt",
                UserRole = UserRole.Admin.ToString(),
                IsEnabled = change
            };
            return userGetViewModel;
        }

        private UserEditViewModel MakeUserEditViewModel()
        {
            var userEditViewModel = new UserEditViewModel
            {
                Email = "TestEmail@yahoo.com",
                OldPassword = "1234566",
                NewPassword = "1234567",
                DisplayName = "UserTest",
                UserRole = "testRole"
            };
            return userEditViewModel;
        }

        private string GetTokenString()
        {
            return "tokenString";
        }
    }
}



