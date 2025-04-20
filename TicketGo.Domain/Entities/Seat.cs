using System;
using System.Collections.Generic;

namespace TicketGo.Domain.Entities
{
    public partial class Seat
    {
        public Seat()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int IdSeat { get; set; }
        public string? NameSeat { get; set; } = null!;
        public bool State { get; set; }
        public int? IdCoach { get; set; }

        public virtual Coach? IdCoachNavigation { get; set; }= null!;
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
