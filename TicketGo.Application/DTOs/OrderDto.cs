namespace TicketGo.Application.DTOs
{
    public class OrderDto
    {
        public int IdOrder { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? DateOrder { get; set; }
        public int? IdTicket { get; set; }
        public int? IdDiscount { get; set; }
        public string DiscountName { get; set; } = null!;
        public string NameCus { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int? IdCus { get; set; }
        public string CustomerName { get; set; } = null!;
        public List<TicketDto> Tickets { get; set; } = new List<TicketDto>();
    }

    public class CreateUpdateOrderDto
    {
        public int IdOrder { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? DateOrder { get; set; }
        public int? IdTicket { get; set; }
        public int? IdDiscount { get; set; }
        public string NameCus { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int? IdCus { get; set; }
        public int? IdCoach { get; set; } // Thêm để lưu IdCoach
        public List<string> ListSeats { get; set; } = new List<string>(); // Thêm để lưu danh sách ghế
    }
}