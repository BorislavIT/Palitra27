﻿namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ShoppingCartProduct;

    public interface IShoppingCartService
    {
        void AddProductInShoppingCart(string id, string username, int? quntity = null);

        void EditProductQuantityInShoppingCart(string id, string username, int quantity);

        List<ShoppingCartProductDTO> FindAllShoppingCartProducts(string username);

        List<ShoppingCartProduct> FindAllDomainShoppingCartProducts(string username);

        void DeleteProductFromShoppingCart(string id, string username);

        void DeleteAllProductFromShoppingCart(string username);

        bool AnyProducts(string username);
    }
}