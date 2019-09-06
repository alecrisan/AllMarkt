using AllMarkt.Entities;
using AllMarkt.Queries.ProductComments;
using FluentAssertions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.ProductComments
{
    public class GetAllProductCommentsQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllProductCommentsQuery, IEnumerable<AllMarkt.ViewModels.ProductCommentViewModel>> _getAllProductCommentsQueryHandler;

        public GetAllProductCommentsQueryTests()
        {
            _getAllProductCommentsQueryHandler = new GetAllProductCommentsQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllProductCommentsQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getAllProductCommentsQueryHandler.Handle(new GetAllProductCommentsQuery(), CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllProductCommentsQueryHandler_ReturnsExistingComments()
        {
            //Arrange
            var user = new AllMarkt.Entities.User
            {
                Email = "test@test.com",
                Password = "123",
                DisplayName = "UserTest"
            };

            var product = new AllMarkt.Entities.Product
            {
                Name = "prodTest",
                Description = "prodTestDescriere",
                Price = 20
            };

            AllMarktContextIM.Users.Add(user);
            AllMarktContextIM.Products.Add(product);
            await AllMarktContextIM.SaveChangesAsync();

            AllMarktContextIM.ProductComments.Add(new AllMarkt.Entities.ProductComment
            {
                Rating = 2,
                Text = "naspa rau",
                AddedBy = user,
                Product = product,
            });

            AllMarktContextIM.ProductComments.Add(new AllMarkt.Entities.ProductComment
            {
                Rating = 5,
                Text = "super fain",
                AddedBy = user,
                Product = product
            });

            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getAllProductCommentsQueryHandler.Handle(new GetAllProductCommentsQuery(), CancellationToken.None);

            //Assert
            result.Count().Should().Be(2);
        }
    }
}
