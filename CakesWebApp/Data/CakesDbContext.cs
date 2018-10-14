namespace CakesWebApp.Data
{
    using CakesWebApp.Models;
    using Microsoft.EntityFrameworkCore;

    public class CakesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });
        }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder
                .UseSqlServer(@"Server=DESKTOP-N8JQK16\SQLEXPRESS;Database=Cakes;Integrated Security=True;")
                .UseLazyLoadingProxies();
        }
    }
}
