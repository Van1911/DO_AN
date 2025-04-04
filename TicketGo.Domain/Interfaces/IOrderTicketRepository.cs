namespace TicketGo.Domain.Interfaces
{
    public interface IOrderTicketRepository : IRepository<OrderTicket>
    {
        Task<OrderTicket> GetByIdAsync(int id);
        Task AddAsync(OrderTicket orderTicket);
        Task UpdateAsync(OrderTicket orderTicket);
        Task DeleteAsync(int id);
    }
}