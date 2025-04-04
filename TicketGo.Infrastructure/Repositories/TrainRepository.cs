using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Infrastructure.Repositories
{
    public class TrainRepository : ITrainRepository
    {
        private readonly AppDbContext _context;

        public TrainRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Train>> SearchTrainsAsync(string pointStart, string pointEnd, DateTime? departureDate, int page, int pageSize)
        {
            var query = _context.Trains
                .Include(t => t.IdTrainRouteNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(pointStart))
            {
                query = query.Where(t => t.IdTrainRouteNavigation.PointStart.Contains(pointStart));
            }

            if (!string.IsNullOrEmpty(pointEnd))
            {
                query = query.Where(t => t.IdTrainRouteNavigation.PointEnd.Contains(pointEnd));
            }

            if (departureDate.HasValue)
            {
                query = query.Where(t => t.DateStart.HasValue && t.DateStart.Value.Date == departureDate.Value.Date);
            }

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}