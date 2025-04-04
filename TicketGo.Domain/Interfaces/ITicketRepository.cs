namespace TicketGo.Domain.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<Ticket> GetByIdAsync(int id);
        Task AddAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(int id);
    } 
}
