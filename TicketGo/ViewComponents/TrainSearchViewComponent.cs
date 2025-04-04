using DO_AN.Models;
using DO_AN.ViewModel.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DO_AN.ViewComponents
{
    public class TrainSearchViewComponent : ViewComponent
    {
        private readonly DOANContext _context;

        public TrainSearchViewComponent(DOANContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string noiDi, string noiDen, DateTime? ngayKhoiHanh, int page = 1, int pageSize = 2)
        {
            var xe = _context.Coaches
                .Include(t => t.IdTrainNavigation)
                .ThenInclude(t => t.IdTrainRouteNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(noiDi))
            {
                xe = xe.Where(v => v.IdTrainNavigation.IdTrainRouteNavigation.PointStart.Contains(noiDi));
            }
            if (!string.IsNullOrEmpty(noiDen))
            {
                xe = xe.Where(v => v.IdTrainNavigation.IdTrainRouteNavigation.PointEnd.Contains(noiDen));
            }
            if (ngayKhoiHanh.HasValue)
            {
                var date = ngayKhoiHanh.Value.Date;
                xe = xe.Where(v => v.IdTrainNavigation.DateStart.HasValue && v.IdTrainNavigation.DateStart.Value.Date == date);
            }

            int totalItem = await xe.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItem / (double)pageSize);

            var pagedTickets = await xe
                .OrderBy(v => v.IdTrain)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new TrainListViewModel
            {
                coaches = pagedTickets,
                PagingInfo = new PagingSearch
                {
                    TotalItems = totalItem,
                    ItemsPerPage = pageSize,
                    CurrentPage = page
                }
            };

            ViewBag.NoiDi = noiDi;
            ViewBag.NoiDen = noiDen;
            ViewBag.NgayKhoiHanh = ngayKhoiHanh;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            return View("Default", viewModel); // Return the view component with its corresponding view
        }

    }
}
