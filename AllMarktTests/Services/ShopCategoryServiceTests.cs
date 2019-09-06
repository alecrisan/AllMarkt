using AllMarkt.Commands.ShopCategory;
using AllMarkt.Services;
using AllMarkt.ViewModels;
using MediatR;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Services
{
    public class ShopCategoryLinkServiceTests
    {
        private ShopCategoryService _shopCategoryService;
        private Mock<IMediator> _mockMediator;

        public ShopCategoryLinkServiceTests()
        {
            _mockMediator = new Mock<IMediator>();
            _shopCategoryService = new ShopCategoryService(_mockMediator.Object);
        }

        [Fact]
        public async Task SaveAsync_Calls_Mediator()
        {
            //Arrange
            var shopCategoryViewModel = new ShopCategoryViewModel
            {
                ShopId = 1,
                CategoryId = 1
            };

            //Act
            await _shopCategoryService.SaveAsync(shopCategoryViewModel);

            //Assert
            _mockMediator.Verify(x =>
            x.Send(It.IsAny<AddShopCategoryLinkCommand>(), default), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_Calls_Mediator()
        {
            //Arrange
            var shopCategoryViewModel = new ShopCategoryViewModel
            {
                ShopId = 1,
                CategoryId = 1
            };

            //Act
            await _shopCategoryService.DeleteAsync(shopCategoryViewModel);

            //Assert
            _mockMediator.Verify(x =>
            x.Send(It.IsAny<DeleteShopCategoryLinkCommand>(), default), Times.Once());
        }
    }
}
