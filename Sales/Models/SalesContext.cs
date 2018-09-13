using Microsoft.EntityFrameworkCore;

namespace Sales.Models
{
    public sealed class SalesContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderBooks> OrderBooks { get; set; }

        public SalesContext(DbContextOptions<SalesContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
