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
    public class GetAllProductCategoriesQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllProductCategoriesQuery, IEnumerable<ProductCategoryViewModel>> _getAllProductCategoriesQueryHandler;
        private FakeEntities _fakeEntities;

        public GetAllProductCategoriesQueryTests()
        {
            _getAllProductCategoriesQueryHandler = new GetAllProductCategoriesQueryHandler(AllMarktQueryContextIM);
            _fakeEntities = new FakeEntities(AllMarktContextIM);
        }

        [Fact]
        public async Task GetAllProductCategoriesQueryHandler_IsEmpty()
        {
            //Arange

            //Act
            var productCategories = await _getAllProductCategoriesQueryHandler.Handle(new GetAllProductCategoriesQuery(), CancellationToken.None);

            //Assert
            productCategories.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllProductCategoriesQueryHandler_IsNotEmpty()
        {
            //Arrange
            await _fakeEntities.SetTestDataProductCategoriesQueryAsync();
            
            //Act
            var getProductCategories = await _getAllProductCategoriesQueryHandler.Handle(new GetAllProductCategoriesQuery(), CancellationToken.None);

            //Assert
            getProductCategories.Count().Should().Be(3);
        }
    }
}
