using AllMarkt.Commands.Product;
using AllMarkt.Queries.Product;
using AllMarkt.Services;
using AllMarkt.ViewModels;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Services
{
    public class ProductsServiceTests
    {
        private ProductsService _productsService;
        private Mock<IMediator> _mockMediator;

        public ProductsServiceTests()
        {
            _mockMediator = new Mock<IMediator>();
            _productsService = new ProductsService(_mockMediator.Object);
        }

        [Fact]
        public async Task GetAllAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _productsService.GetAllAsync();

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<GetAllProductsQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task GetProductWithRatingByCategoryAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _productsService.GetProductWithRatingByCategoryAsync(1);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<GetProductsWithRatingByCategoryIdQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task GetAllProductsByShopAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _productsService.GetAllProductsByShopAsync(1);

            //Assert
            _mockMediator.Verify(x =>
            x.Send(It.IsAny<GetAllProductsByShopQuery>(), default), Times.Once);
        }

        [Fact]
        public async Task SaveAsync_Calls_Mediator_ForAdd_WhenId_IsZero()
        {
            //Arrange
            var productViewModel = new ProductViewModel
            {
                Name = "Test Name1",
                Description = "Test Description1",
                Price = 10,
                ImageURI = "",
                State = true,
                ProductCategoryId = 1,
                ProductCategoryName = "category"
            };

            //Act
            await _productsService.SaveAsync(productViewModel);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<AddProductCommand>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task SaveAsync_Calls_Mediator_ForEdit_WhenId_IsNotZero()
        {
            //Arrange
            var productViewModel = new ProductViewModel
            {
                Id = 1,
                Name = "Test Name1",
                Description = "Test Description1",
                Price = 10,
                ImageURI = "",
                State = true,
                ProductCategoryId = 1,
                ProductCategoryName = "category"
            };

            //Act
            await _productsService.SaveAsync(productViewModel);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<EditProductCommand>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _productsService.DeleteAsync(1);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<DeleteProductCommand>(), default(CancellationToken)), Times.Once());
        }
    }
}
