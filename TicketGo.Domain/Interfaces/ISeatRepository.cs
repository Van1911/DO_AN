namespace TicketGo.Domain.Interfaces
{
    public interface ISeatRepository : IRepository<Seat>
    {
        Task<List<Seat>> GetAllAsync();
        Task<Seat> GetByNameAndCoachIdAsync(string nameSeat, int idCoach);
        Task<Seat> GetByIdAsync(int id);
        Task AddAsync(Seat seat);
        Task UpdateAsync(Seat seat);
        Task DeleteAsync(int id);
    }
}