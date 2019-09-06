using AllMarkt.Queries.Category;
using AllMarkt.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AllMarktTests.Queries.Category
{
    public class GetAllCategoriesQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryViewModel>> _getAllCategoriesQueryHandler;

        public GetAllCategoriesQueryTests()
        {
            _getAllCategoriesQueryHandler = new GetAllCategoriesQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllCategoriesQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getAllCategoriesQueryHandler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

            //Assert
            //Assert.NotNull(result);
            //Assert.Empty(result);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllCategoriesQueryHandler_ReturnsExistingCategories()
        {
            //Arrange
            AllMarktContextIM.Categories.Add(new AllMarkt.Entities.Category
            {
                Name = "Test Name1",
                Description = "Test Description1"
            });

            AllMarktContextIM.Categories.Add(new AllMarkt.Entities.Category
            {
                Name = "Test Name2",
                Description = "Test Description2"
            });

            AllMarktContextIM.SaveChanges();

            //Act
            var result = await _getAllCategoriesQueryHandler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

            //Assert
            result.Count().Should().Be(2);
        }
    }
}
