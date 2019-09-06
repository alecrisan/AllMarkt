using AllMarkt.Commands.ProductComments;
using AllMarkt.Queries.ProductComments;
using AllMarkt.Services;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Services
{
    public class ProductCommentsServiceTests
    {
        private ProductCommentsService _productCommentsService;
        private Mock<IMediator> _mockMediator;


        public ProductCommentsServiceTests()
        {
            _mockMediator = new Mock<IMediator>();
            _productCommentsService = new ProductCommentsService(_mockMediator.Object);
        }

        [Fact]
        public async Task GetAllAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _productCommentsService.GetAllAsync();

            //Assert
            _mockMediator.Verify (x => x.Send(It.IsAny<GetAllProductCommentsQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task GetProductCommentsByProductIdAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _productCommentsService.GetProductCommentsByProductIdAsync(1);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetProductCommentsByProductIdQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task SaveAsync_Calls_Mediator_ForAdd_WhenId_IsZero()
        {
            //Arrange
            var productCommentViewModel = new AllMarkt.ViewModels.ProductCommentViewModel
            {
                Rating = 3,
                Text = "test"
            };

            //Act
            await _productCommentsService.SaveAsync(productCommentViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<AddProductCommentCommand>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task SaveAsync_Calls_Mediator_ForEdit_WhenId_IsNotZero()
        {
            //Arrange
            var productCommentViewModel = new AllMarkt.ViewModels.ProductCommentViewModel
            {
                Id = 1,
                Rating = 3,
                Text = "test"
            };

            //Act
            await _productCommentsService.SaveAsync(productCommentViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<EditProductCommentCommand>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _productCommentsService.DeleteAsync(1);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<DeleteProductCommentCommand>(), default(CancellationToken)), Times.Once());
        }
    }
}
