using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _context.Accounts
                .Include(a => a.IdRoleNavigation)
                .ToListAsync();
        }

        public async Task AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Account> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        }
        
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Accounts.AnyAsync(a => a.IdAccount == id);
        }
    }
}