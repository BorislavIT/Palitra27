namespace Palitra27.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Palitra27.Common;
    using Palitra27.Data.Models;

    public class ProductsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Products.Any())
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var admin = await userManager.FindByNameAsync(GlobalConstants.AdminUsername);

            var brandNational = dbContext.Brands.FirstOrDefault(b => b.Name == "NationalPaints");
            var brandHBbody = dbContext.Brands.FirstOrDefault(b => b.Name == "HBBody");
            var brandPalitra = dbContext.Brands.FirstOrDefault(b => b.Name == "Palitra");
            var brandDebeer = dbContext.Brands.FirstOrDefault(b => b.Name == "Debeer");
            var allBrands = new List<Brand>() { brandNational, brandHBbody, brandPalitra, brandDebeer };

            var categoryFiller = dbContext.Categories.FirstOrDefault(c => c.Name == "Filler");
            var categorySpray = dbContext.Categories.FirstOrDefault(c => c.Name == "Spray");
            var categoryThinner = dbContext.Categories.FirstOrDefault(c => c.Name == "Thinner");
            var categoryPaint = dbContext.Categories.FirstOrDefault(c => c.Name == "Paint");
            var allCategories = new List<Category>() { categoryFiller, categorySpray, categoryThinner, categoryPaint };

            var random = new Random();

            var productsList = new List<Product>();

            for (int i = 0; i < allBrands.Count; i++)
            {
                for (int z = 0; z < 10; z++)
                {
                    var review = new Review() { Message = "Very good", Stars = 4, User = admin, UserId = admin.Id, DateOfCreation = DateTime.UtcNow };
                    var list = new List<Review>() { review };

                    var product = new Product { Name = $"product-{z}", Brand = allBrands[i], Category = allCategories[i], Image = "https://www.hbbody.com.gr/images/products/SPRAY-FILL.png", Price = random.Next(0, 20), Reviews = list };
                    productsList.Add(product);
                }
            }

            await dbContext.Products.AddRangeAsync(productsList);
        }
    }
}
