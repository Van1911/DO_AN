using System.ComponentModel.DataAnnotations;

namespace DO_AN.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
