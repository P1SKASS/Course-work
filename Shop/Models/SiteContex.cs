using Microsoft.EntityFrameworkCore;

namespace Shop.Models
{
    public class SiteContex : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public SiteContex(DbContextOptions<SiteContex> options)
            : base(options) 
        {
            
        }
    }
}
