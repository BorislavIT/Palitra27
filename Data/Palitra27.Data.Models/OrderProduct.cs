namespace Palitra27.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OrderProduct
    {
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }

        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
