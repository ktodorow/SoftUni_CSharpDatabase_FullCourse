using BookShop.Data.EntityConfiguration;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace BookShop.Data
{
    public class BookShopContext : DbContext
    {
        public BookShopContext() {}
        public BookShopContext(DbContextOptions options) : base(options) {}

        //dbsets
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookCategoryConfiguration());
        }   
    }
}
