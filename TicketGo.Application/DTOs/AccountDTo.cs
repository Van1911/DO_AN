namespace TicketGo.Application.DTOs
{
    public class AccountDto
    {
        public int IdAccount { get; set; }
        // public string Fullname { get; set; } = null!;
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int IdRole { get; set; }
        public string RoleName { get; set; } // Tên vai trò (từ IdRoleNavigation)
    }

   public class CreateUpdateAccountDto
    {
        public int IdAccount { get; set; }
        public string Fullname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool? Sex { get; set; } 
        public DateTime? DateOfBirth { get; set; } 
        public int IdRole { get; set; }
    }

}