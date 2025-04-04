using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface ITrainService
    {
        Task<List<string>> GetStartPointsAsync(string term);
        Task<List<string>> GetEndPointsAsync(string term);
        Task<List<TrainResultDto>> SearchTrainsAsync(TrainSearchDto searchDto);
    }
}