namespace Palitra27.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public int Stars { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public DateTime DateOfCreation { get; set; }
    }
}
