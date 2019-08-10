namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ShoppingCartProduct;

    public interface IShoppingCartsService
    {
        void AddProductInShoppingCart(string id, string username, int? quntity = null);

        void EditProductQuantityInShoppingCart(string id, string username, int quantity);

        List<ShoppingCartProductDTO> FindAllShoppingCartProducts(string username);

        List<ShoppingCartProduct> FindAllDomainShoppingCartProducts(string username);

        void RemoveProductFromShoppingCart(string id, string username);

        void RemoveAllProductsFromShoppingCart(string username);

        bool AnyProducts(string username);
    }
}