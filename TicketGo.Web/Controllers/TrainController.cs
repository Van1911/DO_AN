using Microsoft.AspNetCore.Mvc;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace TicketGo.Web.Controllers
{
    public class TrainController : Controller
    {
        private readonly ITrainService _trainService;

        public TrainController(ITrainService trainService)
        {
            _trainService = trainService;
        }

        [HttpGet]
        public async Task<IActionResult> ListTrain([FromQuery] TrainSearchRequest request)
        {
            var result = await _trainService.SearchTrainsAsync(request);
            return View("ListTrain", result); 
        }

        [HttpGet("start-points")]
        public async Task<IActionResult> GetStartPoints(string term) =>
            Ok(await _trainService.GetStartPointsAsync(term));

        [HttpGet("end-points")]
        public async Task<IActionResult> GetEndPoints(string term) =>
            Ok(await _trainService.GetEndPointsAsync(term));
    }
}