namespace TicketGo.Application.DTOs
{
    public class TrainSearchDto
    {
        public string PointStart { get; set; }
        public string PointEnd { get; set; }
        public DateTime? DepartureDate { get; set; }
        public int Page { get; set; }
    }
}