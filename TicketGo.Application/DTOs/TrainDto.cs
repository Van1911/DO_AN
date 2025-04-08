namespace TicketGo.Application.DTOs
{
    public class TrainDto
    {
        public int IdTrain { get; set; }
        public string NameTrain { get; set; }
        public DateTime? DateStart { get; set; }
        public int IdTrainRoute { get; set; }
        public string TrainRouteName { get; set; } // Tên tuyến tàu (từ IdTrainRouteNavigation)
        public double? CoefficientTrain { get; set; }
    }
    public class CreateUpdateTrainDto
    {
        public int IdTrain { get; set; }
        public string NameTrain { get; set; }
        public DateTime? DateStart { get; set; }
        public int IdTrainRoute { get; set; }
        public double? CoefficientTrain { get; set; }
    }
}