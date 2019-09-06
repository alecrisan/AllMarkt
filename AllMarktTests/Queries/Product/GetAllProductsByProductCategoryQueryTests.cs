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
    public class GetAllProductsByProductCategoryQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllProductsByProductCategoryQuery, IEnumerable<ProductViewModel>> _getAllProductsQueryHandler;

        public GetAllProductsByProductCategoryQueryTests()
        {
            _getAllProductsQueryHandler = new GetAllProductsByProductCategoryQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllProductsByProductCategoryQueryHandler_ReturnsEmpty()
        {
            //Arrange
            var productCategory = new AllMarkt.Entities.ProductCategory { Name = "category", Description = "description" };
            AllMarktContextIM.ProductCategories.Add(productCategory);
            AllMarktContextIM.SaveChanges();

            //Act
            var result = await _getAllProductsQueryHandler.Handle(new GetAllProductsByProductCategoryQuery(productCategory.Id), CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllProductsByProductCategoryQueryHandler_ReturnsExistingProducts()
        {
            //Arrange
            var productCategory = new AllMarkt.Entities.ProductCategory { Name = "category", Description = "description" };
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
            var result = await _getAllProductsQueryHandler.Handle(new GetAllProductsByProductCategoryQuery(productCategory.Id), CancellationToken.None);

            //Assert

            result.Count().Should().Be(2);
        }
    }
}
