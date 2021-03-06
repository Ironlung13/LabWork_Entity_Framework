using LabWork_Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace LabWork_Entity_Framework.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=teststoredb;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Sales");
            modelBuilder.Entity<OrderItem>()
                .HasKey(a => new { a.OrderId, a.Id });
            modelBuilder.Entity<Stock>()
                .HasKey(a => new { a.StoreId, a.ProductId });
        }

        //Sales
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Store> Stores { get; set; }
        //Production
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
