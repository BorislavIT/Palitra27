namespace Palitra27.Data.Models
{
    using System;

    public class Review
    {
        public string Id { get; set; }

        public int Stars { get; set; }

        public string Message { get; set; }

        public string UserName { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime DateOfCreation { get; set; }
    }
}
