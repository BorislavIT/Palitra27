namespace Palitra27.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ShoppingCartProduct
    {
        [Required]
        public string ShoppingCartId { get; set; }

        [Required]
        public virtual ShoppingCart ShoppingCart { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public virtual Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
