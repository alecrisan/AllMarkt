using AllMarkt.Controller;
using AllMarkt.Services.Interfaces;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Controllers
{
    public class ShopCategoryControllerTests
    {
        public ShopCategoryController _shopCategoryController;
        public Mock<IShopCategoryService> _mockShopCategoryService;
        public Mock<IClaimsGetter> _mockClaimsGetter;
        public Mock<IUsersService> _mockUserService;

        public ShopCategoryControllerTests()
        {
            _mockClaimsGetter = new Mock<IClaimsGetter>();
            _mockUserService = new Mock<IUsersService>();
            _mockShopCategoryService = new Mock<IShopCategoryService>();
            _shopCategoryController = new ShopCategoryController(_mockShopCategoryService.Object, _mockClaimsGetter.Object, _mockUserService.Object);

        }

        [Fact]
        public async Task AddAsync_Returns_NoContent()
        {
            //Arrange
            int userId = 1;
            int categoryId = 1;

            _mockClaimsGetter
            .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
            .Returns(userId);

            var shop = new ShopViewModel
            {
                Id = userId
            };

            _mockUserService
              .Setup(x => x.GetShopByUserIdAsync(It.IsAny<int>()))
              .Returns(Task.FromResult(shop));

            //Act
            var result = await _shopCategoryController.AddAsync(categoryId);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task AddAsync_Calls_ShopCategoryService()
        {
            //Arrange
            int userId = 1;
            int categoryId = 1;

            _mockClaimsGetter
            .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
            .Returns(userId);

            var shop = new ShopViewModel
            {
                Id = userId
            };

            _mockUserService
              .Setup(x => x.GetShopByUserIdAsync(It.IsAny<int>()))
              .Returns(Task.FromResult(shop));

            //Act
            await _shopCategoryController.AddAsync(categoryId);

            //Assert
            _mockShopCategoryService.Verify(x => x.SaveAsync(It.IsAny<ShopCategoryViewModel>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Returns_NoContent()
        {
            //Arrange
            int userId = 1;
            int categoryId = 1;

            _mockClaimsGetter
            .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
            .Returns(userId);

            var shop = new ShopViewModel
            {
                Id = userId
            };

            _mockUserService
              .Setup(x => x.GetShopByUserIdAsync(It.IsAny<int>()))
              .Returns(Task.FromResult(shop));

            //Act
            var result = await _shopCategoryController.DeleteAsync(categoryId);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsync_Calls_ShopCategoryService()
        {
            //Arrange
            int userId = 1;
            int categoryId = 1;

            _mockClaimsGetter
            .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
            .Returns(userId);

            var shop = new ShopViewModel
            {
                Id = userId
            };

            _mockUserService
              .Setup(x => x.GetShopByUserIdAsync(It.IsAny<int>()))
              .Returns(Task.FromResult(shop));

            //Act
            await _shopCategoryController.DeleteAsync(categoryId);

            //Assert
            _mockShopCategoryService.Verify(x => x.DeleteAsync(It.IsAny<ShopCategoryViewModel>()), Times.Once);
        }
    }
}
