namespace TicketGo.Application.DTOs
{
    public class TrainRouteDto
    {
        public int IdTrainRoute { get; set; }
        public string PointStart { get; set; }
        public string PointEnd { get; set; }
    }

    public class CreateUpdateTrainRouteDto
    {
        public int IdTrainRoute { get; set; }
        public string PointStart { get; set; }
        public string PointEnd { get; set; }
    }
}