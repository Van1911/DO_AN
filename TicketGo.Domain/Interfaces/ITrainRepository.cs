namespace TicketGo.Domain.Interfaces
{
    public interface ITrainRepository : IRepository<Train>
    {
        Task<List<Train>> SearchTrainsAsync(string pointStart, string pointEnd, DateTime? departureDate, int page, int pageSize);
        Task<Train> GetByIdAsync(int id);
        Task AddAsync(Train train);
        Task UpdateAsync(Train train);
        Task DeleteAsync(int id);
    }
}