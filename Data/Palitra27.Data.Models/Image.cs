namespace Palitra27.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Image
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
