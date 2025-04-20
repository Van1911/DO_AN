using TicketGo.Domain.Entities;
using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface ITrainRouteService
    {
        Task<List<TrainRouteDto>> GetAllTrainRoutesAsync();
        Task<TrainRouteDto> GetTrainRouteByIdAsync(int id);
        Task CreateTrainRouteAsync(CreateUpdateTrainRouteDto trainRouteDto);
        Task UpdateTrainRouteAsync(int id, CreateUpdateTrainRouteDto trainRouteDto);
        Task DeleteTrainRouteAsync(int id);
    }
}