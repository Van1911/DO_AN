using System;
using System.Collections.Generic;

namespace DO_AN.Models
{
    public partial class Train
    {
        public Train()
        {
            Coaches = new HashSet<Coach>();
            Tickets = new HashSet<Ticket>();
        }

        public int IdTrain { get; set; }
        public string? NameTrain { get; set; }
        public DateTime? DateStart { get; set; }
        public int IdTrainRoute { get; set; }
        public decimal? CoefficientTrain { get; set; }

        public virtual TrainRoute IdTrainRouteNavigation { get; set; }
        public virtual ICollection<Coach> Coaches { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
