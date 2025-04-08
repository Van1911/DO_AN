using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;

namespace TicketGo.Web.Areas.Admin.Controllers
{   
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class TrainsController : Controller
    {
        private readonly ITrainService _trainService;

        public TrainsController(ITrainService trainService)
        {
            _trainService = trainService;
        }

        // GET: Admin/Trains
        public async Task<IActionResult> Index()
        {
            var trains = await _trainService.GetAllTrainsAsync();
            return View(trains);
        }

        // GET: Admin/Trains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _trainService.GetTrainByIdAsync(id.Value);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // GET: Admin/Trains/Create
        public async Task<IActionResult> Create()
        {
            var trainRoutes = await _trainService.GetAllTrainRoutesAsync();
            ViewData["IdTrainRoute"] = new SelectList(trainRoutes, "IdTrainRoute", "PointStart", "PointEnd");
            return View();
        }

        // POST: Admin/Trains/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUpdateTrainDto trainDto)
        {
            if (ModelState.IsValid)
            {
                await _trainService.CreateTrainAsync(trainDto);
                return RedirectToAction(nameof(Index));
            }

            var trainRoutes = await _trainService.GetAllTrainRoutesAsync();
            ViewData["IdTrainRoute"] = new SelectList(trainRoutes, "IdTrainRoute", "PointStart", trainDto.IdTrainRoute);
            return View(trainDto);
        }

        // GET: Admin/Trains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _trainService.GetTrainByIdAsync(id.Value);
            if (train == null)
            {
                return NotFound();
            }

            var trainRoutes = await _trainService.GetAllTrainRoutesAsync();
            ViewData["IdTrainRoute"] = new SelectList(trainRoutes, "IdTrainRoute", "PointStart", train.IdTrainRoute);

            var trainDto = new CreateUpdateTrainDto
            {
                IdTrain = train.IdTrain,
                NameTrain = train.NameTrain,
                DateStart = train.DateStart,
                IdTrainRoute = train.IdTrainRoute,
                CoefficientTrain = train.CoefficientTrain
            };

            return View(trainDto);
        }

        // POST: Admin/Trains/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateUpdateTrainDto trainDto)
        {
            if (id != trainDto.IdTrain)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _trainService.UpdateTrainAsync(id, trainDto);
                }
                catch (Exception)
                {
                    if (!await _trainService.GetTrainByIdAsync(id) != null)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var trainRoutes = await _trainService.GetAllTrainRoutesAsync();
            ViewData["IdTrainRoute"] = new SelectList(trainRoutes, "IdTrainRoute", "PointStart", trainDto.IdTrainRoute);
            return View(trainDto);
        }

        // GET: Admin/Trains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _trainService.GetTrainByIdAsync(id.Value);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // POST: Admin/Trains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _trainService.DeleteTrainAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}