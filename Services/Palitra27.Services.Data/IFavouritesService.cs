namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Palitra27.Web.ViewModels.FavouriteList;

    public interface IFavouritesService
    {
        void AddProduct(string productId, string username);

        void RemoveProduct(string productId, string username);

        List<FavouriteProductViewModel> AllFavouriteProducts(string username);
    }
}
