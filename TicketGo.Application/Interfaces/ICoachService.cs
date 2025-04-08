using TicketGo.Application.DTOs;

namespace TicketGo.Application.Interfaces
{
    public interface ICoachService
    {
        Task<List<CoachDto>> GetAllCoachesAsync();
        Task<CoachDto> GetCoachByIdAsync(int id);
        Task CreateCoachAsync(CreateUpdateCoachDto coachDto);
        Task UpdateCoachAsync(int id, CreateUpdateCoachDto coachDto);
        Task DeleteCoachAsync(int id);
        Task<List<TrainDto>> GetAllTrainsAsync();
    }
}