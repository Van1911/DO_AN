using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Application.Interfaces;

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
        public async Task<List<TrainDto>> GetAllTrainsAsync()
        {
            var trains = await _trainRepository.GetAllAsync();
            return trains.Select(t => new TrainDto
            {
                IdTrain = t.IdTrain,
                NameTrain = t.NameTrain,
                DateStart = t.DateStart,
                IdTrainRoute = t.IdTrainRoute,
                TrainRouteName = t.IdTrainRouteNavigation != null 
                    ? $"{t.IdTrainRouteNavigation.PointStart} - {t.IdTrainRouteNavigation.PointEnd}" 
                    : null,
                CoefficientTrain = (double?)t.CoefficientTrain
            }).ToList();
        }

        public async Task<TrainDto> GetTrainByIdAsync(int id)
        {
            var train = await _trainRepository.GetByIdAsync(id);
            if (train == null)
            {
                return null;
            }

            return new TrainDto
            {
                IdTrain = train.IdTrain,
                NameTrain = train.NameTrain,
                DateStart = train.DateStart,
                IdTrainRoute = train.IdTrainRoute,
                TrainRouteName = train.IdTrainRouteNavigation != null 
                    ? $"{train.IdTrainRouteNavigation.PointStart} - {train.IdTrainRouteNavigation.PointEnd}" 
                    : null,
                CoefficientTrain =  (double?)train.CoefficientTrain
            };
        }

        public async Task CreateTrainAsync(CreateUpdateTrainDto trainDto)
        {
            var train = new Train
            {
                NameTrain = trainDto.NameTrain,
                DateStart = trainDto.DateStart,
                IdTrainRoute = trainDto.IdTrainRoute,
                CoefficientTrain =  (decimal?)trainDto.CoefficientTrain
            };

            await _trainRepository.AddAsync(train);
        }

        public async Task UpdateTrainAsync(int id, CreateUpdateTrainDto trainDto)
        {
            var train = await _trainRepository.GetByIdAsync(id);
            if (train == null)
            {
                throw new Exception("Train not found");
            }

            train.NameTrain = trainDto.NameTrain;
            train.DateStart = trainDto.DateStart;
            train.IdTrainRoute = trainDto.IdTrainRoute;
            train.CoefficientTrain =  (decimal?)trainDto.CoefficientTrain;

            await _trainRepository.UpdateAsync(train);
        }

        public async Task DeleteTrainAsync(int id)
        {
            await _trainRepository.DeleteAsync(id);
        }

        public async Task<List<TrainRouteDto>> GetAllTrainRoutesAsync()
        {
            var trainRoutes = await _trainRouteRepository.GetAllAsync();
            return trainRoutes.Select(tr => new TrainRouteDto
            {
                IdTrainRoute = tr.IdTrainRoute,
                PointStart = tr.PointStart,
                PointEnd = tr.PointEnd
            }).ToList();
        }
    }
}