using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Commands.Category;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AllMarktTests.Commands.Category
{
    public class EditCategoryCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<EditCategoryCommand> _editCategoryCommandHandler;

        public EditCategoryCommandTests()
        {
            _editCategoryCommandHandler = new EditCategoryCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task EditCategoryCommandHandle_UpdatesExistingCategory()
        {
            //Arrange
            var category = new AllMarkt.Entities.Category
            {
                Name = "TestName",
                Description = "TestDescription"
            };

            AllMarktContextIM.Categories.Add(category);
            AllMarktContextIM.SaveChanges();

            var existingCategory = AllMarktContextIM.Categories.First();

            var editCategoryCommand = new EditCategoryCommand
            {
                Id = existingCategory.Id,
                Name = "TestName_EDIT",
                Description = "TestDescription_EDIT"
            };

            //Act
            await _editCategoryCommandHandler.Handle(editCategoryCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Categories.Should().Contain(x => x.Id == editCategoryCommand.Id);

            category.Name.Should().Be(editCategoryCommand.Name);
            category.Description.Should().Be(editCategoryCommand.Description);
        }
    }
}
