using AllMarkt.Entities;
using AllMarkt.Queries.ProductComments;
using FluentAssertions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.ProductComments
{
    public class GetProductCommentsByProductIdQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetProductCommentsByProductIdQuery, IEnumerable<AllMarkt.ViewModels.ProductCommentViewModel>> _getProductCommentsQueryHandler;

        public GetProductCommentsByProductIdQueryTests()
        {
            _getProductCommentsQueryHandler = new GetProductCommentsByProductIdQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetProductCommentsByProductIdQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getProductCommentsQueryHandler.Handle(new GetProductCommentsByProductIdQuery(1), CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }


        [Fact]
        public async Task GetGetProductCommentsByProductIdQueryHandler_ReturnsExistingComments()
        {
            //Arrange
            var user = new AllMarkt.Entities.User
            {
                Email = "test@test.com",
                Password = "123",
                DisplayName = "UserTest"
            };

            var product1 = new AllMarkt.Entities.Product
            {
                Name = "prodTest1",
                Description = "prodTestDescriere1",
                Price = 20
            };
            var product2 = new AllMarkt.Entities.Product
            {
                Name = "prodTest2",
                Description = "prodTestDescriere2",
                Price = 20
            };

            AllMarktContextIM.Users.Add(user);
            AllMarktContextIM.Products.Add(product1);
            AllMarktContextIM.Products.Add(product2);
            await AllMarktContextIM.SaveChangesAsync();

            var productComment1 = new AllMarkt.Entities.ProductComment
            {
                Rating = 2,
                Text = "naspa rau",
                AddedBy = user,
                Product = product1,
                DateAdded = DateTime.UtcNow
            };

            var productComment2 = new AllMarkt.Entities.ProductComment
            {
                Rating = 4,
                Text = "super fain",
                AddedBy = user,
                Product = product2,
                DateAdded = DateTime.UtcNow
            };

            AllMarktContextIM.ProductComments.Add(productComment1);
            AllMarktContextIM.ProductComments.Add(productComment2);

            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getProductCommentsQueryHandler.Handle(new GetProductCommentsByProductIdQuery(product1.Id), CancellationToken.None);

            //Assert
            result.Count().Should().Be(1);
        }
    }
}
