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
    public class AddProductCategoryCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<AddProductCategoryCommand> _addProductCategoryCommandHandler;

        public AddProductCategoryCommandTests()
        {
            _addProductCategoryCommandHandler = new AddProductCategoryCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task AddProductCategoryCommandHandler_AddsCategory()
        {
            //Arange
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
            var shop = AllMarktContextIM.Shops.FirstOrDefault();

            var addedProductCategory = new AddProductCategoryCommand
            {
                Name = "testName",
                Description = "DescName",
                ShopId = shop.Id
            };

            //Act
            await _addProductCategoryCommandHandler.Handle(addedProductCategory, CancellationToken.None);

            //Assert
            AllMarktContextIM.ProductCategories
                .Should()
                .Contain(productCategory => productCategory.Name==addedProductCategory.Name
                    && productCategory.Description==addedProductCategory.Description);
        }

        [Fact]
        public async Task AddproductCategoryCommandHandler_ThereIsNoShop()
        {
            //Arrange
            var addedProductCategory = new AddProductCategoryCommand
            {
                Name = "testName",
                Description = "DescName"
            };

            //Act
            await _addProductCategoryCommandHandler.Handle(addedProductCategory, CancellationToken.None);

            //Assert
            AllMarktContextIM.ProductCategories
                .Should()
                .NotContain(productCategory => productCategory.Name == addedProductCategory.Name
                    && productCategory.Description == addedProductCategory.Description);
        }
    }
}
