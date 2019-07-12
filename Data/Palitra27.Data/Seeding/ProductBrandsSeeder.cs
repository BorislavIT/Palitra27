namespace Palitra27.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Palitra27.Data.Models;

    public class ProductBrandsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ProductsBrands.Any())
            {
                return;
            }

            var productBrandDebeer = new ProductBrand() {Name = "Debeer" };
            var productBrandHBBody = new ProductBrand() {Name = "HBBody" };
            var productBrandNationalPaints = new ProductBrand() {Name = "NationalPaints" };
            var productBrandPalitra = new ProductBrand() {Name = "Palitra" };

            var allProductBrands = new List<ProductBrand>() { productBrandDebeer, productBrandHBBody, productBrandNationalPaints, productBrandPalitra};

            await dbContext.AddRangeAsync(allProductBrands);
        }
    }
}
