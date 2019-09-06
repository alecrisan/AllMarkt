using AllMarkt.Data;
using System.Threading.Tasks;

namespace AllMarktTests.Queries.ProductCategory
{
    public class FakeEntities
    {
        private AllMarktContext _allMarktContextIM;

        public FakeEntities(AllMarktContext allMarktContextIM)
        {
            _allMarktContextIM = allMarktContextIM;
        }

        public async Task<int> SetTestDataProductCategoriesQueryAsync()
        {
            var user = new AllMarkt.Entities.User
            {
                Email = "email@asd.com",
                DisplayName = "tets"
            };
            _allMarktContextIM.Users.Add(user);
            await _allMarktContextIM.SaveChangesAsync();

            var shop1 = new AllMarkt.Entities.Shop
            {

                Address = "dddd",
                User = user,
                Comments = null,
                CUI = "ddd",
                IBAN = "dffddfdfd",
                Orders = null,
                PhoneNumber = "0123654789",
                ProductCategories = null,
                ShopCategoryLink = null,
                SocialCapital = 3
            };

            var shop2 = new AllMarkt.Entities.Shop
            {
                Address = "aaa",
                User = user,
                Comments = null,
                CUI = "ddd",
                IBAN = "aaaa",
                Orders = null,
                PhoneNumber = "0123654789",
                ProductCategories = null,
                ShopCategoryLink = null,
                SocialCapital = 2
            };

            _allMarktContextIM.Shops.Add(shop1);
            _allMarktContextIM.Shops.Add(shop2);
            await _allMarktContextIM.SaveChangesAsync();

            _allMarktContextIM.ProductCategories.Add(new AllMarkt.Entities.ProductCategory
            {
                Description = "desc1",
                Name = "name1",
                Shop = shop1
            });
            _allMarktContextIM.ProductCategories.Add(new AllMarkt.Entities.ProductCategory
            {
                Description = "desc2",
                Name = "name2",
                Shop = shop1
            });

            _allMarktContextIM.ProductCategories.Add(new AllMarkt.Entities.ProductCategory
            {
                Description = "desc3",
                Name = "name3",
                Shop = shop2
            });
            await _allMarktContextIM.SaveChangesAsync();
            return shop1.Id;
        }
    }
}
