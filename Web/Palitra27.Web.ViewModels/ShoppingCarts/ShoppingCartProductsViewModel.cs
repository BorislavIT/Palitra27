namespace Palitra27.Web.ViewModels.ShoppingCart
{
    public class ShoppingCartProductsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
