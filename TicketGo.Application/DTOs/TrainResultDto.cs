namespace TicketGo.Application.DTOs
{
    public class TrainResultDto
    {
        public int IdTrain { get; set; }
        public string PointStart { get; set; }
        public string PointEnd { get; set; }
        public DateTime? DateStart { get; set; }
        // Các thuộc tính khác nếu cần
    }
}