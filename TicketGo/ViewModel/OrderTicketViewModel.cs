using DO_AN.Models;

namespace DO_AN.ViewModel
{
    public class OrderTicketViewModel
    {
        public Train Train { get; set; }
        public int IdTrain { get; set; }
        public int IdCoach { get; set; }
        public string PointStart { get; set; }
        public string PointEnd { get; set; }
        public string DateStart { get; set; }
        public decimal Price { get; set; }
        public string VehicleType { get; set; }
        public List<Seat> OccupiedSeats { get; set; } // Danh sách các ghế đã đặt
    }
}
