using AllMarkt.Data;
using AllMarkt.Queries.Product;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using Moq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.Product
{
    public class GetAllProductsByShopQueryTests
    {
        private readonly Mock<IAllMarktQueryContext> _queryContextMock;
        private readonly IRequestHandler<GetAllProductsByShopQuery, IEnumerable<ProductWithRatingViewModel>> _getAllProductsWithRatingQueryHandler;


        public GetAllProductsByShopQueryTests()
        {
            _queryContextMock = new Mock<IAllMarktQueryContext>();
            _getAllProductsWithRatingQueryHandler = new GetAllProductsByShopQueryHandler(
                _queryContextMock.Object
            );
        }

        [Fact]
        public async Task GetAllProductsByShopQueryHandler_ReturnsEmpty()
        {
            //Arrange
            var expectedItems = new[] { new ProductWithRatingViewModel() };
            _queryContextMock
                .Setup(queryContext => queryContext.ExecuteStoredProcedureAsync<ProductWithRatingViewModel>(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedItems);

            //Act
            var actualItems = await _getAllProductsWithRatingQueryHandler.Handle(
                new GetAllProductsByShopQuery
                {
                    ShopId = 5
                },
                CancellationToken.None
            );

            //Assert
            actualItems.Should().BeSameAs(expectedItems);
            _queryContextMock
                .Verify(
                    queryContext => queryContext.ExecuteStoredProcedureAsync<ProductWithRatingViewModel>(
                        "GetAllProductsByShop",
                        It.Is<SqlParameter[]>(
                            sqlParameters => sqlParameters
                                .Select(sqlParameter => new { sqlParameter.ParameterName, sqlParameter.Value })
                                .SequenceEqual(new[]
                                {
                                    new
                                    {
                                        ParameterName = "@shopId",
                                        Value = (object)5
                                    }
                                })
                        )
                    ),
                    Times.Once()
                );
        }
    }
}
