using Microsoft.AspNetCore.Mvc;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;

namespace TicketGo.Web.Areas.Admin.Controllers
{   
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class TrainRoutesController : Controller
    {
        private readonly ITrainRouteService _trainRouteService;

        public TrainRoutesController(ITrainRouteService trainRouteService)
        {
            _trainRouteService = trainRouteService;
        }

        // GET: Admin/TrainRoutes
        public async Task<IActionResult> Index()
        {
            var trainRoutes = await _trainRouteService.GetAllTrainRoutesAsync();
            return View(trainRoutes);
        }

        // GET: Admin/TrainRoutes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainRoute = await _trainRouteService.GetTrainRouteByIdAsync(id.Value);
            if (trainRoute == null)
            {
                return NotFound();
            }

            return View(trainRoute);
        }

        // GET: Admin/TrainRoutes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TrainRoutes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUpdateTrainRouteDto trainRouteDto)
        {
            if (ModelState.IsValid)
            {
                await _trainRouteService.CreateTrainRouteAsync(trainRouteDto);
                return RedirectToAction(nameof(Index));
            }

            return View(trainRouteDto);
        }

        // GET: Admin/TrainRoutes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainRoute = await _trainRouteService.GetTrainRouteByIdAsync(id.Value);
            if (trainRoute == null)
            {
                return NotFound();
            }

            var trainRouteDto = new CreateUpdateTrainRouteDto
            {
                IdTrainRoute = trainRoute.IdTrainRoute,
                PointStart = trainRoute.PointStart,
                PointEnd = trainRoute.PointEnd
            };

            return View(trainRouteDto);
        }

        // POST: Admin/TrainRoutes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateUpdateTrainRouteDto trainRouteDto)
        {
            if (id != trainRouteDto.IdTrainRoute)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _trainRouteService.UpdateTrainRouteAsync(id, trainRouteDto);
                }
                catch (Exception)
                {
                    if (!await _trainRouteService.GetTrainRouteByIdAsync(id) != null)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(trainRouteDto);
        }

        // GET: Admin/TrainRoutes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainRoute = await _trainRouteService.GetTrainRouteByIdAsync(id.Value);
            if (trainRoute == null)
            {
                return NotFound();
            }

            return View(trainRoute);
        }

        // POST: Admin/TrainRoutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _trainRouteService.DeleteTrainRouteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}