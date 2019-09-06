using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Commands.Category;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AllMarktTests.Commands.Category
{
    public class AddCategoryCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<AddCategoryCommand> _addCategoryCommandHandler;

        public AddCategoryCommandTests()
        {
            _addCategoryCommandHandler = new AddCategoryCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task AddCategoryCommandHandle_AddsCategory()
        {
            //Arrange
            var addCategoryCommand = new AddCategoryCommand
            {
                Name = "TestName",
                Description = "TestDescription"
            };

            //Act
            await _addCategoryCommandHandler.Handle(addCategoryCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Categories.Should()
                .Contain(category =>
                    category.Name == addCategoryCommand.Name
                    && category.Description == addCategoryCommand.Description);
        }
    }
}
