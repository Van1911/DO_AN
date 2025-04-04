namespace TicketGo.Domain.Interfaces
{
    public interface ITrainRouteRepository : IRepository<TrainRoute>
    {
        Task<List<string>> GetStartPointsAsync(string term);
        Task<List<string>> GetEndPointsAsync(string term);
        Task<TrainRoute> GetByIdAsync(int id);
        Task AddAsync(TrainRoute trainRoute);
        Task UpdateAsync(TrainRoute trainRoute);
        Task DeleteAsync(int id);
    }
}