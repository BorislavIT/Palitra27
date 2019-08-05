namespace Palitra27.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Palitra27.Data.Models;

    public class BrandsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Brands.Any())
            {
                return;
            }

            var BrandDebeer = new Brand() { Name = "Debeer" };
            var BrandHBBody = new Brand() { Name = "HBBody" };
            var BrandNationalPaints = new Brand() { Name = "NationalPaints" };
            var BrandPalitra = new Brand() { Name = "Palitra" };

            var allBrands = new List<Brand>() { BrandDebeer, BrandHBBody, BrandNationalPaints, BrandPalitra };

            await dbContext.AddRangeAsync(allBrands);
        }
    }
}
