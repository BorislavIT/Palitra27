namespace Palitra27.Data.Models.DtoModels.ShoppingCartProduct
{
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Data.Models.DtoModels.ShoppingCart;

    public class ShoppingCartProductDTO
    {
        public string ShoppingCartId { get; set; }
        public virtual ShoppingCartDTO ShoppingCart { get; set; }

        public string ProductId { get; set; }
        public virtual ProductDTO Product { get; set; }

        public int Quantity { get; set; }
    }
}
