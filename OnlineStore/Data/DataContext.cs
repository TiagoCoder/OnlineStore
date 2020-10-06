using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data.Entities;

namespace OnlineStore.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderDetailsTemp> OrderDetailsTemp { get; set; }

        public DbSet<TempOrder> TempOrders { get; set; }

        public DbSet<TempOrderDetail> TempOrderDetails { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


    }
}
