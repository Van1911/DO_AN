using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Application.Services
{
    public class CoachService : ICoachService
    {
        private readonly ICoachRepository _coachRepository;
        private readonly ITrainRepository _trainRepository;

        public CoachService(ICoachRepository coachRepository, ITrainRepository trainRepository)
        {
            _coachRepository = coachRepository;
            _trainRepository = trainRepository;
        }

        public async Task<List<CoachDto>> GetAllCoachesAsync()
        {
            var coaches = await _coachRepository.GetAllAsync();
            return coaches.Select(c => new CoachDto
            {
                IdCoach = c.IdCoach,
                NameCoach = c.NameCoach,
                Category = c.Category,
                SeatsQuantity = c.SeatsQuantity,
                BasicPrice = c.BasicPrice,
                IdTrain = c.IdTrain,
                TrainName = c.IdTrainNavigation?.IdTrain.ToString() // Có thể thay bằng thuộc tính phù hợp của Train
            }).ToList();
        }

        public async Task<CoachDto> GetCoachByIdAsync(int id)
        {
            var coach = await _coachRepository.GetByIdAsync(id);
            if (coach == null)
            {
                return null;
            }

            return new CoachDto
            {
                IdCoach = coach.IdCoach,
                NameCoach = coach.NameCoach,
                Category = coach.Category,
                SeatsQuantity = coach.SeatsQuantity,
                BasicPrice = coach.BasicPrice,
                IdTrain = coach.IdTrain,
                TrainName = coach.IdTrainNavigation?.IdTrain.ToString() // Có thể thay bằng thuộc tính phù hợp của Train
            };
        }

        public async Task CreateCoachAsync(CreateUpdateCoachDto coachDto)
        {
            var coach = new Coach
            {
                NameCoach = coachDto.NameCoach,
                Category = coachDto.Category,
                SeatsQuantity = coachDto.SeatsQuantity,
                BasicPrice = coachDto.BasicPrice,
                IdTrain = coachDto.IdTrain
            };

            await _coachRepository.AddAsync(coach);
        }

        public async Task UpdateCoachAsync(int id, CreateUpdateCoachDto coachDto)
        {
            var coach = await _coachRepository.GetByIdAsync(id);
            if (coach == null)
            {
                throw new Exception("Coach not found");
            }

            coach.NameCoach = coachDto.NameCoach;
            coach.Category = coachDto.Category;
            coach.SeatsQuantity = coachDto.SeatsQuantity;
            coach.BasicPrice = coachDto.BasicPrice;
            coach.IdTrain = coachDto.IdTrain;

            await _coachRepository.UpdateAsync(coach);
        }

        public async Task DeleteCoachAsync(int id)
        {
            await _coachRepository.DeleteAsync(id);
        }

        public async Task<List<TrainDto>> GetAllTrainsAsync()
        {
            var trains = await _trainRepository.GetAllAsync();
            return trains.Select(t => new TrainDto
            {
                IdTrain = t.IdTrain,
                TrainName = t.IdTrain.ToString() // Có thể thay bằng thuộc tính phù hợp của Train
            }).ToList();
        }
    }
}