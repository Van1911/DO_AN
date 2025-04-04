using System;
using System.Collections.Generic;

namespace DO_AN.Models
{
    public partial class Discount
    {
        public Discount()
        {
            Orders = new HashSet<Order>();
        }

        public int IdDiscount { get; set; }
        public string? NameDiscount { get; set; }
        public string? Information { get; set; }
        public int? PercentDiscount { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
