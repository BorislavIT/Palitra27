namespace Palitra27.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Web.ViewModels.Shop;

    public class ShopService : IShopService
    {
        private readonly ApplicationDbContext context;

        public ShopService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Product> Find(ShopViewModel model)
        {
            IQueryable<Product> products;
            products = this.context.Products;

            var brand = this.context.Brands.FirstOrDefault(b => b.Name == model.Brand);

            var category = this.context.Categories.FirstOrDefault(c => c.Name == model.Category);

            if (brand == null && category == null)
            {
                if (model.PriceUpper == 0)
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower);
                }
                else
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower && p.Price <= model.PriceUpper);
                }
            }
            else if (brand == null)
            {
                if (model.PriceUpper == 0)
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower)
                        .Where(c => c.Category == category);
                }
                else
                {
                    products = products
                     .Where(p => p.Price >= model.PriceLower && p.Price <= model.PriceUpper)
                     .Where(c => c.Category == category);
                }
            }
            else if (category == null)
            {
                if (model.PriceUpper == 0)
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower)
                        .Where(p => p.Brand == brand);
                }
                else
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower && p.Price <= model.PriceUpper)
                        .Where(x => x.Brand == brand);
                }
            }

            if (brand != null && category != null)
            {
                if (model.PriceUpper == 0)
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower)
                        .Where(p => p.Category == category && p.Brand == brand);
                }
                else
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower && p.Price <= model.PriceUpper)
                        .Where(p => p.Category == category && p.Brand == brand);
                }
            }

            switch (model.Sorting)
            {
                case "pASC":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "pDESC":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "nAZ":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "nZA":
                    products = products.OrderByDescending(p => p.Name);
                    break;
            }

            return products;
        }
    }
}
