using AllMarkt.Controller;
using AllMarkt.Services.Interfaces;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Controllers
{
    public class UsersControllerTests
    {
        private UsersController _usersController;
        private Mock<IUsersService> _mockUserService;
        private Mock<IClaimsGetter> _mockClaimsGetter;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUsersService>();
            _mockClaimsGetter = new Mock<IClaimsGetter>();
            _mockClaimsGetter = new Mock<IClaimsGetter>();
            _usersController = new UsersController(_mockUserService.Object, _mockClaimsGetter.Object);
        }

        [Fact]
        public async Task GetAllAsync_Returns_OkObjectResult()
        {
            //Arrange

            //Act
            var result = await _usersController.GetAllAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllAsyns_Calls_UsersService()
        {
            //Arrange

            //Act
            await _usersController.GetAllAsync();

            //Assert
            _mockUserService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_Returns_Users()
        {
            //Arrange
            IEnumerable<UserGetViewModel> users = new[]
            {
                new UserGetViewModel { Email = "email1@yahoo.com", DisplayName = "name1", UserRole = "Customer" },
                new UserGetViewModel { Email = "email2@yahoo.com", DisplayName = "name2", UserRole = "Customer" },
                new UserGetViewModel { Email = "email3@yahoo.com", DisplayName = "name3", UserRole = "Customer" }
            };

            _mockUserService
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(users));

            //Act
            var result = await _usersController.GetAllAsync() as ObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var items = result.Value as IEnumerable<UserGetViewModel>;
            items.Should().NotBeNull();
            items.Count().Should().Be(3);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_OkObjectResult()
        {
            //Arrange

            //Act
            var result = await _usersController.GetByIdAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetByIdAsync_Calls_UsersService()
        {
            //Arrange
            int userId = 1;
            _mockClaimsGetter
            .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
            .Returns(userId);

            //Act
            await _usersController.GetByIdAsync();

            //Assert
            _mockUserService.Verify(x => x.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_UserById()
        {
            //Arrange
            UserGetViewModel user = new UserGetViewModel
            {
                Email = "email1@yahoo.com",
                DisplayName = "name1",
                UserRole = "Customer"
            };

            _mockUserService
                .Setup(x => x.GetByIdAsync(1))
                .Returns(Task.FromResult(user));

            //Act
            var result = await _usersController.GetByIdAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().Equals(user);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_Returns_OkObjectResult()
        {
            //Arrange

            //Act
            var result = await _usersController.GetCustomerByIdAsync(1);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetCustomerByIdAsync_Calls_UsersService()
        {
            //Arrange

            //Act
            await _usersController.GetCustomerByIdAsync(1);

            //Assert
            _mockUserService.Verify(x => x.GetCustomerByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_Returns_CustomerById()
        {
            //Arrange
            UserGetViewModel user = new UserGetViewModel
            {
                Email = "email1@yahoo.com",
                DisplayName = "name1",
                UserRole = "Customer"
            };

            CustomerViewModel customer = new CustomerViewModel
            {
                Address = "address",
                PhoneNumber = "0123654789",
                UserDisplayName = user.DisplayName,
                UserId = user.Id
            };

            _mockUserService
                .Setup(x => x.GetCustomerByIdAsync(1))
                .Returns(Task.FromResult(customer));

            //Act
            var result = await _usersController.GetCustomerByIdAsync(1);

            //Assert
            result.Should().NotBeNull();
            result.Should().Equals(customer);
        }

        [Fact]
        public async Task GetShopByIdAsync_Returns_OkObjectResult()
        {
            //Arrange

            //Act
            var result = await _usersController.GetShopByIdAsync(1);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetShopByIdAsync_Calls_UsersService()
        {
            //Arrange

            //Act
            await _usersController.GetShopByIdAsync(1);

            //Assert
            _mockUserService.Verify(x => x.GetShopByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetShopByIdAsync_Returns_ShopById()
        {
            //Arrange
            UserGetViewModel user = new UserGetViewModel
            {
                Email = "email1@yahoo.com",
                DisplayName = "name1",
                UserRole = "Customer"
            };

            ShopViewModel shop = new ShopViewModel
            {
                Address = "address",
                PhoneNumber = "0123654789",
                UserDisplayName = user.DisplayName,
                UserId = user.Id,
                CUI = "5555",
                IBAN = "SE0000000000000000000000",
                SocialCapital = 12
            };

            _mockUserService
                .Setup(x => x.GetShopByIdAsync(1))
                .Returns(Task.FromResult(shop));

            //Act
            var result = await _usersController.GetShopByIdAsync(1);

            //Assert
            result.Should().NotBeNull();
            result.Should().Equals(shop);
        }

        [Fact]
        public async Task GetCustomerByUserIdAsync_Returns_OkObjectResult()
        {
            //Arrange

            //Act
            var result = await _usersController.GetCustomerByUserIdAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetCustomerByUserIdAsync_Calls_UsersService()
        {
            //Arrange
            int userId = 1;
            _mockClaimsGetter
            .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
            .Returns(userId);

            //Act
            await _usersController.GetCustomerByUserIdAsync();

            //Assert
            _mockUserService.Verify(x => x.GetCustomerByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetCustomerByUserIdAsync_Returns_CustomerById()
        {
            //Arrange
            UserGetViewModel user = new UserGetViewModel
            {
                Email = "email1@yahoo.com",
                DisplayName = "name1",
                UserRole = "Customer"
            };

            CustomerViewModel customer = new CustomerViewModel
            {
                Address = "address",
                PhoneNumber = "0123654789",
                UserDisplayName = user.DisplayName,
                UserId = user.Id,
            };

            _mockUserService
                .Setup(x => x.GetCustomerByUserIdAsync(1))
                .Returns(Task.FromResult(customer));

            //Act
            var result = await _usersController.GetCustomerByUserIdAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().Equals(customer);
        }

        [Fact]
        public async Task GetShopByUserIdAsync_Returns_OkObjectResult()
        {
            //Arrange
            int userId = 1;
            _mockClaimsGetter
                .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
                .Returns(userId);

            //Act
            var result = await _usersController.GetShopByUserIdAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetShopByUserIdAsync_Calls_UsersService()
        {
            //Arrange
            int userId = 1;
            _mockClaimsGetter
            .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
            .Returns(userId);

            //Act
            await _usersController.GetShopByUserIdAsync();

            //Assert
            _mockUserService.Verify(x => x.GetShopByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetShopByUserIdAsync_Returns_ShopById()
        {
            //Arrange
            UserGetViewModel user = new UserGetViewModel
            {
                Email = "email1@yahoo.com",
                DisplayName = "name1",
                UserRole = "Customer"
            };

            ShopViewModel shop = new ShopViewModel
            {
                Address = "address",
                PhoneNumber = "0123654789",
                UserDisplayName = user.DisplayName,
                UserId = user.Id,
                CUI = "5555",
                IBAN = "SE0000000000000000000000",
                SocialCapital = 12
            };

            _mockUserService
                .Setup(x => x.GetShopByUserIdAsync(1))
                .Returns(Task.FromResult(shop));
            int userId = 1;
            _mockClaimsGetter
                .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
                .Returns(userId);

            //Act
            var result = await _usersController.GetShopByUserIdAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().Equals(shop);
        }

        [Fact]
        public async Task AddAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _usersController.AddAsync(new UserInputViewModel());

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task AddAsync_Calls_UsersService()
        {
            //Arrange
            var user = new UserInputViewModel();

            //Act
            await _usersController.AddAsync(user);

            //Assert
            _mockUserService.Verify(x => x.SaveAsync(user));
        }

        [Fact]
        public async Task EditAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _usersController.EditAsync(new UserEditViewModel());

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task EditAsync_Calls_UsersService()
        {
            //Arrange
            var user = new UserEditViewModel();

            //Act
            await _usersController.EditAsync(user);

            //Assert
            _mockUserService.Verify(x => x.EditAsync(user));
        }

        [Fact]
        public async Task DeleteAsync_Returns_NoContent()
        {
            //Arrange
            int id = 1;

            //Act
            var result = await _usersController.DeleteAsync(id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsyns_Calls_UsersService()
        {
            //Arrange
            int id = 1;

            //Act
            await _usersController.DeleteAsync(id);

            //Assert
            _mockUserService.Verify(x => x.DeleteAsync(id));
        }

        [Fact]
        public async Task DisableUserByIdAsync_Returns_NoContent()
        {
            //Arrange
            var user = new UserEditViewModel();

            //Act
            var result = await _usersController.DisableUserByIdAsync(user);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DisableUserByIdAsync_Calls_UsersService()
        {
            //Arrange
            var user = new UserEditViewModel();

            //Act
            await _usersController.DisableUserByIdAsync(user);

            //Assert
            _mockUserService.Verify(x => x.DisableUserByIdAsync(It.IsAny<UserEditViewModel>()));
        }

        [Fact]
        public async Task GetAllShopsAsync_Returns_OkObjectResult()
        {
            //Arrange

            //Act
            var result = await _usersController.GetAllShopsAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllShopsAsyns_Calls_UsersService()
        {
            //Arrange

            //Act
            await _usersController.GetAllShopsAsync();

            //Assert
            _mockUserService.Verify(x => x.GetAllShopsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllShopsAsync_Returns_Users()
        {
            //Arrange
            IEnumerable<ShopViewModel> shops = new[]
            {
                new ShopViewModel {
                    Address = "address 1",
                    CUI = "RO123654987C",
                    IBAN = "010101010101010101010101",
                    UserDisplayName = "Shop 1",
                    PhoneNumber = "0123654789",
                    SocialCapital = 3,
                    UserId = 1
                }
            };

            _mockUserService
                .Setup(x => x.GetAllShopsAsync())
                .Returns(Task.FromResult(shops));

            //Act
            var result = await _usersController.GetAllShopsAsync() as ObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var items = result.Value as IEnumerable<ShopViewModel>;
            items.Should().NotBeNull();
            items.Count().Should().Be(1);
        }

        [Fact]
        public async Task EditShopAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _usersController.EditShopAsync(new ShopViewModel());

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task EditShopAsync_Calls_UsersService()
        {
            //Arrange
            var shop = new ShopViewModel();

            //Act
            await _usersController.EditShopAsync(shop);

            //Assert
            _mockUserService.Verify(x => x.EditShopAsync(shop));
        }

        [Fact]
        public async Task GetAllCustomersAsync_Returns_OkObjectResult()
        {
            //Arrange

            //Act
            var result = await _usersController.GetAllCustomersAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllCustomersAsync_Calls_UserService()
        {
            //Arrange

            //Act
            var result = await _usersController.GetAllCustomersAsync();

            //Assert
            _mockUserService.Verify(x => x.GetAllCustomersAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllCustomersAsync_Returns_Users()
        {
            //Arrange
            IEnumerable<CustomerViewModel> customers = new[]
            {
                new CustomerViewModel {
                    Address = "address 1",
                    UserDisplayName = "Shop 1",
                    PhoneNumber = "0123654789",
                    UserId = 1
                }
            };

            _mockUserService
                .Setup(x => x.GetAllCustomersAsync())
                .Returns(Task.FromResult(customers));

            //Act
            var result = await _usersController.GetAllCustomersAsync() as ObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var items = result.Value as IEnumerable<CustomerViewModel>;
            items.Should().NotBeNull();
            items.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetShopsByCategoryIdAsync_Returns_OkObjectResult()
        {
            //Arrange

            //Act
            var result = await _usersController.GetShopsByCategoryIdAsync(1);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetShopsByCategoryIdAsync_Calls_UserService()
        {
            //Arrange

            //Act
            var result = await _usersController.GetShopsByCategoryIdAsync(1);

            //Assert
            _mockUserService.Verify(x => x.GetShopsByCategoryIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetShopsByCategoryIdAsync_Returns_Shops()
        {
            //Arrange
            IEnumerable<ShopViewModel> shops = new[]
          {
                new ShopViewModel {
                    Address = "address 1",
                    CUI = "RO123654987C",
                    IBAN = "010101010101010101010101",
                    UserDisplayName = "Shop 1",
                    PhoneNumber = "0123654789",
                    SocialCapital = 3,
                    UserId = 1
                }
            };
            _mockUserService
                .Setup(x => x.GetShopsByCategoryIdAsync(1))
                .Returns(Task.FromResult(shops));

            //Act
            var result = await _usersController.GetShopsByCategoryIdAsync(1) as ObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var items = result.Value as IEnumerable<ShopViewModel>;
            items.Should().NotBeNull();
            items.Count().Should().Be(1);
        }

        [Fact]
        public async Task Authenticate_WithGivenViewModel_Returns_OkObjectResult()
        {
            //Arrange
            var user = new UserLoginResponseViewModel
            {
                Email = "TestEmail@yahoo.com",
                DisplayName = "UserTest"
            };

            _mockUserService
                .Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            //Act
            var result = await _usersController.Authenticate(new UserLoginRequestViewModel()) as ObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Authenticate_With_Wrong_GivenViewModel_Returns_BadRequest()
        {
            //Arrange
            _mockUserService
                .Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<UserLoginResponseViewModel>(null));

            //Act
            var result = await _usersController.Authenticate(new UserLoginRequestViewModel()) as ObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Authenticate_WithGivenViewModel_Calls_UserService()
        {
            //Arrange
            var user = new UserLoginRequestViewModel
            {
                Email = "test@test.com",
                Password = AllMarkt.Tools.Hash.ComputeSha256Hash("123456")
            };

            //Act
            var result = await _usersController.Authenticate(user);

            //Assert
            _mockUserService.Verify(x => x.Authenticate(user.Email, user.Password), Times.Once);
        }

        [Fact]
        public async Task EditCustomerAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _usersController.EditCustomerAsync(new CustomerViewModel());

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task EditCustomerAsync_Calls_UsersService()
        {
            //Arrange
            var customer = new CustomerViewModel();

            //Act
            await _usersController.EditCustomerAsync(customer);

            //Assert
            _mockUserService.Verify(x => x.EditCustomerAsync(customer));
        }

        [Fact]
        public async Task RegisterAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _usersController.RegisterAsync(new UserInputViewModel());

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task RegisterAsync_Calls_UsersService()
        {
            //Arrange
            var user = new UserInputViewModel();

            //Act
            await _usersController.RegisterAsync(user);

            //Assert
            _mockUserService.Verify(x => x.RegisterAsync(user));
        }

        [Fact]
        public async Task GetMyData_Returns_OkObjectResult()
        {
            //Arrange
            _mockClaimsGetter
               .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
               .Returns(1);

            //Act
            var result = await _usersController.GetMyData();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetMyDataAsShop_Returns_OkObjectResult()
        {
            //Arrange
            _mockClaimsGetter
               .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
               .Returns(1);

            //Act
            var result = await _usersController.GetMyDataAsShop();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetMyDataAsCustomer_Returns_OkObjectResult()
        {
            //Arrange
            _mockClaimsGetter
               .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
               .Returns(1);

            //Act
            var result = await _usersController.GetMyDataAsCustomer();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}