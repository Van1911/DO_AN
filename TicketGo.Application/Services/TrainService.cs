using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Application.Services
{
    public class TrainService : ITrainService
    {
        private readonly ITrainRouteRepository _trainRouteRepository;
        private readonly ITrainRepository _trainRepository;
        private const int PageSize = 10; // Số lượng bản ghi trên mỗi trang

        public TrainService(ITrainRouteRepository trainRouteRepository, ITrainRepository trainRepository)
        {
            _trainRouteRepository = trainRouteRepository;
            _trainRepository = trainRepository;
        }

        public async Task<List<string>> GetStartPointsAsync(string term)
        {
            return await _trainRouteRepository.GetStartPointsAsync(term);
        }

        public async Task<List<string>> GetEndPointsAsync(string term)
        {
            return await _trainRouteRepository.GetEndPointsAsync(term);
        }

        public async Task<List<TrainResultDto>> SearchTrainsAsync(TrainSearchDto searchDto)
        {
            var trains = await _trainRepository.SearchTrainsAsync(
                searchDto.PointStart,
                searchDto.PointEnd,
                searchDto.DepartureDate,
                searchDto.Page,
                PageSize);

            return trains.Select(t => new TrainResultDto
            {
                IdTrain = t.IdTrain,
                PointStart = t.IdTrainRouteNavigation?.PointStart,
                PointEnd = t.IdTrainRouteNavigation?.PointEnd,
                DateStart = t.DateStart
            }).ToList();
        }
    }
}