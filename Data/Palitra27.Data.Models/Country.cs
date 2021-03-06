﻿namespace Palitra27.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 1, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string Name { get; set; }
    }
}
