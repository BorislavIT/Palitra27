namespace Palitra27.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class FavouriteProduct
    {
        public string FavouriteListId { get; set; }
        public virtual FavouriteList FavouriteList { get; set; }

        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
