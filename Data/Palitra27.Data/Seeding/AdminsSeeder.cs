namespace Palitra27.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Palitra27.Common;
    using Palitra27.Data.Models;

    internal class AdminsSeeder : ISeeder
    {
        private readonly UserManager<ApplicationUser> userManager;

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        { 
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedAdminAsync(dbContext, userManager);
        }

        private static async Task SeedAdminAsync(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            var admin = await userManager.FindByNameAsync("admin");
            if (admin == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@email.com",
                    NormalizedEmail = "ADMIN@EMAIL.COM",
                };

                var password = "123321";

                var result = await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
