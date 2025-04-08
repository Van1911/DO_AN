namespace TicketGo.Domain.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IPagedList<Ticket>> GetPagedTicketsAsync(int pageNumber, int pageSize);
        Task<Ticket> GetByIdAsync(int id);
        Task AddAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    } 
}
