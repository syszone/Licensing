using Licensing.Manager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Licensing.Manager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Models.Customer> Customer { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<LicenseModel> LicenseModel { get; set; }
        public DbSet<ProductFeature> ProductFeature { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasKey(c => new { c.ProductId });
            modelBuilder.Entity<Product>().Property(a => a.ProductId).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();

            modelBuilder.Entity<LicenseModel>().HasKey(c => new { c.LicenseModelId });
            modelBuilder.Entity<LicenseModel>().Property(a => a.ProductId).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();

            modelBuilder.Entity<Models.Customer>().HasKey(c => new { c.CustomerId });
            modelBuilder.Entity<Models.Customer>().Property(a => a.CustomerId).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductFeature>().HasKey(c => new { c.FeatureId });
            modelBuilder.Entity<ProductFeature>().Property(a => a.FeatureId).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductFeature>()
            .HasOne(s => s.Product)
            .WithMany(g => g.ProductFeatures);

            modelBuilder.Entity<LicenseModel>()
            .HasOne(s => s.Product)
            .WithMany(g => g.LicenseModels);

        }
         
    }
}
