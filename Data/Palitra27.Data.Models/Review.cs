namespace Palitra27.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Review
    {
        public string Id { get; set; }

        public int Stars { get; set; }

        public string Message { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public DateTime DateOfCreation { get; set; }
    }
}
