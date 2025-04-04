using DO_AN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DO_AN.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly DOANContext _context;
       

        public HomeController(DOANContext context)
        {
            _context = context;
        }

        public IActionResult TrangChu()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            return View();
        }


        //[HttpGet("GetStartPoints")]
        //public async Task<IActionResult> GetStartPoints(string term = "")
        //{
        //    var query = _context.TrainRoutes.AsQueryable();

        //    if (!string.IsNullOrEmpty(term))
        //    {
        //        query = query.Where(r => r.PointStart.Contains(term));
        //    }

        //    var results = await query.Select(r => r.PointStart)
        //                             .Distinct()
        //                             .ToListAsync();

        //    return Ok(results);
        //}

        public IActionResult GetStartPoints(string term = "")
        {
            var query = _context.TrainRoutes.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(r => r.PointStart.Contains(term));
            }

            var results = query.Select(r => r.PointStart)
                               .Distinct()
                               .ToList();

            return Json(results);
        }

        public IActionResult GetEndPoints(string term = "")
        {
            var query = _context.TrainRoutes.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(r => r.PointEnd.Contains(term));
            }

            var results = query.Select(r => r.PointEnd)
                               .Distinct()
                               .ToList();

            return Json(results);
        }
        //[HttpGet("GetEndPoints")]
        //public async Task<IActionResult> GetEndPoints(string term = "")
        //{
        //    var query = _context.TrainRoutes.AsQueryable();

        //    if (!string.IsNullOrEmpty(term))
        //    {
        //        query = query.Where(r => r.PointEnd.Contains(term));
        //    }

        //    var results = await query.Select(r => r.PointEnd)
        //                             .Distinct()
        //                             .ToListAsync();

        //    return Ok(results);
        //}

    }
}


