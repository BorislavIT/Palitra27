namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ShoppingCartProduct;

    public interface IShoppingCartService
    {
        void AddProductInShoppingCart(string productId, string username, int? quntity = null);

        void EditProductQuantityInShoppingCart(string productId, string username, int quantity);

        IEnumerable<ShoppingCartProductDTO> GetAllShoppingCartProducts(string username);

        IEnumerable<ShoppingCartProduct> GetAllDomainShoppingCartProducts(string username);

        void DeleteProductFromShoppingCart(string id, string username);

        void DeleteAllProductFromShoppingCart(string username);

        bool AnyProducts(string username);
    }
}