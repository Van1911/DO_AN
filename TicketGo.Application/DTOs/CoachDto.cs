namespace TicketGo.Application.DTOs
{
    public class CoachDto
    {
        public int IdCoach { get; set; }
        public string NameCoach { get; set; }
        public string Category { get; set; }
        public int? SeatsQuantity { get; set; }
        public decimal? BasicPrice { get; set; }
        public int IdTrain { get; set; }
        public string TrainName { get; set; } // Tên tàu (từ IdTrainNavigation)
    }
    
    public class CreateUpdateCoachDto
    {
        public int IdCoach { get; set; }
        public string NameCoach { get; set; }
        public string Category { get; set; }
        public int? SeatsQuantity { get; set; }
        public decimal? BasicPrice { get; set; }
        public int IdTrain { get; set; }
    }
}