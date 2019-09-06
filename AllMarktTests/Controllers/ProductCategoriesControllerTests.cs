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
    public class ProductCategoriesControllerTests
    {
        private ProductCategoriesController _productCategoriesController;
        private Mock<IProductCategoriesService> _mockProductCategoriesService;
        private Mock<IClaimsGetter> _mockClaimsGetter;
        private Mock<IUsersService> _mockUserService;

        public ProductCategoriesControllerTests() 
        {
            _mockProductCategoriesService = new Mock<IProductCategoriesService>();
            _mockClaimsGetter = new Mock<IClaimsGetter>();
            _mockUserService = new Mock<IUsersService>();
            _productCategoriesController = new ProductCategoriesController(_mockProductCategoriesService.Object, _mockClaimsGetter.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task GetAllAsync_Returns_OkResult()
        {
            //Arrange

            //Act
            var result = await _productCategoriesController.GetAllAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllAsync_Calls_ProductCategoriesService()
        {
            //Arrange

            //Act
            var result = await _productCategoriesController.GetAllAsync();

            //Assert
            _mockProductCategoriesService.Verify(x => x.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetAllByShopAsync_Returns_OkResult()
        {
            //Arrange
            int userId = 1;

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
            var result = await _productCategoriesController.GetAllByShopIdAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllByShopAsync_Calls_ProductCategoriesService()
        {
            //Arrange
            int userId = 1;

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
            var result = await _productCategoriesController.GetAllByShopIdAsync();

            //Assert
            _mockProductCategoriesService.Verify(x => x.GetAllByShopIdAsync(userId), Times.Once());
        }

        [Fact]
        public  async Task AddAsync_Returns_NoContentResult()
        {
            //Arrange
            int userId = 1;

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
            var result =  await _productCategoriesController.AddAsync(new ProductCategoryViewModel());

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task AddAsync_Calls_ProductCategoriesService()
        {
            //Arrange
            var productCategory = new ProductCategoryViewModel();

            int userId = 1;

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
            var result = await _productCategoriesController.AddAsync(productCategory);

            //Assert
            _mockProductCategoriesService.Verify(x => x.SaveAsync(productCategory), Times.Once());
        }

        [Fact]
        public async Task EditAsync_Returns_NoContent()
        {
            //Arrange
            var productCategory = new ProductCategoryViewModel();

            //Act
            var result = await _productCategoriesController.EditAsync(productCategory);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task EditAsync_Calls_ProductCategoriesService()
        {
            //Arrange
            var productCategory = new ProductCategoryViewModel();

            //Act
            var result = await _productCategoriesController.EditAsync(productCategory);

            //Assert
            _mockProductCategoriesService.Verify(x => x.SaveAsync(productCategory), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_Returns_NoContent()
        {
            //Arrange
            int id = 1;

            //Act
            var result = await _productCategoriesController.DeleteAsync(id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsync_Calls_ProductCategoriesService()
        {
            //Arrange
            int id = 1;

            //Act
            var result = await _productCategoriesController.DeleteAsync(id);

            //Assert
            _mockProductCategoriesService.Verify(x => x.DeleteAsync(id), Times.Once());
        }
    }
}
