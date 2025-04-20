using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Infrastructure.Data;

namespace TicketGo.Infrastructure.Repositories
{
    public class TrainRepository :  ITrainRepository
    {
        private readonly AppDbContext _context;

        public TrainRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Train>> GetAllAsync()
        {
            return await _context.Trains.ToListAsync();
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

        public async Task<Train> GetByIdAsync(int id)
        {
            return await _context.Trains
                .Include(t => t.IdTrainRouteNavigation)
                .FirstOrDefaultAsync(t => t.IdTrain == id);
        }
        public async Task AddAsync(Train train)
        {
            await _context.Trains.AddAsync(train);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Train train)
        {
            _context.Trains.Update(train);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            if (train != null)
            {
                _context.Trains.Remove(train);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Trains.AnyAsync(t => t.IdTrain == id);
        }
    }
    
}