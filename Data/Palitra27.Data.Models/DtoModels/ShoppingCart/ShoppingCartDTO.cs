namespace Palitra27.Data.Models.DtoModels.ShoppingCart
{
    using System.Collections.Generic;

    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Data.Models.DtoModels.ShoppingCartProduct;

    public class ShoppingCartDTO
    {
        public string Id { get; set; }

        public virtual ApplicationUserDTO User { get; set; }

        public virtual ICollection<ShoppingCartProductDTO> ShoppingCartProducts { get; set; }
    }
}
