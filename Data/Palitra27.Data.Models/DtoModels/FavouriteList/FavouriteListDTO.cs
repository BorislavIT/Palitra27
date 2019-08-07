namespace Palitra27.Data.Models.DtoModels.FavouriteList
{
    using System.Collections.Generic;

    public class FavouriteListDTO
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<FavouriteProduct> FavouriteProducts { get; set; }
    }
}
