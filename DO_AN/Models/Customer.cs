﻿using System;
using System.Collections.Generic;

namespace DO_AN.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int IdCus { get; set; }
        public string? FullName { get; set; }
        public int IdAccount { get; set; }

        public virtual Account IdAccountNavigation { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
