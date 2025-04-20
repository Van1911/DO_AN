using TicketGo.Domain.Entities;
using TicketGo.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace TicketGo.Application.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(AccountDto registerDto);
        Task<Account> LoginAsync(string email, string password);
        Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto, HttpContext httpContext);
        string GenerateVerificationCode();

        Task<List<AccountDto>> GetAllAccountsAsync();
        Task<AccountDto> GetAccountByIdAsync(int id);
        Task CreateAccountAsync(CreateUpdateAccountDto accountDto);
        Task UpdateAccountAsync(int id, CreateUpdateAccountDto accountDto);
        Task DeleteAccountAsync(int id);
        Task<List<RoleDto>> GetAllRolesAsync();
    }
}