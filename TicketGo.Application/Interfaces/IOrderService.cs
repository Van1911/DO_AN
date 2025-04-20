using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderTicketDto> GetOrderTicketDetailsAsync(int idCoach);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task CreateOrderAsync(CreateUpdateOrderDto orderDto);
        Task UpdateOrderAsync(int id, CreateUpdateOrderDto orderDto);
        Task DeleteOrderAsync(int id);
        Task<List<CustomerDto>> GetAllCustomersAsync();
    }
}