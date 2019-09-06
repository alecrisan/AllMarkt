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
    public class EditProductCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<EditProductCommand> _editProductCommandHandler;

        public EditProductCommandTests()
        {
            _editProductCommandHandler = new EditProductCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task EditProduct_CommandHandle_UpdatesExistingProduct()
        {
            //Arrange
            var productCategory = new AllMarkt.Entities.ProductCategory
            {
                Name = "category",
                Description = "description"
            };

            AllMarktContextIM.ProductCategories.Add(productCategory);
            await AllMarktContextIM.SaveChangesAsync();

            var product = new AllMarkt.Entities.Product
            {
                Name = "Test Name1",
                Description = "Test Description1",
                Price = 10,
                ImageURI = "",
                State = true,
                ProductCategory = productCategory
            };

            AllMarktContextIM.Products.Add(product);
            await AllMarktContextIM.SaveChangesAsync();

            var existingProduct = AllMarktContextIM.Products.First();

            var editProductCommand = new EditProductCommand
            {
                Id = existingProduct.Id,
                Name = "TestName_EDIT",
                Description = "TestDescription_EDIT",
                Price = 11,
                ImageURI = "abc",
                State = false,
            };

            //Act
            await _editProductCommandHandler.Handle(editProductCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Products.Should().Contain(x => x.Id == editProductCommand.Id);

            product.Name.Should().Be(editProductCommand.Name);
            product.Description.Should().Be(editProductCommand.Description);
            product.Price.Should().Be(editProductCommand.Price);
            product.ImageURI.Should().Be(editProductCommand.ImageURI);
            product.State.Should().Be(editProductCommand.State);
        }
    }
}
