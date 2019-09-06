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
    public class DeleteCategoryCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<DeleteCategoryCommand> _deleteCategoryCommandHandler;

        public DeleteCategoryCommandTests()
        {
            _deleteCategoryCommandHandler = new DeleteCategoryCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task DeleteCategoryCommandHandle_DeletesExistingCategory()
        {
            //Arrange
            AllMarktContextIM.Categories.Add(new AllMarkt.Entities.Category
            {
                Name = "TestName",
                Description = "TestDescription"
            });
            AllMarktContextIM.SaveChanges();
            var existingCategory = AllMarktContextIM.Categories.First();

            var deleteCategoryCommand = new DeleteCategoryCommand { Id = existingCategory.Id };

            //Act
            await _deleteCategoryCommandHandler.Handle(deleteCategoryCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Categories
                .Should()
                .NotContain(category => category.Id == deleteCategoryCommand.Id);
        }
    }
}
