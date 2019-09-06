using AllMarkt.Data;
using AllMarkt.Queries.Product;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.Product
{
    public class GetProductsWithRatingByCategoryIdQueryTests
    //: AllMarktContextTests
    {
        private readonly Mock<IAllMarktQueryContext> _queryContextMock;
        private readonly IRequestHandler<GetProductsWithRatingByCategoryIdQuery, IEnumerable<ProductWithRatingViewModel>> _getAllProductsWithRatingQueryHandler;


        public GetProductsWithRatingByCategoryIdQueryTests()
        {
            _queryContextMock = new Mock<IAllMarktQueryContext>();
            _getAllProductsWithRatingQueryHandler = new GetProductsWithRatingByCategoryIdQueryHandler(
                _queryContextMock.Object
            );
        }

        [Fact]
        public async Task GetProductsWithRatingByCategoryIdQueryHandler_ReturnsEmpty()
        {
            //Arrange
            var expectedItems = new[] { new ProductWithRatingViewModel() };
            _queryContextMock
                .Setup(queryContext => queryContext.ExecuteStoredProcedureAsync<ProductWithRatingViewModel>(It.IsAny<string>(), It.IsAny<SqlParameter[]>()))
                .ReturnsAsync(expectedItems);

            //Act
            var actualItems = await _getAllProductsWithRatingQueryHandler.Handle(
                new GetProductsWithRatingByCategoryIdQuery
                {
                    ProductCategoryId = 5
                },
                CancellationToken.None
            );

            //Assert
            actualItems.Should().BeSameAs(expectedItems);
            _queryContextMock
                .Verify(
                    queryContext => queryContext.ExecuteStoredProcedureAsync<ProductWithRatingViewModel>(
                        "GetProductsByCategoryIdWithAverageRating",
                        It.Is<SqlParameter[]>(
                            sqlParameters => sqlParameters
                                .Select(sqlParameter => new { sqlParameter.ParameterName, sqlParameter.Value })
                                .SequenceEqual(new []
                                {
                                    new
                                    {
                                        ParameterName = "@productCategoryId",
                                        Value = (object)5
                                    }
                                })
                        )
                    ),
                    Times.Once()
                );
        }

        // VARIANTA COMPLICATA!!!

        //private readonly Mock<AllMarktContext> _dbContextMock;
        //private readonly IRequestHandler<GetProductsWithRatingByCategoryIdQuery, IEnumerable<ProductWithRatingViewModel>> _getAllProductsWithRatingQueryHandler;

        //public GetProductsWithRatingByCategoryIdQueryTests()
        //{
        //    _dbContextMock = new Mock<AllMarktContext>(
        //        new DbContextOptionsBuilder<AllMarktContext>()
        //            .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //            .Options
        //    );
        //    _getAllProductsWithRatingQueryHandler = new GetProductsWithRatingByCategoryIdQueryHandler(
        //        new AllMarktQueryContext(
        //            _dbContextMock.Object
        //        )
        //    );
        //}

        //[Fact]
        //public async Task GetProductsWithRatingByCategoryIdQueryHandler_ReturnsEmpty()
        //{
        //    //Arrange
        //    var expectedItems = new[] { new ProductWithRatingViewModel() };
        //    var items = new QueriableMock<ProductWithRatingViewModel>(expectedItems);
        //    MethodCallExpression actualMethodCallExpression = null;

        //    var queryProviderMock = new Mock<IQueryProvider>();
        //    queryProviderMock.Setup(p => p.CreateQuery<ProductWithRatingViewModel>(It.IsAny<MethodCallExpression>()))
        //        .Returns<MethodCallExpression>(methodCallExpression =>
        //        {
        //            actualMethodCallExpression = methodCallExpression;
        //            return items;
        //        });

        //    var dbQueryMock = new Mock<DbQuery<ProductWithRatingViewModel>>();
        //    dbQueryMock
        //        .As<IQueryable<ProductWithRatingViewModel>>()
        //        .SetupGet(query => query.Provider)
        //        .Returns(queryProviderMock.Object);
        //    dbQueryMock
        //        .As<IQueryable<ProductWithRatingViewModel>>()
        //        .SetupGet(query => query.Expression)
        //        .Returns(Expression.Constant(dbQueryMock.Object));
        //    _dbContextMock.Setup(dbContext => dbContext.Query<ProductWithRatingViewModel>()).Returns(dbQueryMock.Object);

        //    //Act
        //    var actualItems = await _getAllProductsWithRatingQueryHandler.Handle(
        //        new GetProductsWithRatingByCategoryIdQuery
        //        {
        //            ProductCategoryId = 5
        //        },
        //        CancellationToken.None
        //    );

        //    //Assert
        //    actualMethodCallExpression.Method.Name.Should().Be(nameof(RelationalQueryableExtensions.FromSql));

        //    var spName = (RawSqlString)actualMethodCallExpression.Arguments.Cast<ConstantExpression>().ElementAt(1).Value;
        //    var spParams = (IEnumerable<SqlParameter>)actualMethodCallExpression.Arguments.Cast<ConstantExpression>().ElementAt(2).Value;

        //    spName.Should().BeEquivalentTo(new RawSqlString("EXEC [dbo].[GetProductsByCategoryIdWithAverageRating] @productCategoryId"));

        //    spParams.Select(spParam => new { spParam.ParameterName, spParam.Value }).Should().BeEquivalentTo(new[]{
        //        new
        //        {
        //            ParameterName = "@productCategoryId",
        //            Value = 5
        //        }
        //    });
        //    actualItems.Should().BeEquivalentTo(expectedItems);
        //}

        //private class QueriableMock<T> : IQueryable<T>, IAsyncEnumerable<T>
        //{
        //    private readonly IEnumerable<T> _items;

        //    public QueriableMock(IEnumerable<T> items)
        //    {
        //        _items = items;
        //    }

        //    public QueriableMock(params T[] items)
        //        : this(items.AsEnumerable())
        //    {
        //    }

        //    public Type ElementType
        //        => typeof(T);

        //    public Expression Expression => throw new NotImplementedException();

        //    public IQueryProvider Provider => new Mock<IQueryProvider>().Object;

        //    public IEnumerator<T> GetEnumerator()
        //        => _items.GetEnumerator();

        //    IEnumerator IEnumerable.GetEnumerator()
        //        => _items.GetEnumerator();

        //    IAsyncEnumerator<T> IAsyncEnumerable<T>.GetEnumerator()
        //        => _items.ToAsyncEnumerable().GetEnumerator();
        //}
    }
}
