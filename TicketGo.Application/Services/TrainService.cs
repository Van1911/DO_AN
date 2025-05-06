using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Application.Interfaces;
using Microsoft.EntityFrameworkCore;


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

        public async Task<PagedResult<TrainResponseDto>> SearchTrainsAsync(TrainSearchRequest request)
        {
            var query = _trainRepository.GetQueryable()
                .Where(t => t.IdTrainRouteNavigation != null)
                .AsQueryable(); // ← KHÔNG cần .Where(t => !t.IsDeleted)

            if (!string.IsNullOrWhiteSpace(request.NoiDi))
                query = query.Where(t => t.IdTrainRouteNavigation.PointStart.Contains(request.NoiDi));

            if (!string.IsNullOrWhiteSpace(request.NoiDen))
                query = query.Where(t => t.IdTrainRouteNavigation.PointEnd.Contains(request.NoiDen));

            if (request.NgayKhoiHanh.HasValue)
                query = query.Where(t => t.DateStart.HasValue && t.DateStart.Value.Date == request.NgayKhoiHanh.Value.Date);

            query = request.Sort switch
            {
                "fare:asc" => query.OrderBy(t => t.CoefficientTrain),
                "fare:desc" => query.OrderByDescending(t => t.CoefficientTrain),
                "time:asc" => query.OrderBy(t => t.DateStart),
                "time:desc" => query.OrderByDescending(t => t.DateStart),
                _ => query.OrderBy(t => t.IdTrain)
            };

            var totalRecords = await query.CountAsync();

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => new TrainResponseDto
                {
                    Id = t.IdTrain,
                    TenTau = t.NameTrain,
                    NoiDi = t.IdTrainRouteNavigation.PointStart,
                    NoiDen = t.IdTrainRouteNavigation.PointEnd,
                    GioKhoiHanh = t.DateStart ?? DateTime.MinValue,
                    GiaVe = (t.CoefficientTrain ?? 1) * 100000
                })
                .ToListAsync();

            return new PagedResult<TrainResponseDto>
            {
                Items = items,
                TotalRecords = totalRecords,
                Page = request.Page,
                PageSize = request.PageSize
            };
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

        public async Task CreateTrainAsync(TrainDto trainDto)
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

        public async Task UpdateTrainAsync(int id, TrainDto trainDto)
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