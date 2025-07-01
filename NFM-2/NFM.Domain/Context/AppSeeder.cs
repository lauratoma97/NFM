
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NFM.Domain.Models;

namespace NFM.Domain.Context;

public class AppSeeder(MyDbContext dbContext, UserManager<IdentityUser> userManager)
{
    public async Task SeedAsync()
    {
        if (!dbContext.Users.Any())
        {
            var adminUserId = Guid.NewGuid().ToString();
            var operatorUserId = Guid.NewGuid().ToString();
            var passwordHasher = new PasswordHasher<IdentityUser>();

            var admin = new IdentityUser()
                {
                    Id = adminUserId,
                    Email = "admin@app.com",
                    UserName = "admin",
                    NormalizedEmail = "admin@app.com",
                    NormalizedUserName = "admin",
                    PasswordHash = passwordHasher.HashPassword(null, "Summer2025!")
                };

            var operatorUser = new IdentityUser()
            {
                Id = operatorUserId,
                Email = "operator@app.com",
                UserName = "operator",
                NormalizedEmail = "operator@app.com",
                NormalizedUserName = "operator",
                PasswordHash = passwordHasher.HashPassword(null, "Summer2025!")
            };

            await userManager.CreateAsync(admin);
            await userManager.AddToRoleAsync(admin, AppUserRole.Admin);

            await userManager.CreateAsync(operatorUser);
            await userManager.AddToRoleAsync(operatorUser, AppUserRole.Operator);
        }

        await dbContext.SaveChangesAsync();
    }
}