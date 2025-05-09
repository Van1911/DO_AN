using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Application.Interfaces;
using Microsoft.AspNetCore.Http;


namespace TicketGo.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRoleRepository _roleRepository;

        public AccountService(IAccountRepository accountRepository, ICustomerRepository customerRepository, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _roleRepository = roleRepository;
        }

        // [Đăng ký tài khoản]
        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            // Tạo tài khoản mới
            var newAccount = new Account // Changed from AccountDto to Account
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
                IdAccount = newAccount.IdAccount,
                FullName = registerDto.Fullname
            };

            // Lưu khách hàng
            await _customerRepository.AddAsync(newCustomer);
            return true;
        }
        //[Đăng nhập tài khoản]
        public async Task<Account> LoginAsync(string email, string password)
        {
            return await _accountRepository.GetByEmailAndPasswordAsync(email, password);
        }

        //[Tìm tài khoản theo email]
        public async Task<Account> GetByEmailAsync(string email)
        {
            return await _accountRepository.GetByEmailAsync(email);
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
        // [Lấy ra danh sách tài khoản]
        public async Task<List<AccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return accounts.Select(a => new AccountDto
            {
                IdAccount = a.IdAccount,
                Phone = a.Phone,
                Email = a.Email,
                Password = a.Password,
                Sex = a.Sex,
                DateOfBirth = a.DateOfBirth,
                IdRole = a.IdRole,
                RoleName = a.IdRoleNavigation?.Name
            }).ToList();
        }

        public async Task<AccountDto> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return null;
            }

            return new AccountDto
            {
                IdAccount = account.IdAccount,
                // Fullname = account.FullName,
                Phone = account.Phone,
                Email = account.Email,
                Password = account.Password,
                Sex = account.Sex,
                DateOfBirth = account.DateOfBirth,
                IdRole = account.IdRole,
                RoleName = account.IdRoleNavigation?.Name
            };
        }
        //[Tạo tài khoản]
        public async Task CreateAccountAsync(AccountDto accountDto)
        {
            var account = new Account
            {
                Phone = accountDto.Phone,
                Email = accountDto.Email,
                Password = accountDto.Password,
                Sex = accountDto.Sex,
                DateOfBirth = accountDto.DateOfBirth,
                IdRole = accountDto.IdRole
            };

            await _accountRepository.AddAsync(account);
        }

        public async Task UpdateAccountAsync(int id, AccountDto accountDto)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                throw new Exception("Account not found");
            }

            account.Phone = accountDto.Phone;
            account.Email = accountDto.Email;
            account.Password = accountDto.Password;
            account.Sex = accountDto.Sex;
            account.DateOfBirth = accountDto.DateOfBirth;
            account.IdRole = accountDto.IdRole;

            await _accountRepository.UpdateAsync(account);
        }

        public async Task DeleteAccountAsync(int id)
        {
            await _accountRepository.DeleteAsync(id);
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(r => new RoleDto
            {
                IdRole = r.IdRole,
                RoleName = r.Name
            }).ToList();
        }
        //[Xac thuc email]
        public async Task<bool> VerifyEmailAsync(string email, string token)
        {
            return await _accountRepository.VerifyEmailAsync(email, token);
        }

        public async Task<string> GenerateTokenAsync(Account account)
        {
            return await _accountRepository.GenerateTokenAsync(account);
        }
    }
}