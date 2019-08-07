namespace Palitra27.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class FavouriteProduct
    {
        [Required]
        public string FavouriteListId { get; set; }

        [Required]
        public virtual FavouriteList FavouriteList { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public virtual Product Product { get; set; }
    }
}
