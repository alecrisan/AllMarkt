using AllMarkt.Commands.ProductCategory;
using AllMarkt.Queries.ProductCategory;
using AllMarkt.Services;
using AllMarkt.ViewModels;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Services
{
    public class ProductCategoriesServiceTests
    {
        private ProductCategoriesService _productCategoriesService;
        private Mock<IMediator> _mockMediator;

        public ProductCategoriesServiceTests() { 
            _mockMediator = new Mock<IMediator>();
            _productCategoriesService = new ProductCategoriesService(_mockMediator.Object);
        }

        [Fact]
        public async Task GetAllAsync_CallsMediator()
        {
            //Arrange
            var productCategoryViewModel = new ProductCategoryViewModel
            {
                Description = "Desc",
                Name = "Name",
                ShopId = 1,
                ShopName = "Shop"
            };

            //Act
            await _productCategoriesService.GetAllAsync();

            //assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetAllProductCategoriesQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task GetAllByShopAsync_CallsMediator()
        {
            //Arrange
            var productCategoryViewModel = new ProductCategoryViewModel
            {
                Description = "Desc",
                Name = "Name",
                ShopId = 1,
                ShopName = "Shop"
            };

            //Act
            await _productCategoriesService.GetAllByShopIdAsync(productCategoryViewModel.ShopId);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetAllProductCategoriesByShopQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task SaveAsync_CallsMediator_ForAdd_WhenId_IsZero()
        {
            //Arrange
            var productCategoryViewModel = new ProductCategoryViewModel
            {
                Description = "Desc",
                Name = "Name",
                ShopId = 1,
                ShopName = "Shop"
            };

            //Act
            await _productCategoriesService.SaveAsync(productCategoryViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<AddProductCategoryCommand>(), default(CancellationToken)), Times.Once());
        }
        [Fact]
        public async Task SaveAsync_CallsMediator_ForEdit_WhenId_IsNotZero()
        {
            var productCategoryViewModel = new ProductCategoryViewModel
            {
                Id = 1,
                Description = "Desc",
                Name = "Name",
                ShopId = 1,
                ShopName = "ShopName"
            };

            //Act
            await _productCategoriesService.SaveAsync(productCategoryViewModel);

            //Assert
            _mockMediator.Verify(x=>x.Send(It.IsAny<EditProductCategoryCommand>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_CallsMediator_ForDelete()
        {
            //Arrange

            //Act
            await _productCategoriesService.DeleteAsync(1);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<DeleteProductCategoryCommand>(), default(CancellationToken)), Times.Once());
        }
    }
}
