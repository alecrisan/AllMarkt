using AllMarkt.Commands.ShopComments;
using AllMarkt.Queries.ShopComments;
using AllMarkt.Services;
using MediatR;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarktTests.Services
{
    class ShopCommentsServiceTests
    {
        private ShopCommentsService _shopCommentsService;
        private Mock<IMediator> _mockMediator;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _shopCommentsService = new ShopCommentsService(_mockMediator.Object);
        }

        [Test]
        public async Task GetAllAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _shopCommentsService.GetAllAsync();

            //Assert
            _mockMediator.Verify (x => x.Send(It.IsAny<GetAllShopCommentsQuery>(), default(CancellationToken)), Times.Once());
        }

        [Test]
        public async Task SaveAsync_Calls_Mediator_ForAdd_WhenId_IsZero()
        {
            //Arrange
            var shopCommentViewModel = new AllMarkt.ViewModels.ShopCommentViewModel
            {
                Rating = 3,
                Text = "test"
            };

            //Act
            await _shopCommentsService.SaveAsync(shopCommentViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<AddShopCommentCommand>(), default(CancellationToken)), Times.Once());
        }

        [Test]
        public async Task SaveAsync_Calls_Mediator_ForEdit_WhenId_IsNotZero()
        {
            //Arrange
            var shopCommentViewModel = new AllMarkt.ViewModels.ShopCommentViewModel
            {
                Id = 1,
                Rating = 3,
                Text = "test"
            };

            //Act
            await _shopCommentsService.SaveAsync(shopCommentViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<EditShopCommentCommand>(), default(CancellationToken)), Times.Once());
        }

        [Test]
        public async Task DeleteAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            await _shopCommentsService.DeleteAsync(1);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<DeleteShopCommentCommand>(), default(CancellationToken)), Times.Once());
        }
    }
}
