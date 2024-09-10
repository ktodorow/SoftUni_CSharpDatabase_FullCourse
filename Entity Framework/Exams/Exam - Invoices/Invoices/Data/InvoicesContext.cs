namespace Invoices.Data
{
    using Models;

    using Microsoft.EntityFrameworkCore;

    public class InvoicesContext : DbContext
    {
        public InvoicesContext() 
        { 
        }

        public InvoicesContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<ProductClient> ProductsClients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductClient>()
                .HasKey(key => new { key.ClientId, key.ProductId });

            //I coded this because EF is throwing warnings about decimals having no precision
            //modelBuilder.Entity<Invoice>()
            //    .Property(a => a.Amount)
            //    .HasPrecision(18, 4);
                
            //modelBuilder.Entity<Product>()
            //    .Property(p => p.Price)
            //    .HasPrecision(18, 4);
        }
    }
}
