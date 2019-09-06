using AllMarkt.Queries.Product;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.Product
{
    public class GetAllProductsQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllProductsQuery, IEnumerable<ProductViewModel>> _getAllProductsQueryHandler;

        public GetAllProductsQueryTests()
        {
            _getAllProductsQueryHandler = new GetAllProductsQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllProductsQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getAllProductsQueryHandler.Handle(new GetAllProductsQuery(), CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllProductsQueryHandler_ReturnsExistingProducts()
        {
            //Arrange
            var productCategory = new AllMarkt.Entities.ProductCategory {Name = "category", Description = "description" };
            AllMarktContextIM.ProductCategories.Add(productCategory);
            AllMarktContextIM.SaveChanges();

            AllMarktContextIM.Products.Add(new AllMarkt.Entities.Product
            {
                Name = "Test Name1",
                Description = "Test Description1",
                Price = 10,
                ImageURI = "",
                State = true,
                ProductCategory = productCategory
            });

            AllMarktContextIM.Products.Add(new AllMarkt.Entities.Product
            {
                Name = "Test Name2",
                Description = "Test Description2",
                Price = 10,
                ImageURI = "",
                State = true,
                ProductCategory = productCategory
            });

            AllMarktContextIM.SaveChanges();

            //Act
            var result = await _getAllProductsQueryHandler.Handle(new GetAllProductsQuery(), CancellationToken.None);

            //Assert

            result.Count().Should().Be(2);
        }
    }
}
