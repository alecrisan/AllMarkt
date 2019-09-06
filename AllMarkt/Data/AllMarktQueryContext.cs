using AllMarkt.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AllMarkt.Data
{
    public class AllMarktQueryContext : IAllMarktQueryContext
    {
        private readonly AllMarktContext _allMarktContext;

        public AllMarktQueryContext(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        public IQueryable<Category> Categories
            => _allMarktContext.Categories.AsNoTracking();

        public IQueryable<PrivateMessage> PrivateMessages
            => _allMarktContext.PrivateMessages
            .Include(privateMessage => privateMessage.Sender)
            .Include(privateMessage => privateMessage.Receiver)
            .AsNoTracking();

        public IQueryable<User> Users
            => _allMarktContext.Users
            .AsNoTracking();

        public IQueryable<ProductCategory> ProductCategories
            => _allMarktContext.ProductCategories
                .Include(x => x.Shop)
                .ThenInclude(s => s.User)
                .AsNoTracking();

        public IQueryable<Product> Products
            => _allMarktContext.Products
                .Include(x => x.ProductCategory)
                .AsNoTracking();

        public IQueryable<ShopComment> ShopComments
            => _allMarktContext.ShopComments
                .Include(y => y.AddedBy)
                .Include(x => x.Shop)
                .ThenInclude(s => s.User)
                .AsNoTracking();

        public async Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(string spName, params SqlParameter[] parameters)
            where TResult : class
        {
            return await _allMarktContext
                .Query<TResult>()
                .FromSql($"EXEC [dbo].[{spName}] {string.Join(',', parameters.Select(p=> p.ParameterName))}", parameters)
                .AsNoTracking()
                .ToListAsync();
        }

        public IQueryable<ProductComment> ProductComments
                => _allMarktContext.ProductComments
                    .Include(y => y.AddedBy)
                    .Include(x => x.Product)
                    .AsNoTracking();

        public IQueryable<Order> Orders
            => _allMarktContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Seller)
                .ThenInclude(s => s.User)
                .Include(o => o.Buyer)
                .ThenInclude(b => b.User)
                .AsNoTracking();

        public IQueryable<OrderItem> OrderItems
            => _allMarktContext.OrderItems;

        public IQueryable<Shop> Shops
            => _allMarktContext.Shops
            .Include(s => s.User)
            .AsNoTracking();

        public IQueryable<Customer> Customers
            => _allMarktContext.Customers
            .Include(s => s.User)
            .AsNoTracking();

        public IQueryable<ShopCategory> ShopCategories
            => _allMarktContext.ShopCategories
            .Include(s => s.Category)
            .AsNoTracking();
    }
}
