using DO_AN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DO_AN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : Controller
    {
        private readonly DOANContext _context;

        public TrainController(DOANContext context)
        {
            _context = context;
        }

        [HttpGet("GetStartPoints")]
        public async Task<IActionResult> GetStartPoints(string term = "")
        {
            var query = _context.TrainRoutes.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(r => r.PointStart.Contains(term));
            }

            var results = await query.Select(r => r.PointStart)
                                     .Distinct()
                                     .ToListAsync();

            return Ok(results);
        }


        [HttpGet("GetEndPoints")]
        public async Task<IActionResult> GetEndPoints(string term = "")
        {
            var query = _context.TrainRoutes.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(r => r.PointEnd.Contains(term));
            }

            var results = await query.Select(r => r.PointEnd)
                                     .Distinct()
                                     .ToListAsync();

            return Ok(results);
        }

        [HttpGet("SearchTrain")]
        [Route("/Train/SearchTrain")]
        public async Task<IActionResult> SearchTrain(string? noiDi, string? noiDen, DateTime? ngayKhoiHanh, int page = 1)
        {
            ViewBag.NoiDi = noiDi;
            ViewBag.NoiDen = noiDen;
            ViewBag.NgayKhoiHanh = ngayKhoiHanh;
            ViewBag.Page = page;
            return View();
        }
    }
}
