namespace TicketGo.Domain.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<List<Coach>> GetAllAsync();
        Task<bool> ExistsAsync(int id);
        Task<Customer> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
    }
}