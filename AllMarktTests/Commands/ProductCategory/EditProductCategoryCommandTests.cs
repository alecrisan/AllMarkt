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
    public class EditProductCategoryCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<EditProductCategoryCommand> _editProductCategoryCommandHandler;

        public EditProductCategoryCommandTests()
        {
            _editProductCategoryCommandHandler = new EditProductCategoryCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task EditProductCategoryCommandHandler_EditExistingProductCategory()
        {
            //Arange
            AllMarktContextIM.Shops.Add(new AllMarkt.Entities.Shop
            {
                    Address = "Address1",
                    Comments = null,
                    CUI = "CUI1",
                    IBAN = "IBAN",
                    Orders = null,
                    PhoneNumber = "0123654789",
                    ProductCategories = null,
                    ShopCategoryLink = null,
                    SocialCapital = 3
            });

            AllMarktContextIM.SaveChanges();
            var shop = AllMarktContextIM.Shops.FirstOrDefault();

            AllMarktContextIM.ProductCategories.Add(new AllMarkt.Entities.ProductCategory
            {
                Name = "firstName",
                Description = "FirstDec",
                Shop = shop
            });
            AllMarktContextIM.SaveChanges();

            var existedProductCategory = AllMarktContextIM.ProductCategories.FirstOrDefault();
            var newProductCategory = new EditProductCategoryCommand
            {
                Id = existedProductCategory.Id,
                Name = "editedName",
                Description = "editedDesc",
            };

            //Act
            await _editProductCategoryCommandHandler.Handle(newProductCategory, CancellationToken.None);

            //Assert
            AllMarktContextIM.ProductCategories
                .Should()
                .Contain(x => x.Id == newProductCategory.Id
                    && x.Name == newProductCategory.Name
                    && x.Description == newProductCategory.Description);
        }
    }
}
