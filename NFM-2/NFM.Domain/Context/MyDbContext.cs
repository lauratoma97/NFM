using Microsoft.EntityFrameworkCore;
using NFM.Domain.Configuration;
using NFM.Domain.Models;

namespace NFM.Domain.Context
{
    public class MyDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
      
        public DbSet<Employee> Employees { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
