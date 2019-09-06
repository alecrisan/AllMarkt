using AllMarkt.Controller;
using AllMarkt.Services.Interfaces;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Controllers
{
    public class PrivateMessagesControllerTests
    {
        private readonly PrivateMessagesController _privateMessagesController;
        private readonly Mock<IPrivateMessagesService> _mockPrivateMessagesService;
        private readonly Mock<IClaimsGetter> _mockClaimsGetter;

        public PrivateMessagesControllerTests()
        {
            _mockPrivateMessagesService = new Mock<IPrivateMessagesService>();
            _mockClaimsGetter = new Mock<IClaimsGetter>();
            _privateMessagesController = new PrivateMessagesController(_mockPrivateMessagesService.Object, _mockClaimsGetter.Object);
        }

        [Fact]
        public async Task GetAllSentPrivateMessagesByUserAsync_WithGivenId_ReturnsOk()
        {
            //Arrange
            _mockClaimsGetter
                .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
                .Returns(1);

            //Act
            var result = await _privateMessagesController.GetAllSentPrivateMessagesByUserAsync();

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllSentPrivateMessagesByUserAsync_WithGivenId_CallsPrivateMessagesService()
        {
            //Arrange
            _mockClaimsGetter
                .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
                .Returns(1);

            //Act
            var result = await _privateMessagesController.GetAllSentPrivateMessagesByUserAsync();

            //Assert
            _mockPrivateMessagesService.Verify(x => x.GetAllSentPrivateMessagesByUserAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAllSentPrivateMessagesByUserAsync_WithGivenId_ReturnsPrivateMessagesAsync()
        {
            //Arrange
            int givenId = 1;
            _mockPrivateMessagesService
                .Setup(x => x.GetAllSentPrivateMessagesByUserAsync(givenId))
                .Returns(Task.FromResult(GetPrivateMessages()));
            _mockClaimsGetter
                .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
                .Returns(givenId);

            //Act
            var okResult = await _privateMessagesController.GetAllSentPrivateMessagesByUserAsync() as ObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeOfType<PrivateMessageViewModel[]>();

            var items = okResult.Value as IEnumerable<PrivateMessageViewModel>;
            items.Should().NotBeNull();

            items.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetAllReceivedPrivateMessagesByUserAsync_WithGivenId_ReturnsOk()
        {
            //Arrange
            _mockClaimsGetter
                .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
                .Returns(1);

            //Act
            var result = await _privateMessagesController.GetAllReceivedPrivateMessagesByUserAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllReceivedPrivateMessagesByUserAsync_WithGivenId_CallsPrivateMessagesService()
        {
            //Arrange
            _mockClaimsGetter
                .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
                .Returns(1);

            //Act
            var result = await _privateMessagesController.GetAllReceivedPrivateMessagesByUserAsync();

            //Assert
            _mockPrivateMessagesService.Verify(x => x.GetAllReceivedPrivateMessagesByUserAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAllReceivedPrivateMessagesByUserAsync_WithGivenId_ReturnsPrivateMessagesAsync()
        {
            //Arrange
            int givenId = 2;
            _mockPrivateMessagesService
                .Setup(x => x.GetAllReceivedPrivateMessagesByUserAsync(givenId))
                .Returns(Task.FromResult(GetPrivateMessages()));
            _mockClaimsGetter
                .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
                .Returns(givenId);

            //Act
            var okResult = await _privateMessagesController.GetAllReceivedPrivateMessagesByUserAsync() as ObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeOfType<PrivateMessageViewModel[]>();

            var items = okResult.Value as IEnumerable<PrivateMessageViewModel>;
            items.Should().NotBeNull();

            items.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetAllPrivateMessagesAsync_WithGivenId_ReturnsOk()
        {
            //Arrange

            //Act
            var result = await _privateMessagesController.GetAllPrivateMessagesAsync();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllPrivateMessagesAsync_WithGivenId_CallsPrivateMessagesService()
        {
            //Arrange

            //Act
            var result = await _privateMessagesController.GetAllPrivateMessagesAsync();

            //Assert
            _mockPrivateMessagesService.Verify(x => x.GetAllPrivateMessagesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllPrivateMessagesAsync_WithGivenId_ReturnsPrivateMessagesAsync()
        {
            //Arrange
            _mockPrivateMessagesService
                .Setup(x => x.GetAllPrivateMessagesAsync())
                .Returns(Task.FromResult(GetPrivateMessages()));

            //Act
            var okResult = await _privateMessagesController.GetAllPrivateMessagesAsync() as ObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeOfType<PrivateMessageViewModel[]>();

            var items = okResult.Value as IEnumerable<PrivateMessageViewModel>;
            items.Should().NotBeNull();

            items.Count().Should().Be(2);
        }

        [Fact]
        public async Task AddPrivateMessageAsync_WithGivenPrivateMessage_CallsPrivateMessagesService()
        {
            //Arrange
            var privateMessage = new PrivateMessageViewModel
            {
                Title = "Titlu",
                Text = "Text",
                Sender = new IdAndDisplayNameUserViewModel(),
                Receiver = new IdAndDisplayNameUserViewModel(),
                DateSent = DateTime.Now
            };
            _mockClaimsGetter
                .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
                .Returns(privateMessage.Sender.Id);

            //Act
            await _privateMessagesController.AddPrivateMessageAsync(privateMessage);

            //Asert
            _mockPrivateMessagesService.Verify(x => x.SavePrivateMessageAsync(privateMessage), Times.Once);
        }

        [Fact]
        public async Task AddPrivateMessageAsync_WithGivenPrivateMessage_ReturnsNoContent()
        {
            //Arrange
            var privateMessage = new PrivateMessageViewModel
            {
                Title = "Titlu",
                Text = "Text",
                Sender = new IdAndDisplayNameUserViewModel(),
                Receiver = new IdAndDisplayNameUserViewModel(),
                DateSent = DateTime.Now
            };
            _mockClaimsGetter
                .Setup(x => x.UserId(It.IsAny<IEnumerable<Claim>>()))
                .Returns(privateMessage.Sender.Id);

            //Act
            var result = await _privateMessagesController.AddPrivateMessageAsync(privateMessage);

            //Asert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeletePrivateMessageAsync_WithGivenId_CallsPrivateMessagesService()
        {
            //Arrange
            int id = 1;

            //Act
            var result = await _privateMessagesController.DeletePrivateMessageAsync(id);

            _mockPrivateMessagesService.Verify(x => x.DeletePrivateMessageAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeletePrivateMessageAsync_WithGivenId_ReturnsNoContent()
        {
            //Arrange
            int id = 1;

            //Act
            var result = await _privateMessagesController.DeletePrivateMessageAsync(id);

            //Asert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task EditPrivateMessageAsync_WithGivenPrivateMessage_CallsPrivateMessagesService()
        {
            //Arrange
            var privateMessage = new PrivateMessageViewModel();

            //Act
            await _privateMessagesController.EditPrivateMessageAsync(privateMessage);

            //Asert
            _mockPrivateMessagesService.Verify(x => x.SavePrivateMessageAsync(privateMessage), Times.Once);
        }

        [Fact]
        public async Task EditPrivateMessageAsync_WithGivenPrivateMessage_ReturnsNoContent()
        {
            //Arrange
            var privateMessage = new PrivateMessageViewModel();

            //Act
            await _privateMessagesController.EditPrivateMessageAsync(privateMessage);

            //Asert
            _mockPrivateMessagesService.Verify(x => x.SavePrivateMessageAsync(privateMessage), Times.Once);
        }

        private IEnumerable<PrivateMessageViewModel> GetPrivateMessages()
        {
            IEnumerable<PrivateMessageViewModel> privateMessages = new[]
            {
                new PrivateMessageViewModel
                {
                    Id = 1,
                    Title = "title1",
                    Text = "text1",
                    DateSent = DateTime.ParseExact("2019-08-06 09:45", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    DateRead = DateTime.ParseExact("2019-08-06 09:45", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    Sender = new IdAndDisplayNameUserViewModel() { Id = 1, DisplayName = "user1" },
                    Receiver = new IdAndDisplayNameUserViewModel() { Id = 2, DisplayName = "user2" }
                },
                new PrivateMessageViewModel
                {
                    Id = 2,
                    Title = "title2",
                    Text = "text2",
                    DateSent = DateTime.ParseExact("2019-08-07 14:09", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    DateRead = DateTime.ParseExact("2019-08-07 14:09", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    Sender = new IdAndDisplayNameUserViewModel() { Id = 1, DisplayName = "user1" },
                    Receiver = new IdAndDisplayNameUserViewModel() { Id = 1, DisplayName = "user1" }
                }
            };

            return privateMessages;
        }

        [Fact]
        public async Task UpdatePrivateMessageStatusAsync_WithGivenPrivateMessage_CallsPrivateMessagesService()
        {
            //Arrange
            var privateMessage = new PrivateMessageViewModel();

            //Act
            await _privateMessagesController.UpdatePrivateMessageStatusAsync(privateMessage);

            //Asert
            _mockPrivateMessagesService.Verify(x => x.UpdateOrDeletePrivateMessage(privateMessage), Times.Once);
        }

        [Fact]
        public async Task UpdatePrivateMessageStatusAsync_WithGivenPrivateMessage_ReturnsNoContent()
        {
            //Arrange
            var privateMessage = new PrivateMessageViewModel();

            //Act
            var result = await _privateMessagesController.UpdatePrivateMessageStatusAsync(privateMessage);

            //Asert
            result.Should().BeOfType<NoContentResult>();
        }

    }
}