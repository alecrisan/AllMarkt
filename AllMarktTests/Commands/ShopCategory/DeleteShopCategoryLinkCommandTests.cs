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
    public class DeleteShopCategoryLinkCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<DeleteShopCategoryLinkCommand> _deleteShopCategoryLinkHandler;

        public DeleteShopCategoryLinkCommandTests()
        {
            _deleteShopCategoryLinkHandler = new DeleteShopCategoryLinkCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task DeleteShopCategoryLinkCommandHandlre_Deleting_ShopCategory()
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

            AllMarktContextIM.ShopCategories.Add(new AllMarkt.Entities.ShopCategory
            {
                ShopId = shop.Id,
                CategoryId = category.Id
            });
            AllMarktContextIM.SaveChanges();

            var deleteShopCategoryLink = new DeleteShopCategoryLinkCommand
            {
                ShopId = shop.Id,
                CategoryId = category.Id
            };

            //Act
            await _deleteShopCategoryLinkHandler.Handle(deleteShopCategoryLink, CancellationToken.None);

            //Assert
            AllMarktContextIM.ShopCategories
                .Should()
                .NotContain(sc => sc.ShopId == deleteShopCategoryLink.ShopId && sc.CategoryId == deleteShopCategoryLink.CategoryId);
        }
    }
}
