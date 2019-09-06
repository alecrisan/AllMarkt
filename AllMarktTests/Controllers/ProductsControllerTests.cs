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
    public class ProductsControllerTests
    {
        private ProductsController _productsController;
        private Mock<IProductsService> _mockProductService;
        private Mock<IClaimsGetter> _mockClaimsGetter;
        private Mock<IUsersService> _mockUserService;

        public ProductsControllerTests()
        {
            _mockClaimsGetter = new Mock<IClaimsGetter>();
            _mockUserService = new Mock<IUsersService>();
            _mockProductService = new Mock<IProductsService>();
            _productsController = new ProductsController(_mockProductService.Object, _mockClaimsGetter.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task GetAllAsync_Returns_OkResult()
        {
            //Arange

            //Act
            var result = await _productsController.GetAllAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllAsync_Calls_ProductsService()
        {
            //Arrange

            //Act
            var result = await _productsController.GetAllAsync();

            //Assert
            _mockProductService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_Returns_Products()
        {
            //Arrange
            _mockProductService
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(GetProducts()));

            //Act
            var okResult = await _productsController.GetAllAsync() as ObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeOfType<ProductViewModel[]>();


            var items = okResult.Value as IEnumerable<ProductViewModel>;
            items.Should().NotBeNull();
            items.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetProductWithRatingByCategoryAsync_Returns_OkResult()
        {
            //Arange

            //Act
            var result = await _productsController.GetProductsWithRatingByCategoryAsync(1);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetProductWithRatingByCategoryAsync_Calls_ProductsService()
        {
            //Arrange

            //Act
            var result = await _productsController.GetProductsWithRatingByCategoryAsync(1);

            //Assert
            _mockProductService.Verify(x => x.GetProductWithRatingByCategoryAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetProductWithRatingByCategoryAsync_Returns_ProductsWithRating()
        {
            //Arrange
            _mockProductService
                .Setup(x => x.GetProductWithRatingByCategoryAsync(1))
                .Returns(Task.FromResult(GetProductsWithRating()));

            //Act
            var okResult = await _productsController.GetProductsWithRatingByCategoryAsync(1) as ObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeOfType<ProductWithRatingViewModel[]>();

            var items = okResult.Value as IEnumerable<ProductWithRatingViewModel>;
            items.Should().NotBeNull();
            items.Count().Should().Be(1);

            var prod = items.FirstOrDefault();
            prod.Should().NotBeNull();
            prod.AverageRating.Should().Be(3);
        }

        [Fact]
        public async Task GetAllProductsByShopAsync_Returns_OkResult()
        {
            //Arange
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
            var result = await _productsController.GetAllProductsByShopAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllProductsByShopAsync_Calls_ProductsService()
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
            var result = await _productsController.GetAllProductsByShopAsync();

            //Assert
            _mockProductService.Verify(x => x.GetAllProductsByShopAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetAllProductsByShopAsync_Returns_ProductsWithRating()
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

            _mockProductService
                .Setup(x => x.GetAllProductsByShopAsync(1))
                .Returns(Task.FromResult(GetProductsWithRating()));

            //Act
            var okResult = await _productsController.GetAllProductsByShopAsync() as ObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeOfType<ProductWithRatingViewModel[]>();

            var items = okResult.Value as IEnumerable<ProductWithRatingViewModel>;
            items.Should().NotBeNull();
            items.Count().Should().Be(1);

            var prod = items.FirstOrDefault();
            prod.Should().NotBeNull();
            prod.AverageRating.Should().Be(3);
        }

        [Fact]
        public async Task AddAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _productsController.AddAsync(new ProductViewModel());

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task AddAsync_Calls_ProductsService()
        {
            //Arrange
            var product = new ProductViewModel();

            //Act
            await _productsController.AddAsync(product);

            //Assert
            _mockProductService.Verify(x => x.SaveAsync(product));
        }

        [Fact]
        public async Task EditAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _productsController.EditAsync(new ProductViewModel());

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task EditAsync_Calls_ProductsService()
        {
            //Arrange
            var product = new ProductViewModel();

            //Act
            await _productsController.EditAsync(product);

            //Assert
            _mockProductService.Verify(x => x.SaveAsync(product));
        }

        [Fact]
        public async Task DeleteAsync_Returns_NoContent()
        {
            //Arrange
            const int id = 1;

            //Act
            var result = await _productsController.DeleteAsync(id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsync_Calls_ProductsService()
        {
            //Arrange
            const int id = 1;

            //Act
            await _productsController.DeleteAsync(id);

            //Assert
            _mockProductService.Verify(x => x.DeleteAsync(id));
        }

        private IEnumerable<ProductViewModel> GetProducts()
        {
            IEnumerable<ProductViewModel> products = new[]
            {
                new ProductViewModel { Id = 1, Name = "Product Name 1", Description = "Desc1", Price = 10, ImageURI = "", State = true, ProductCategoryId = 1, ProductCategoryName = "categoryName"},
                new ProductViewModel { Id = 2, Name = "Product Name 2", Description = "Desc2", Price = 10, ImageURI = "", State = true, ProductCategoryId = 1, ProductCategoryName = "categoryName"}
            };

            return products;
        }

        private IEnumerable<ProductWithRatingViewModel> GetProductsWithRating()
        {
            IEnumerable<ProductWithRatingViewModel> productsWithRatings = new[]
            {
                new ProductWithRatingViewModel {
                Id = 1,
                Name = "Name",
                Description = "Description",
                Price = 0,
                State = true,
                ImageURI = null,
                ProductCategoryId = 1,
                AverageRating = 3
                }
            };
            return productsWithRatings;
        }
    }
}
