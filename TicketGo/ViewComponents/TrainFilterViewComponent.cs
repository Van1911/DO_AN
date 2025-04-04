using DO_AN.Models;
using DO_AN.ViewModel;
using DO_AN.ViewModel.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DO_AN.ViewComponents
{
    public class TrainFilterViewComponent : ViewComponent
    {
        private readonly DOANContext _context;
         
        public TrainFilterViewComponent(DOANContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string noiDi, string noiDen, int page = 1, int pageSize = 2)
        {
            var routeQuery = _context.TrainRoutes.AsQueryable();

            //// Apply filters only when provided
            //if (!string.IsNullOrEmpty(noiDi))
            //{
            //    routeQuery = routeQuery.Where(v => v.PointStart.Contains(noiDi));
            //}
            //if (!string.IsNullOrEmpty(noiDen))
            //{
            //    routeQuery = routeQuery.Where(v => v.PointStart.Contains(noiDen));
            //}

            //int totalRoutes = await routeQuery.CountAsync();
            //int totalPages = (int)Math.Ceiling(totalRoutes / (double)pageSize);

            var pagedRoutes = await routeQuery
                .OrderBy(v => v.IdTrainRoute)
                //.Skip((page - 1) * pageSize)
                //.Take(pageSize)
                .ToListAsync();

            var viewModel = new TrainListViewModel
            {
                TrainRoutes = pagedRoutes
            };

            ViewBag.NoiDi = noiDi;
            ViewBag.NoiDen = noiDen;
            //ViewBag.Page = page;
            //ViewBag.TotalPages = totalPages;

            return View("Default", viewModel); // Return the view component with its corresponding view
        }
    }
}
