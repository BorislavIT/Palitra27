namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Web.ViewModels.Shop;

    public interface IShopService
    {
        List<ProductDTO> Find(ShopViewModel model);
    }
}
