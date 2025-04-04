using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DO_AN.Models;

namespace DO_AN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoachesController : Controller
    {
        private readonly DOANContext _context;

        public CoachesController(DOANContext context)
        {
            _context = context;
        }

        // GET: Admin/Coaches
        public async Task<IActionResult> Index()
        {
            var dOANContext = _context.Coaches.Include(c => c.IdTrainNavigation);
            return View(await dOANContext.ToListAsync());
        }

        // GET: Admin/Coaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Coaches == null)
            {
                return NotFound();
            }

            var coach = await _context.Coaches
                .Include(c => c.IdTrainNavigation)
                .FirstOrDefaultAsync(m => m.IdCoach == id);
            if (coach == null)
            {
                return NotFound();
            }

            return View(coach);
        }

        // GET: Admin/Coaches/Create
        public IActionResult Create()
        {
            ViewData["IdTrain"] = new SelectList(_context.Trains, "IdTrain", "IdTrain");
            return View();
        }

        // POST: Admin/Coaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCoach,NameCoach,Category,SeatsQuantity,BasicPrice,IdTrain")] Coach coach)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTrain"] = new SelectList(_context.Trains, "IdTrain", "IdTrain", coach.IdTrain);
            return View(coach);
        }

        // GET: Admin/Coaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Coaches == null)
            {
                return NotFound();
            }

            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null)
            {
                return NotFound();
            }
            ViewData["IdTrain"] = new SelectList(_context.Trains, "IdTrain", "IdTrain", coach.IdTrain);
            return View(coach);
        }

        // POST: Admin/Coaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCoach,NameCoach,Category,SeatsQuantity,BasicPrice,IdTrain")] Coach coach)
        {
            if (id != coach.IdCoach)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoachExists(coach.IdCoach))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTrain"] = new SelectList(_context.Trains, "IdTrain", "IdTrain", coach.IdTrain);
            return View(coach);
        }

        // GET: Admin/Coaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Coaches == null)
            {
                return NotFound();
            }

            var coach = await _context.Coaches
                .Include(c => c.IdTrainNavigation)
                .FirstOrDefaultAsync(m => m.IdCoach == id);
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
            if (_context.Coaches == null)
            {
                return Problem("Entity set 'DOANContext.Coaches'  is null.");
            }
            var coach = await _context.Coaches.FindAsync(id);
            if (coach != null)
            {
                _context.Coaches.Remove(coach);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoachExists(int id)
        {
          return (_context.Coaches?.Any(e => e.IdCoach == id)).GetValueOrDefault();
        }
    }
}
