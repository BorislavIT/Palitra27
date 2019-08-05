namespace Palitra27.Data.Models.DtoModels.Review
{
    using System;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.Product;

    public class ReviewDTO
    {
        public string Id { get; set; }

        public int Stars { get; set; }

        public string Message { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ProductId { get; set; }

        public virtual ProductDTO Product { get; set; }

        public DateTime DateOfCreation { get; set; }
    }
}
