namespace TicketGo.Application.DTOs
{
    public class OrderDto
    {
        public int IdOrder { get; set; }
        public double? UnitPrice { get; set; }
        public DateTime? DateOrder { get; set; }
        public int? IdTicket { get; set; }
        public int? IdDiscount { get; set; }
        public string DiscountName { get; set; } // Tên mã giảm giá (từ IdDiscountNavigation)
        public string NameCus { get; set; }
        public string Phone { get; set; }
        public int? IdCus { get; set; }
        public string CustomerName { get; set; } // Tên khách hàng (từ IdCusNavigation)
    }
    public class CreateUpdateOrderDto
    {
        public int IdOrder { get; set; }
        public double? UnitPrice { get; set; }
        public DateTime? DateOrder { get; set; }
        public int? IdTicket { get; set; }
        public int? IdDiscount { get; set; }
        public string NameCus { get; set; }
        public string Phone { get; set; }
        public int? IdCus { get; set; }
    }
}