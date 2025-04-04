namespace TicketGo.Domain.Interfaces
{
    public interface ICoachRepository : IRepository<Coach>
    {
        Task<Coach> GetCoachWithRelatedDataAsync(int idCoach);
        Task<Coach> GetByIdAsync(int id);
        Task AddAsync(Coach coach);
        Task UpdateAsync(Coach coach);
        Task DeleteAsync(int id);
    }
}