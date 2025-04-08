namespace TicketGo.Domain.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {   
        Task<List<Account>> GetAllAsync();
        Task<Account> GetByIdAsync(int id);
        Task<Account> GetByEmailAsync(string email);
        Task<Account> GetByEmailAndPasswordAsync(string email, string password);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}