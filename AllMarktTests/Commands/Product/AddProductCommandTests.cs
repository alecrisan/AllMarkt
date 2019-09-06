using AllMarkt.Commands.Product;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.Product
{
    public class AddProductCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<AddProductCommand> _addProductCommandHandler;
        private AllMarkt.Entities.ProductCategory _productCategory;

        public AddProductCommandTests()
        {
            _addProductCommandHandler = new AddProductCommandHandler(AllMarktContextIM);
            _productCategory = new AllMarkt.Entities.ProductCategory();
        }

        [Fact]
        public async Task AddProductCommandHandle_AddsProduct()
        {
            //Arrange
            var productCategory = new AllMarkt.Entities.ProductCategory { Name = "category", Description = "description" };
            AllMarktContextIM.ProductCategories.Add(productCategory);
            AllMarktContextIM.SaveChanges();

            var addProductCommand = new AddProductCommand
            {
                Name = "Test Name1",
                Description = "Test Description1",
                Price = 10,
                ImageURI = "",
                State = true,
                ProductCategoryId = productCategory.Id
            };

            //Act
            await _addProductCommandHandler.Handle(addProductCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Products.Should()
                .Contain(product =>
                    product.Name == addProductCommand.Name
                    && product.Description == addProductCommand.Description 
                    && product.Price == addProductCommand.Price
                    && product.ImageURI == addProductCommand.ImageURI
                    && product.State == addProductCommand.State
                    && product.ProductCategory.Id == addProductCommand.ProductCategoryId);
        }
    }
}
