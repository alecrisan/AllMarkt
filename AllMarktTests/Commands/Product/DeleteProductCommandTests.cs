using AllMarkt.Commands.Product;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.Product
{
    public class DeleteProductCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<DeleteProductCommand> _deleteProductCommandHandler;

        public DeleteProductCommandTests()
        {
            _deleteProductCommandHandler = new DeleteProductCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task DeleteProduct_CommandHandle_DeletesExistingProducty()
        {
            //Arrange
            var productCategory = new AllMarkt.Entities.ProductCategory { Name = "category", Description = "description" };
            AllMarktContextIM.ProductCategories.Add(productCategory);
            await AllMarktContextIM.SaveChangesAsync();

            AllMarktContextIM.Products.Add(new AllMarkt.Entities.Product
            {
                Name = "Test Name1",
                Description = "Test Description1",
                Price = 10,
                ImageURI = "",
                State = true,
                ProductCategory = productCategory
            });
            await AllMarktContextIM.SaveChangesAsync();
            var existingProduct = AllMarktContextIM.Products.First();

            var deleteProductCommand = new DeleteProductCommand { Id = existingProduct.Id };

            //Act
            await _deleteProductCommandHandler.Handle(deleteProductCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Products
                .Should()
                .NotContain(product => product.Id == deleteProductCommand.Id);
        }
    }
}
