namespace Palitra27.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Palitra27.Data.Models;
    using Palitra27.Web.ViewModels.Shop;

    public interface IShopService
    {
        IQueryable<Product> Find(ShopViewModel model);
    }
}
