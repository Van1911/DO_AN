using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderTicketDto> GetOrderTicketDetailsAsync(int idCoach);
        Task CreateOrderAsync(CreateOrderDto createOrderDto);
    }
}