namespace Palitra27.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class FavouriteList
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<FavouriteProduct> FavouriteProducts { get; set; }
    }
}
