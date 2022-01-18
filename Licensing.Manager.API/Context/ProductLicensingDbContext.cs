using Licensing.Manager.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Context
{
    public class ProductLicensingDbContext : DbContext
    {
        public ProductLicensingDbContext()
        {

        }

        public ProductLicensingDbContext(DbContextOptions<ProductLicensingDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=database.mycodelibraries.com;Database=LicensingApp;User ID=VisionNetUserPro;Password=VisionNetUserPro2020!@;Trusted_Connection=false;MultipleActiveResultSets=false");
            }
        }
    }
}
