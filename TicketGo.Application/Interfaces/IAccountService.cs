namespace TicketGo.Application.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task<Account> LoginAsync(string email, string password);
        Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto, HttpContext httpContext);
        string GenerateVerificationCode();
    }
}