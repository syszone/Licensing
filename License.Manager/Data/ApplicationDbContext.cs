using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using License.Manager.Models;

namespace License.Manager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customer { get; set; }
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

            modelBuilder.Entity<Customer>().HasKey(c => new { c.CustomerId });
            modelBuilder.Entity<Customer>().Property(a => a.CustomerId).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductFeature>().HasKey(c => new { c.FeatureId });
            modelBuilder.Entity<ProductFeature>().Property(a => a.FeatureId).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductFeature>()
            .HasOne(s => s.Product)
            .WithMany(g => g.ProductFeatures);

            modelBuilder.Entity<LicenseModel>()
            .HasOne(s => s.Product)
            .WithMany(g => g.LicenseModels);

        }
        public DbSet<License.Manager.Models.Product> Product_1 { get; set; }
    }
}
