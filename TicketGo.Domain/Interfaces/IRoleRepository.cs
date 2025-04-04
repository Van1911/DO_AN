namespace TicketGo.Domain.Interfaces{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetByIdAsync(int id);
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(int id);
    }
}