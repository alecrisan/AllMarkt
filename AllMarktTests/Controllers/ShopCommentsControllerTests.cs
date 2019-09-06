using AllMarkt.Controller;
using AllMarkt.Services.Interfaces;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllMarktTests.Controllers
{
    class ShopCommentsControllerTests
    {
        private ShopCommentsController _shopCommentsController;
        private Mock<IShopCommentsService> _mockShopCommentsService;
        private Mock<IClaimsGetter> _mockClaimsGetter;

        [SetUp]
        public void Setup()
        {
            _mockShopCommentsService = new Mock<IShopCommentsService>();
            _mockClaimsGetter = new Mock<IClaimsGetter>();
            _shopCommentsController = new ShopCommentsController(_mockShopCommentsService.Object, _mockClaimsGetter.Object);
        }

        [Test]
        public async Task GetShopCommentsByShopIdAsync_Returns_OkResult()
        {
            //Arrange

            //Act
            var result = await _shopCommentsController.GetShopCommentsByShopIdAsync(1);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetShopCommentsByShopIdAsync_Calls_ShopCommentsService()
        {
            //Arrange

            //Act
            var result = await _shopCommentsController.GetShopCommentsByShopIdAsync(1);

            //Assert
            _mockShopCommentsService.Verify(x => x.GetShopCommentsByShopId(1), Times.Once);
        }

        [Test]
        public async Task GetShopCommentsByShopIdAsync_Returns_ShopComments()
        {
            //Arrange
            _mockShopCommentsService
                .Setup(x => x.GetShopCommentsByShopId(1))
                .Returns(Task.FromResult(GetShopComments()));

            //Act
            var okResult = await _shopCommentsController.GetShopCommentsByShopIdAsync(1) as ObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<IEnumerable<ShopCommentViewModel>>(okResult.Value);

            var items = okResult.Value as IEnumerable<ShopCommentViewModel>;
            Assert.IsNotNull(items);

            Assert.AreEqual(2, items.Count());
        }

        [Test]
        public async Task GetAllAsync_Returns_OkResult()
        {
            //Arange

            //Act
            var result = await _shopCommentsController.GetAllAsync();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllAsync_Calls_ShopCommentsService()
        {
            //Arrange

            //Act
            var result = await _shopCommentsController.GetAllAsync();

            //Assert
            _mockShopCommentsService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_Returns_ShopComments()
        {
            //Arrange
            _mockShopCommentsService
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(GetShopComments()));

            //Act
            var okResult = await _shopCommentsController.GetAllAsync() as ObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<IEnumerable<ShopCommentViewModel>>(okResult.Value);

            var items = okResult.Value as IEnumerable<ShopCommentViewModel>;
            Assert.IsNotNull(items);

            Assert.AreEqual(2, items.Count());
        }

        [Test]
        public async Task AddAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _shopCommentsController.AddAsync(new ShopCommentViewModel());

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddAsync_Calls_ShopCommentsService()
        {
            //Arrange
            var category = new ShopCommentViewModel();

            //Act
            await _shopCommentsController.AddAsync(category);

            //Assert
            _mockShopCommentsService.Verify(x => x.SaveAsync(category));
        }

        [Test]
        public async Task EditAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _shopCommentsController.EditAsync(new ShopCommentViewModel());

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task EditAsync_Calls_ShopCommentsService()
        {
            //Arrange
            var shopComment = new ShopCommentViewModel();

            //Act
            await _shopCommentsController.EditAsync(shopComment);

            //Assert
            _mockShopCommentsService.Verify(x => x.SaveAsync(shopComment));
        }

        [Test]
        public async Task DeleteAsync_Returns_NoContent()
        {
            //Arrange
            const int id = 1;

            //Act
            var result = await _shopCommentsController.DeleteAsync(id);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteAsync_Calls_ShopCommentsService()
        {
            //Arrange
            const int id = 1;

            //Act
            await _shopCommentsController.DeleteAsync(id);

            //Assert
            _mockShopCommentsService.Verify(x => x.DeleteAsync(id));
        }

        private IEnumerable<ShopCommentViewModel> GetShopComments()
        {
            IEnumerable<ShopCommentViewModel> shopComments = new[]
            {
                new ShopCommentViewModel { Id = 1, Rating = 5, Text = "Super bun", AddedById = 1 },
                new ShopCommentViewModel { Id = 2, Rating = 3, Text = "Nu chiar super bun dar decent", AddedById = 1 }
            };
            return shopComments;
        }
    }
}
