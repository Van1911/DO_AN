namespace TicketGo.Application.DTOs
{
    public class AccountDto
    {
        public int IdAccount { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int IdRole { get; set; }
        public string RoleName { get; set; } // Tên vai trò (từ IdRoleNavigation)
    }
}