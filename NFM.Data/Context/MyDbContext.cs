using Microsoft.EntityFrameworkCore;
using NFM.Domain.Models;

namespace NFM.Domain.Context
{
    public class MyDbContext : DbContext
    {
        private readonly string _windowsConnectionString = @"Server=NBKR004644;Database=NetworkFarmacyManagementDb;Trusted_Connection=True;TrustServerCertificate=true";
        
        public DbSet<Product> Products { get; set; }
      
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_windowsConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
