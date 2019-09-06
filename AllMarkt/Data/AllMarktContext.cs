using AllMarkt.Entities;
using AllMarkt.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AllMarkt.Data
{
    public class AllMarktContext : DbContext
    {
        public AllMarktContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Moderator> Moderators { get; set; }

        public DbSet<Shop> Shops { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ShopCategory> ShopCategories { get; set; }

        public DbSet<ShopComment> ShopComments { get; set; }

        public DbSet<ProductComment> ProductComments { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<PrivateMessage> PrivateMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<User>()
                .HasMany(u => u.ReceivedMessages)
                .WithOne(x => x.Receiver)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasMany(u => u.SentMessages)
                .WithOne(x => x.Sender)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ShopCategory>()
                .HasKey(x => new { x.ShopId, x.CategoryId });

            builder.Entity<Category>()
                .HasMany(x => x.ShopCategoryLink)
                .WithOne(x => x.Category)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ShopCategory>()
                .HasOne(x => x.Category)
                .WithMany(x => x.ShopCategoryLink)
                .OnDelete(DeleteBehavior.Cascade);              

            builder.Entity<Shop>()
                .HasMany(x => x.ShopCategoryLink)
                .WithOne(x => x.Shop)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Shop>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.Shop)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.Product)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasMany(u => u.OrderItems)
                .WithOne(x => x.Order)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Query<ProductWithRatingViewModel>();
        }
    }
}
