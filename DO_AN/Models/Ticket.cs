﻿using System;
using System.Collections.Generic;

namespace DO_AN.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            OrderTickets = new HashSet<OrderTicket>();
        }

        public int IdTicket { get; set; }
        public DateTime? Date { get; set; }
        public double? Price { get; set; }
        public int IdSeat { get; set; }
        public int IdTrain { get; set; }

        public virtual Seat IdSeatNavigation { get; set; } // Assuming Seat model is defined elsewhere
        public virtual Train IdTrainNavigation { get; set; } // Navigation property to Train
        public virtual ICollection<OrderTicket> OrderTickets { get; set; }
    }
}