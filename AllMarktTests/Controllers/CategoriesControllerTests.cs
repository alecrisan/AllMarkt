using AllMarkt.Controller;
using AllMarkt.Services.Interfaces;
using AllMarkt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllMarktTests.Controllers
{
    public class CategoriesControllerTests
    {
        private CategoriesController _categoriesController;
        private Mock<ICategoriesService> _mockCategoryService;

        [SetUp]
        public void Setup()
        {
            _mockCategoryService = new Mock<ICategoriesService>();
            _categoriesController = new CategoriesController(_mockCategoryService.Object);
        }

        [Test]
        public void GetAllAsync_Returns_OkResult()
        {
            //Arange

            //Act
            var result = _categoriesController.GetAllAsync();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public void GetAllAsync_Calls_CategoriesService()
        {
            //Arrange

            //Act
            var result = _categoriesController.GetAllAsync();

            //Assert
            _mockCategoryService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public void GetAllAsync_Returns_Categories()
        {
            //Arrange
            _mockCategoryService
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(GetCategories()));

            //Act
            var okResult = _categoriesController.GetAllAsync().Result as ObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<IEnumerable<CategoryViewModel>>(okResult.Value);

            var items = okResult.Value as IEnumerable<CategoryViewModel>;
            Assert.IsNotNull(items);

            Assert.AreEqual(2, items.Count());

            //items.Count().Should().Be(2);
        }

        [Test]
        public void AddAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = _categoriesController.AddAsync(new CategoryViewModel());

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result.Result);
        }

        [Test]
        public async Task AddAsync_Calls_CategoriesService()
        {
            //Arrange
            var category = new CategoryViewModel();

            //Act
            await _categoriesController.AddAsync(category);

            //Assert
            _mockCategoryService.Verify(x => x.SaveAsync(category));
        }

        [Test]
        public void EditAsync_Returns_NoContent()
        {
            //Arrange

            //Act
            var result = _categoriesController.EditAsync(new CategoryViewModel());

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result.Result);
        }

        [Test]
        public async Task EditAsync_Calls_CategoriesService()
        {
            //Arrange
            var category = new CategoryViewModel();

            //Act
            await _categoriesController.EditAsync(category);

            //Assert
            _mockCategoryService.Verify(x => x.SaveAsync(category));
        }

        [Test]
        public void DeleteAsync_Returns_NoContent()
        {
            //Arrange
            const int id = 1;

            //Act
            var result = _categoriesController.DeleteAsync(id);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result.Result);
        }

        [Test]
        public async Task DeleteAsync_Calls_CategoriesService()
        {
            //Arrange
            const int id = 1;

            //Act
            await _categoriesController.DeleteAsync(id);

            //Assert
            _mockCategoryService.Verify(x => x.DeleteAsync(id));
        }

        private IEnumerable<CategoryViewModel> GetCategories()
        {
            IEnumerable<CategoryViewModel> categories = new[]
            {
                new CategoryViewModel { Id = 1, Name = "Category Name 1", Description = "Desc1" },
                new CategoryViewModel { Id = 2, Name = "Category Name 2", Description = "Desc2" }
            };

            return categories;
        }
    }
}
