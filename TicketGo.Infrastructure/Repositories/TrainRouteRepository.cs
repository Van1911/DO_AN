using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Infrastructure.Repositories
{
    public class TrainRouteRepository : ITrainRouteRepository
    {
        private readonly AppDbContext _context;

        public TrainRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetStartPointsAsync(string term)
        {
            var query = _context.TrainRoutes.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(r => r.PointStart.Contains(term));
            }

            return await query.Select(r => r.PointStart)
                              .Distinct()
                              .ToListAsync();
        }

        public async Task<List<string>> GetEndPointsAsync(string term)
        {
            var query = _context.TrainRoutes.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(r => r.PointEnd.Contains(term));
            }

            return await query.Select(r => r.PointEnd)
                              .Distinct()
                              .ToListAsync();
        }
    }
}