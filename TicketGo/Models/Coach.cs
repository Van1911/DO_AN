using System;
using System.Collections.Generic;

namespace DO_AN.Models
{
    public partial class Coach
    {
        public Coach()
        {
            Seats = new HashSet<Seat>();
        }

        public int IdCoach { get; set; }
        public string? NameCoach { get; set; }
        public string? Category { get; set; }
        public int? SeatsQuantity { get; set; }
        public double? BasicPrice { get; set; }
        public int? IdTrain { get; set; }

        public virtual Train? IdTrainNavigation { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
