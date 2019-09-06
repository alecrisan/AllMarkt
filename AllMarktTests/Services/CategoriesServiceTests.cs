using System.Threading;
using AllMarkt.Commands.Category;
using AllMarkt.Queries.Category;
using AllMarkt.Services;
using AllMarkt.ViewModels;
using MediatR;
using Moq;
using NUnit.Framework;

namespace AllMarktTests.Services
{
    public class CategoriesServiceTests
    {
        private CategoriesService _categoriesService;
        private Mock<IMediator> _mockMediator;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _categoriesService = new CategoriesService(_mockMediator.Object);
        }

        [Test]
        public void GetAllAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            _categoriesService.GetAllAsync();

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<GetAllCategoriesQuery>(), default(CancellationToken)), Times.Once());
        }

        [Test]
        public void SaveAsync_Calls_Mediator_ForAdd_WhenId_IsZero()
        {
            //Arrange
            var categoryViewModel = new CategoryViewModel
            {
                Name = "Name",
                Description = "Desc"
            };

            //Act
            _categoriesService.SaveAsync(categoryViewModel);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<AddCategoryCommand>(), default(CancellationToken)), Times.Once());
        }

        [Test]
        public void SaveAsync_Calls_Mediator_ForEdit_WhenId_IsNotZero()
        {
            //Arrange
            var categoryViewModel = new CategoryViewModel
            {
                Id = 1,
                Name = "Name",
                Description = "Desc"
            };

            //Act
            _categoriesService.SaveAsync(categoryViewModel);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<EditCategoryCommand>(), default(CancellationToken)), Times.Once());
        }

        [Test]
        public void DeleteAsync_Calls_Mediator()
        {
            //Arrange

            //Act
            _categoriesService.DeleteAsync(1);

            //Assert
            _mockMediator.Verify(x =>
                x.Send(It.IsAny<DeleteCategoryCommand>(), default(CancellationToken)), Times.Once());
        }
    }
}
