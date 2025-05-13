namespace TicketGo.Application.DTOs
{

    public class OrderDto
    {
        public int IdOrder { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? DateOrder { get; set; }
        
        public string? DiscountName { get; set; } // Thêm thuộc tính này để lưu tên giảm giá
        public int? IdTicket { get; set; }
        public int? IdDiscount { get; set; }
        public string NameCus { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int? IdAccount { get; set; }
        public int? IdCoach { get; set; } // Thêm để lưu IdCoach
        public List<string> ListSeats { get; set; } = new List<string>(); // Thêm để lưu danh sách ghế
    }
}