using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NFM.Domain.Models;

namespace NFM.Domain.Context
{
    public class MyDbContext : IdentityDbContext
    {      
        public DbSet<Employee> Employees { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            AddUserRoles(builder);
            AddEmployeeData(builder);
        }

        private void AddEmployeeData(ModelBuilder builder)
        {
            var employees = new List<Employee>
            {
                new() { Id = 1, CNP = "1234567890123", Name = "John Doe" },
                new() { Id = 2, CNP = "9876543210987", Name = "Jane Smith" }
            };
            builder.Entity<Employee>().HasData(employees);
        }

        private void AddUserRoles(ModelBuilder builder)
        {
            var adminRoleId = "06e6871e-18de-4b90-85ad-afbadff862b1";
            var operatorRoleId = "c30f5824-76d4-44d9-a38e-35a955c6bc7f";

            var roles = new List<IdentityRole>
                       {
                           new()
                           {
                               Id = adminRoleId,
                               ConcurrencyStamp = adminRoleId,
                               Name = AppUserRole.Admin,
                               NormalizedName = AppUserRole.Admin.ToLower()
                           },
                           new()
                           {
                               Id = operatorRoleId,
                               ConcurrencyStamp = operatorRoleId,
                               Name = AppUserRole.Operator,
                               NormalizedName = AppUserRole.Operator.ToLower()
                           }
                       };


            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
