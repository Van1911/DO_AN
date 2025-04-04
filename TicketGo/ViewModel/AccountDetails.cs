namespace DO_AN.ViewModel
{
    public class AccountDetails
    {
        public string? Phone { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool? Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
