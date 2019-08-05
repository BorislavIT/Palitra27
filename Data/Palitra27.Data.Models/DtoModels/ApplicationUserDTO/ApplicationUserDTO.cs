namespace Palitra27.Data.Models.DtoModels.ApplicationUserDTO
{
    using System;
    using System.Collections.Generic;
    using Palitra27.Data.Models.DtoModels.FavouriteList;
    using Palitra27.Data.Models.DtoModels.Order;
    using Palitra27.Data.Models.DtoModels.ShoppingCart;

    public class ApplicationUserDTO
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<OrderDTO> Orders { get; set; }

        public string ShoppingCartId { get; set; }
        public virtual ShoppingCartDTO ShoppingCart { get; set; }

        public string FavouriteListId { get; set; }
        public virtual FavouriteListDTO FavouriteList { get; set; }
    }
}
