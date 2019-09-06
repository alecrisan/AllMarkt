using AllMarkt.Commands.PrivateMessage;
using AllMarkt.Queries.PrivateMessage;
using AllMarkt.Services;
using AllMarkt.ViewModels;
using AllMarktTests.Queries;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Services
{
    public class PrivateMessagesServiceTests : AllMarktContextTests
    {
        private readonly PrivateMessagesService _privateMessagesService;
        private readonly Mock<IMediator> _mockMediator;

        public PrivateMessagesServiceTests()
        {
            _mockMediator = new Mock<IMediator>();
            _privateMessagesService = new PrivateMessagesService(_mockMediator.Object);
        }

        [Fact]
        public async Task GetAllSentPrivateMessagesByUserAsync_WithSenderId_CallsMediator()
        {
            //Arrange

            //Act
            await _privateMessagesService.GetAllSentPrivateMessagesByUserAsync(1);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetAllSentPrivateMessagesByUserQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task GetAllReceivedPrivateMessagesByUserAsync_WithSenderId_CallsMediator()
        {
            //Arrange

            //Act
            await _privateMessagesService.GetAllReceivedPrivateMessagesByUserAsync(1);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetAllReceivedPrivateMessagesByUserQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async Task GetAllPrivateMessagesAsync_CallsMediator()
        {
            //Arrange

            //Act
            await _privateMessagesService.GetAllPrivateMessagesAsync();

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetAllPrivateMessagesQuery>(), default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task GetPrivateMessageByIdAsync_CallsMediator()
        {
            //Arrange
            int id = 1;

            //Act
            await _privateMessagesService.GetPrivateMessageByIdAsync(id);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<GetPrivateMessageByIdQuery>(), default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task SavePrivateMessageAsync_WhenIdIs0_CallsMediatorForAdd()
        {
            //Arrange
            var privateMessageViewModel = new PrivateMessageViewModel
            {
                Id = 0,
                Title = "title",
                Text = "text",
                DateSent = DateTime.Now,
                DateRead = null,
                Sender = new IdAndDisplayNameUserViewModel { Id = 1, DisplayName = "user1" },
                Receiver = new IdAndDisplayNameUserViewModel { Id = 2, DisplayName = "user2" }
            };

            //Act
            await _privateMessagesService.SavePrivateMessageAsync(privateMessageViewModel);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<AddPrivateMessageCommand>(), default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task SavePrivateMessageAsync_WhenIdIsNot0_CallsMediatorForEdit()
        {
            //Arrange
            var privateMessageViewModel = new PrivateMessageViewModel
            {
                Id = 1,
                Title = "title",
                Text = "text",
                DateSent = DateTime.Now,
                DateRead = null,
                Sender = new IdAndDisplayNameUserViewModel { Id = 1, DisplayName = "user1" },
                Receiver = new IdAndDisplayNameUserViewModel { Id = 2, DisplayName = "user2" }
            };

            //Act
            await _privateMessagesService.SavePrivateMessageAsync(privateMessageViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<EditPrivateMessageCommand>(), default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task DeletePrivateMessageAsync_CallsMediatorForDelete()
        {
            //Arrange
            int id = 1;

            //Act
            await _privateMessagesService.DeletePrivateMessageAsync(id);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<DeletePrivateMessageCommand>(), default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task UpdateOrDeletePrivateMessage_WhenDeletedBy_IsNull_CallsMediatorForEdit()
        {
            //Arrange
            var privateMessageViewModel = new PrivateMessageViewModel
            {
                Id = 1,
                Title = "title",
                Text = "text",
                DateSent = DateTime.Now,
                DateRead = DateTime.Now,
                Sender = new IdAndDisplayNameUserViewModel { Id = 1, DisplayName = "user1" },
                Receiver = new IdAndDisplayNameUserViewModel { Id = 2, DisplayName = "user2" },
                DeletedBy = null
            };

            _mockMediator
                .Setup(call => call.Send(It.IsAny<GetPrivateMessageByIdQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult(privateMessageViewModel));

            //Act
            await _privateMessagesService.UpdateOrDeletePrivateMessage(privateMessageViewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<EditPrivateMessageCommand>(), default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task UpdateOrDeletePrivateMessage_WhenDeletedBy_IsNotNull_CallsMediatorForDelete()
        {
            //Arrange
            var privateMessageViewModel = new PrivateMessageViewModel
            {
                Id = 14,
                Title = "title",
                Text = "text",
                DateSent = DateTime.Now,
                DateRead = null,
                Sender = new IdAndDisplayNameUserViewModel { Id = 1, DisplayName = "user1" },
                Receiver = new IdAndDisplayNameUserViewModel { Id = 2, DisplayName = "user2" },
                DeletedBy = AllMarkt.Entities.DeletedBy.Sender
            };
            _mockMediator
                .Setup(call => call.Send(It.IsAny<GetPrivateMessageByIdQuery>(), default(CancellationToken)))
                .Returns(Task.FromResult(privateMessageViewModel));

            var viewModel = new PrivateMessageViewModel
            {
                Id = 14,
                Title = "title",
                Text = "text",
                DateSent = DateTime.Now,
                DateRead = null,
                Sender = new IdAndDisplayNameUserViewModel { Id = 1, DisplayName = "user1" },
                Receiver = new IdAndDisplayNameUserViewModel { Id = 2, DisplayName = "user2" },
                DeletedBy = AllMarkt.Entities.DeletedBy.Receiver
            };

            //Act
            await _privateMessagesService.UpdateOrDeletePrivateMessage(viewModel);

            //Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<DeletePrivateMessageCommand>(), default(CancellationToken)), Times.Once);
        }
    }
}
