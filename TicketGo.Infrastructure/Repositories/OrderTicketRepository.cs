using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Infrastructure.Repositories
{
    public class OrderTicketRepository : IOrderTicketRepository
    {
        private readonly AppDbContext _context;

        public OrderTicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderTicket orderTicket)
        {
            await _context.OrderTickets.AddAsync(orderTicket);
            await _context.SaveChangesAsync();
        }
    }
}