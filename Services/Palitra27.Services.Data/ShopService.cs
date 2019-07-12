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

            if (model.PriceUpper == 0)
            {
                products = this.context.Products
                    .Where(p => p.Price >= model.PriceLower);
                return products;
            }

            products = this.context.Products
                .Where(p => p.Price >= model.PriceLower && p.Price <= model.PriceUpper);
            return products;
        }
    }
}
