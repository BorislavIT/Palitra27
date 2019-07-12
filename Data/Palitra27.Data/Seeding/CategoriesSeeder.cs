namespace Palitra27.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Palitra27.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var thinnerCategory = new Category { Name = "Thinner" };
            var sprayCategory = new Category { Name = "Spray" };
            var fillerCategory = new Category { Name = "Filler" };
            var paintCategory = new Category { Name = "Paint" };

            var allCategories = new List<Category>() { thinnerCategory, sprayCategory, fillerCategory, paintCategory };

            await dbContext.Categories.AddRangeAsync(allCategories);
        }
    }
}
