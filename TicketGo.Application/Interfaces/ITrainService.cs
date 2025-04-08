using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface ITrainService
    {
        Task<List<string>> GetStartPointsAsync(string term);
        Task<List<string>> GetEndPointsAsync(string term);
        Task<List<TrainResultDto>> SearchTrainsAsync(TrainSearchDto searchDto);
        Task<List<TrainDto>> GetAllTrainsAsync();
        Task<TrainDto> GetTrainByIdAsync(int id);
        Task CreateTrainAsync(CreateUpdateTrainDto trainDto);
        Task UpdateTrainAsync(int id, CreateUpdateTrainDto trainDto);
        Task DeleteTrainAsync(int id);
        Task<List<TrainRouteDto>> GetAllTrainRoutesAsync();
    }
}