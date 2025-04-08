using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IPagedList<Ticket>> GetPagedTicketsAsync(int pageNumber, int pageSize)
        {
            return await _context.Tickets
                .Include(t => t.IdSeatNavigation)
                .Include(t => t.IdTrainNavigation)
                .OrderBy(x => x.IdTicket)
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<Ticket> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.IdSeatNavigation)
                .Include(t => t.IdTrainNavigation)
                .FirstOrDefaultAsync(t => t.IdTicket == id);
        }
        
        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Tickets.AnyAsync(t => t.IdTicket == id);
        }
    }
}