using AllMarkt.Commands.ProductCategory;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.ProductCategory
{
    public class DeleteProductCategoryCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<DeleteProductCategoryCommand> _deleteProductCategoryHandler;

        public DeleteProductCategoryCommandTests()
        {
            _deleteProductCategoryHandler = new DeleteProductCategoryCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task DeleteProductCategoryCommandHandle_DeletingCategory()
        {
            //Arrange
            AllMarktContextIM.Shops.Add(new AllMarkt.Entities.Shop
            {
                Address = "dddd",
                Comments = null,
                CUI = "ddd",
                IBAN = "dffddfdfd",
                Orders = null,
                PhoneNumber = "0123654789",
                ProductCategories = null,
                ShopCategoryLink = null,
                SocialCapital = 3
            });
            AllMarktContextIM.SaveChanges();
            var shop = AllMarktContextIM.Shops.First();

            AllMarktContextIM.ProductCategories.Add(new AllMarkt.Entities.ProductCategory
            {
                Name = "TestName",
                Description = "TestDescription",
                Shop = shop,
                Products = null
            }) ;
            AllMarktContextIM.SaveChanges();
            var existentProductCategory = AllMarktContextIM.ProductCategories.First();
            var deletedProductCategory = new DeleteProductCategoryCommand { Id = existentProductCategory.Id };

            //Act
            await _deleteProductCategoryHandler.Handle(deletedProductCategory, CancellationToken.None);

            //Assert
            AllMarktContextIM.ProductCategories
                .Should()
                .NotContain(productCategory => productCategory.Id == deletedProductCategory.Id);
        }
    }
}
