using AllMarkt.Commands.ShopCategory;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.ShopCategory
{
    public class AddShopCategoryLinkCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<AddShopCategoryLinkCommand> _addShopCategoryLinkHandler;

        public AddShopCategoryLinkCommandTests()
        {
            _addShopCategoryLinkHandler = new AddShopCategoryLinkCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task AddShopCategoryLinkCommandHandler_AddShopCategory()
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
            var shop = AllMarktContextIM.Shops.FirstOrDefault();

            AllMarktContextIM.Categories.Add(new AllMarkt.Entities.Category
            {
                Name = "Categ 1",
                Description = "Description"
            });

            AllMarktContextIM.SaveChanges();
            var category = AllMarktContextIM.Categories.FirstOrDefault();

            var addShopCategoryLink = new AddShopCategoryLinkCommand
            {
                ShopId = shop.Id,
                CategoryId = category.Id
            };
            //Act
            await _addShopCategoryLinkHandler.Handle(addShopCategoryLink, CancellationToken.None);

            //Assert
            AllMarktContextIM.ShopCategories
                .Should()
                .Contain(sc => (sc.ShopId == addShopCategoryLink.ShopId) && (sc.CategoryId == addShopCategoryLink.CategoryId));
        }
    }
}
