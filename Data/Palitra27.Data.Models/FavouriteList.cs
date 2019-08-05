namespace Palitra27.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class FavouriteList
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<FavouriteProduct> FavouriteProducts { get; set; }
    }
}
