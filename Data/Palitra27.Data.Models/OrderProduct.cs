namespace Palitra27.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class OrderProduct
    {
        [Required]
        public string OrderId { get; set; }

        [Required]
        public virtual Order Order { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public virtual Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
