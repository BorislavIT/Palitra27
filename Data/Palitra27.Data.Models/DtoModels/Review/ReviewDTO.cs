namespace Palitra27.Data.Models.DtoModels.Review
{
    using System;

    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Data.Models.DtoModels.Product;

    public class ReviewDTO
    {
        public string Id { get; set; }

        public int Stars { get; set; }

        public string Message { get; set; }

        public string UserName { get; set; }

        public DateTime DateOfCreation { get; set; }
    }
}
