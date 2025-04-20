﻿using System;
using System.Collections.Generic;

namespace TicketGo.Domain.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderTickets = new HashSet<OrderTicket>();
        }

        public int IdOrder { get; set; }
        public double? UnitPrice { get; set; }
        public DateTime? DateOrder { get; set; }
        public int IdTicket { get; set; }
        public int? IdDiscount { get; set; }
        public string? NameCus { get; set; }
        public string? Phone { get; set; }
        public int? IdCus { get; set; }

        public virtual Customer? IdCusNavigation { get; set; }= null!;
        public virtual Discount? IdDiscountNavigation { get; set; }
        public virtual ICollection<OrderTicket> OrderTickets { get; set; }
    }
}
