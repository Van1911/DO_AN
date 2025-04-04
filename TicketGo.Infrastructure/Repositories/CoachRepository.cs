using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Infrastructure.Repositories
{
    public class CoachRepository : ICoachRepository
    {
        private readonly AppDbContext _context;

        public CoachRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Coach> GetCoachWithRelatedDataAsync(int idCoach)
        {
            return await _context.Coaches
                .Include(c => c.Seats)
                .Include(c => c.IdTrainNavigation)
                    .ThenInclude(t => t.IdTrainRouteNavigation)
                .FirstOrDefaultAsync(c => c.IdCoach == idCoach);
        }
    }
}