using AllMarkt.Controller;
using AllMarkt.Services.Interfaces;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Controllers
{
    public class ProductCommentsControllerTests
    {
        private ProductCommentsController _productCommentsController;
        private Mock<IProductCommentsService> _mockProductCommentsService;
        private readonly Mock<IClaimsGetter> _mockClaimsGetter;

        public ProductCommentsControllerTests()
        {
            _mockProductCommentsService = new Mock<IProductCommentsService>();
            _mockClaimsGetter = new Mock<IClaimsGetter>();
            _productCommentsController = new ProductCommentsController(_mockProductCommentsService.Object, _mockClaimsGetter.Object);
        }

        [Fact]
        public async Task GetProductCommentsByProductIdAsync_Returns_OkResult()
        {
            //Arrange

            //Act
            var result = await _productCommentsController.GetProductCommentsByProductIdAsync(1);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetProductCommentsByProductIdAsync_Calls_ProductCommentsService()
        {
            //Arrange

            //Act
            var result = await _productCommentsController.GetProductCommentsByProductIdAsync(1);

            //Assert
            _mockProductCommentsService.Verify(x => x.GetProductCommentsByProductIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetProductCommentsByProductIdAsync_Returns_ProductComments()
        {
            //Arrange
            _mockProductCommentsService
                .Setup(x => x.GetProductCommentsByProductIdAsync(1))
                .Returns(Task.FromResult(GetProductComments()));

            //Act
            var okResult = await _productCommentsController.GetProductCommentsByProductIdAsync(1) as ObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeOfType<ProductCommentViewModel[]>();

            var items = okResult.Value as IEnumerable<ProductCommentViewModel>;
            items.Should().NotBeNull();
            items.Count().Should().Equals(2);
        }

        [Fact]
        public async Task GetAllAsync_Returns_OkResult()
        {
            //Arrange

            //Act
            var result = await _productCommentsController.GetAllAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllAsync_Returns_ProductComments()
        {
            //Arrange
            _mockProductCommentsService
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(GetProductComments()));

            //Act
            var okResult = await _productCommentsController.GetAllAsync() as ObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeOfType<ProductCommentViewModel[]>();

            var items = okResult.Value as IEnumerable<ProductCommentViewModel>;
            items.Should().NotBeNull();
            items.Count().Should().Equals(2);
        }

        [Fact]
        public async Task GetAllAsync_Calls_ProductCommentsService()
        {
            //Arrange

            //Act
            var result = await _productCommentsController.GetAllAsync();

            //Assert
            _mockProductCommentsService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _productCommentsController.AddAsync(new ProductCommentViewModel());

            //Asert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task AddAsync_Calls_ProductCommentsService()
        {
            //Arrange
            var productComment = new ProductCommentViewModel();

            //Act
            await _productCommentsController.AddAsync(productComment);

            //Assert
            _mockProductCommentsService.Verify(x => x.SaveAsync(productComment));
        }

        [Fact]
        public async Task EditAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = await _productCommentsController.EditAsync(new ProductCommentViewModel());

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task EditAsync_Calls_ProductCommentsService()
        {
            //Arrange
            var productComment = new ProductCommentViewModel();

            //Act
            await _productCommentsController.EditAsync(productComment);

            //Assert
            _mockProductCommentsService.Verify(x => x.SaveAsync(productComment));
        }

        [Fact]
        public async Task DeleteAsync_Returns_NoContent()
        {
            //Arrange
            const int id = 1;

            //Act
            var result = await _productCommentsController.DeleteAsync(id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsync_Calls_ProductCommentsService()
        {
            //Arrange
            const int id = 1;

            //Act
            await _productCommentsController.DeleteAsync(id);

            //Assert
            _mockProductCommentsService.Verify(x => x.DeleteAsync(id));
        }

        private IEnumerable<ProductCommentViewModel> GetProductComments()
        {
            IEnumerable<ProductCommentViewModel> productComments = new[]
            {
                new ProductCommentViewModel { Id = 1, Rating = 2, Text = "a ajuns stricat", AddedById = 1},
                new ProductCommentViewModel { Id = 2, Rating = 5, Text = "nu, nu a ajuns stricat", AddedById = 2 }
            };
            return productComments;
        }
    }
}
