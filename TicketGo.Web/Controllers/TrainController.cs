using Microsoft.AspNetCore.Mvc;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;

namespace TicketGo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : Controller
    {
        private readonly ITrainService _trainService;

        public TrainController(ITrainService trainService)
        {
            _trainService = trainService;
        }

        [HttpGet("GetStartPoints")]
        public async Task<IActionResult> GetStartPoints(string term = "")
        {
            var results = await _trainService.GetStartPointsAsync(term);
            return Ok(results);
        }

        [HttpGet("GetEndPoints")]
        public async Task<IActionResult> GetEndPoints(string term = "")
        {
            var results = await _trainService.GetEndPointsAsync(term);
            return Ok(results);
        }

        [HttpGet("SearchTrain")]
        [Route("/Train/SearchTrain")]
        public async Task<IActionResult> SearchTrain(string? noiDi, string? noiDen, DateTime? ngayKhoiHanh, int page = 1)
        {
            var searchDto = new TrainSearchDto
            {
                PointStart = noiDi,
                PointEnd = noiDen,
                DepartureDate = ngayKhoiHanh,
                Page = page
            };

            var results = await _trainService.SearchTrainsAsync(searchDto);
            return Ok(new
            {
                NoiDi = noiDi,
                NoiDen = noiDen,
                NgayKhoiHanh = ngayKhoiHanh,
                Page = page,
                Trains = results
            });
        }
    }
}