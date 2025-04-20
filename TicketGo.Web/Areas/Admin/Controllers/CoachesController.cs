using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace TicketGo.Web.Areas.Admin.Controllers
{   
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CoachesController : Controller
    {
        private readonly ICoachService _coachService;

        public CoachesController(ICoachService coachService)
        {
            _coachService = coachService;
        }

        // GET: Admin/Coaches
        public async Task<IActionResult> Index()
        {
            var coaches = await _coachService.GetAllCoachesAsync();
            return View(coaches);
        }

        // GET: Admin/Coaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = await _coachService.GetCoachByIdAsync(id.Value);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        // GET: Admin/Coaches/Create
        public async Task<IActionResult> Create()
        {
            var trains = await _coachService.GetAllTrainsAsync();
            ViewData["IdTrain"] = new SelectList(trains, "IdTrain", "TrainName");
            return View();
        }

        // POST: Admin/Coaches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUpdateCoachDto coachDto)
        {
            if (ModelState.IsValid)
            {
                await _coachService.CreateCoachAsync(coachDto);
                return RedirectToAction(nameof(Index));
            }

            var trains = await _coachService.GetAllTrainsAsync();
            ViewData["IdTrain"] = new SelectList(trains, "IdTrain", "TrainName", coachDto.IdTrain);
            return View(coachDto);
        }

        // GET: Admin/Coaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = await _coachService.GetCoachByIdAsync(id.Value);
            if (coach == null)
            {
                return NotFound();
            }

            var trains = await _coachService.GetAllTrainsAsync();
            ViewData["IdTrain"] = new SelectList(trains, "IdTrain", "TrainName", coach.IdTrain);

            var coachDto = new CreateUpdateCoachDto
            {
                IdCoach = coach.IdCoach,
                NameCoach = coach.NameCoach,
                Category = coach.Category,
                SeatsQuantity = coach.SeatsQuantity,
                BasicPrice = coach.BasicPrice,
                IdTrain = coach.IdTrain.Value
            };

            return View(coachDto);
        }

        // POST: Admin/Coaches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateUpdateCoachDto coachDto)
        {
            if (id != coachDto.IdCoach)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _coachService.UpdateCoachAsync(id, coachDto);
                }
                catch (Exception)
                {
                    if (await _coachService.GetCoachByIdAsync(id) != null)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var trains = await _coachService.GetAllTrainsAsync();
            ViewData["IdTrain"] = new SelectList(trains, "IdTrain", "TrainName", coachDto.IdTrain);
            return View(coachDto);
        }

        // GET: Admin/Coaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coach = await _coachService.GetCoachByIdAsync(id.Value);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        // POST: Admin/Coaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _coachService.DeleteCoachAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}