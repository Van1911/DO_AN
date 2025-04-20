using System;
using System.Collections.Generic;

namespace TicketGo.Domain.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int IdCus { get; set; }
        public string? FullName { get; set; }= null!;
        public int IdAccount { get; set; }

        public virtual Account IdAccountNavigation { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
