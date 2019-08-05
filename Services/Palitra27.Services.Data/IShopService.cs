namespace Palitra27.Services.Data
{
    using System.Linq;

    using Palitra27.Data.Models;
    using Palitra27.Web.ViewModels.Shop;

    public interface IShopService
    {
        IQueryable<Product> Find(ShopViewModel model);
    }
}
