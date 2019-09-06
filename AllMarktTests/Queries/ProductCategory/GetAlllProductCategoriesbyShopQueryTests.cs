using AllMarkt.Queries.ProductCategory;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.ProductCategory
{
    public class GetAllProductCategoriesByShopQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllProductCategoriesByShopQuery, IEnumerable<ProductCategoryViewModel>> _getAllProductCategoriesByShopQueryHandler;
        private FakeEntities _fakeEntities;

        public GetAllProductCategoriesByShopQueryTests()
        {
            _getAllProductCategoriesByShopQueryHandler = new GetAllProductCategoriesByShopQueryHandler(AllMarktQueryContextIM);
            _fakeEntities = new FakeEntities(AllMarktContextIM);
        }

        [Fact]
        public async Task GetAllProductCategoriesbyShopQueryHandler_IsEmpty()
        {
            //Arange

            //Act
            var productCategories = await _getAllProductCategoriesByShopQueryHandler.Handle(new GetAllProductCategoriesByShopQuery(1), CancellationToken.None);

            //Assert
            productCategories.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllProductCategoriesbyShopQueryHandler_WhenId_IsOne()
        {
            //Arrange
            var shopId = await _fakeEntities.SetTestDataProductCategoriesQueryAsync();

            //Act
            var productCategories = await _getAllProductCategoriesByShopQueryHandler.Handle(new GetAllProductCategoriesByShopQuery(shopId), CancellationToken.None);

            //Assert
            productCategories.Count().Should().Be(2);

        }

        [Fact]
        public async Task GetAllProductCategoriesByShopQueryHandler_WhenId_DoesNotExist()
        {
            //Arrange

            //Act
            var productCategories = await _getAllProductCategoriesByShopQueryHandler.Handle(new GetAllProductCategoriesByShopQuery(3), CancellationToken.None);

            //Assert
            productCategories.Count().Should().Be(0);
        }
    }
}
