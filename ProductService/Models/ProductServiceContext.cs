using System.Data.Entity;

namespace ProductService.Models
{
    public class ProductServiceContext : DbContext
    {
        public ProductServiceContext() : base("name=ProductServiceContext")
        {
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOption> ProductOptions { get; set; }
    }
}