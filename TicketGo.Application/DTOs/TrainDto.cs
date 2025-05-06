namespace TicketGo.Application.DTOs
{
    public class TrainDto
    {
        public int? IdTrain { get; set; }
        public string NameTrain { get; set; }= null!;
        public DateTime? DateStart { get; set; }
        public int IdTrainRoute { get; set; }
        public string? TrainRouteName { get; set; } // Tên tuyến tàu (từ IdTrainRouteNavigation)
        public double? CoefficientTrain { get; set; }
    }

    public class TrainSearchRequest : PagingRequest
    {
        public string? NoiDi { get; set; }
        public string? NoiDen { get; set; }
        public DateTime? NgayKhoiHanh { get; set; }
        public string? Sort { get; set; }
    }

    
    public class TrainResponseDto
    {
        public int Id { get; set; }
        public string TenTau { get; set; } = default!;
        public string NoiDi { get; set; } = default!;
        public string NoiDen { get; set; } = default!;
        public DateTime GioKhoiHanh { get; set; }
        public decimal GiaVe { get; set; }
    }

}