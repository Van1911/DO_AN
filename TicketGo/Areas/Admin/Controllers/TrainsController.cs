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
    public class TrainsController : Controller
    {
        private readonly DOANContext _context;

        public TrainsController(DOANContext context)
        {
            _context = context;
        }

        // GET: Admin/Trains
        public async Task<IActionResult> Index()
        {
            var dOANContext = _context.Trains.Include(t => t.IdTrainRouteNavigation);
            return View(await dOANContext.ToListAsync());
        }

        // GET: Admin/Trains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Trains == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .Include(t => t.IdTrainRouteNavigation)
                .FirstOrDefaultAsync(m => m.IdTrain == id);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // GET: Admin/Trains/Create
        public IActionResult Create()
        {
            ViewData["IdTrainRoute"] = new SelectList(_context.TrainRoutes, "IdTrainRoute", "IdTrainRoute");
            return View();
        }

        // POST: Admin/Trains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTrain,NameTrain,DateStart,IdTrainRoute,CoefficientTrain")] Train train)
        {
            if (ModelState.IsValid)
            {
                _context.Add(train);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTrainRoute"] = new SelectList(_context.TrainRoutes, "IdTrainRoute", "IdTrainRoute", train.IdTrainRoute);
            return View(train);
        }

        // GET: Admin/Trains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trains == null)
            {
                return NotFound();
            }

            var train = await _context.Trains.FindAsync(id);
            if (train == null)
            {
                return NotFound();
            }
            ViewData["IdTrainRoute"] = new SelectList(_context.TrainRoutes, "IdTrainRoute", "IdTrainRoute", train.IdTrainRoute);
            return View(train);
        }

        // POST: Admin/Trains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTrain,NameTrain,DateStart,IdTrainRoute,CoefficientTrain")] Train train)
        {
            if (id != train.IdTrain)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(train);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainExists(train.IdTrain))
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
            ViewData["IdTrainRoute"] = new SelectList(_context.TrainRoutes, "IdTrainRoute", "IdTrainRoute", train.IdTrainRoute);
            return View(train);
        }

        // GET: Admin/Trains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Trains == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .Include(t => t.IdTrainRouteNavigation)
                .FirstOrDefaultAsync(m => m.IdTrain == id);
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
            if (_context.Trains == null)
            {
                return Problem("Entity set 'DOANContext.Trains'  is null.");
            }
            var train = await _context.Trains.FindAsync(id);
            if (train != null)
            {
                _context.Trains.Remove(train);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainExists(int id)
        {
          return (_context.Trains?.Any(e => e.IdTrain == id)).GetValueOrDefault();
        }
    }
}
