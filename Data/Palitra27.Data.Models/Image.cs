namespace Palitra27.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public virtual Product Product { get; set; }
    }
}
