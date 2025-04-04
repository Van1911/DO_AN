using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;

        public AccountService(IAccountRepository accountRepository, ICustomerRepository customerRepository)
        {
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            // Tạo tài khoản mới
            var newAccount = new Account
            {
                Email = registerDto.Email,
                Password = registerDto.Password,
                Phone = registerDto.Phone,
                Sex = registerDto.Sex,
                DateOfBirth = registerDto.DateOfBirth,
                IdRole = 2 // Giả sử ID vai trò cho người dùng thường
            };

            // Lưu tài khoản
            await _accountRepository.AddAsync(newAccount);

            // Tạo khách hàng mới liên kết với tài khoản
            var newCustomer = new Customer
            {
                IdCus = newAccount.IdAccount,
                IdAccount = newAccount.IdAccount,
                FullName = registerDto.FullName
            };

            // Lưu khách hàng
            await _customerRepository.AddAsync(newCustomer);

            return true;
        }

        public async Task<Account> LoginAsync(string email, string password)
        {
            return await _accountRepository.GetByEmailAndPasswordAsync(email, password);
        }

        public async Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto, HttpContext httpContext)
        {
            // Lấy mã xác thực từ Session và kiểm tra thời gian hết hạn
            var savedVerificationCode = httpContext.Session.GetString("VerificationCode");
            var expirationTime = httpContext.Session.GetInt32("VerificationCodeExpiration");

            if (savedVerificationCode == null || DateTime.UtcNow.Minute >= expirationTime)
            {
                return false; // Mã không tồn tại hoặc đã hết hạn
            }

            if (verifyEmailDto.VerificationCode != savedVerificationCode)
            {
                return false; // Mã không khớp
            }

            // Tìm tài khoản dựa trên email
            var account = await _accountRepository.GetByEmailAsync(verifyEmailDto.Email);
            if (account == null)
            {
                return false; // Không tìm thấy tài khoản
            }

            // Đánh dấu tài khoản là đã xác thực (nếu cần)
            // Ví dụ: account.IsEmailVerified = true;
            // await _accountRepository.UpdateAsync(account);

            // Xóa mã xác thực khỏi Session
            httpContext.Session.Remove("VerificationCode");
            httpContext.Session.Remove("VerificationCodeExpiration");

            return true;
        }

        public string GenerateVerificationCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6); // Lấy 6 ký tự đầu tiên
        }
    }
}   