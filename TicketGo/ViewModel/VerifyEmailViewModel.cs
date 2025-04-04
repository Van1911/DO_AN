using System.ComponentModel.DataAnnotations;

namespace DO_AN.ViewModel
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã xác thực")]
        public string VerificationCode { get; set; }

        public string Email { get; set; }
    }
}
