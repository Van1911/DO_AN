using System;
using System.Collections.Generic;

namespace DO_AN.Models
{
    public partial class OrderTicket
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdTicket { get; set; }

        public virtual Order IdOrderNavigation { get; set; } 
        public virtual Ticket IdTicketNavigation { get; set; } 
    }
}
