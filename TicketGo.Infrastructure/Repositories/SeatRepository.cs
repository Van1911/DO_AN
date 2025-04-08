using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Infrastructure.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;

        public SeatRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Seat>> GetAllAsync()
        {
            return await _context.Seats.ToListAsync();
        }

        public async Task<Seat> GetByNameAndCoachIdAsync(string nameSeat, int idCoach)
        {
            return await _context.Seats
                .FirstOrDefaultAsync(s => s.NameSeat == nameSeat && s.IdCoach == idCoach);
        }

        public async Task UpdateAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
        }
    }
}