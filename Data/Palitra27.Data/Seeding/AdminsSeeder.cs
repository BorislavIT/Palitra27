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
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedAdminAsync(dbContext, userManager);
        }

        private static async Task SeedAdminAsync(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            var admin = await userManager.FindByNameAsync(GlobalConstants.AdminUsername);
            if (admin == null)
            {
                string id = Guid.NewGuid().ToString();
                var shoppingCart = new ShoppingCart() { Id = id };
                var FavouriteList = new FavouriteList() { Id = id };
                var user = new ApplicationUser
                {
                    Id = id,
                    UserName = GlobalConstants.AdminUsername,
                    NormalizedUserName = "ADMIN",
                    Email = "admin@email.com",
                    NormalizedEmail = "ADMIN@EMAIL.COM",
                    ShoppingCart = shoppingCart,
                    ShoppingCartId = id,
                    FavouriteList = FavouriteList,
                    FavouriteListId = FavouriteList.Id,
                };

                FavouriteList.User = user;
                FavouriteList.UserId = user.Id;

                var password = "123321";
                await dbContext.ShoppingCarts.AddAsync(shoppingCart);
                await dbContext.FavouriteLists.AddAsync(FavouriteList);
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
