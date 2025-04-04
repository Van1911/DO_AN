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
    public class TrainRoutesController : Controller
    {
        private readonly DOANContext _context;

        public TrainRoutesController(DOANContext context)
        {
            _context = context;
        }

        // GET: Admin/TrainRoutes
        public async Task<IActionResult> Index()
        {
              return _context.TrainRoutes != null ? 
                          View(await _context.TrainRoutes.ToListAsync()) :
                          Problem("Entity set 'DOANContext.TrainRoutes'  is null.");
        }

        // GET: Admin/TrainRoutes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TrainRoutes == null)
            {
                return NotFound();
            }

            var trainRoute = await _context.TrainRoutes
                .FirstOrDefaultAsync(m => m.IdTrainRoute == id);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTrainRoute,PointStart,PointEnd")] TrainRoute trainRoute)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainRoute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainRoute);
        }

        // GET: Admin/TrainRoutes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TrainRoutes == null)
            {
                return NotFound();
            }

            var trainRoute = await _context.TrainRoutes.FindAsync(id);
            if (trainRoute == null)
            {
                return NotFound();
            }
            return View(trainRoute);
        }

        // POST: Admin/TrainRoutes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTrainRoute,PointStart,PointEnd")] TrainRoute trainRoute)
        {
            if (id != trainRoute.IdTrainRoute)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainRoute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainRouteExists(trainRoute.IdTrainRoute))
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
            return View(trainRoute);
        }

        // GET: Admin/TrainRoutes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TrainRoutes == null)
            {
                return NotFound();
            }

            var trainRoute = await _context.TrainRoutes
                .FirstOrDefaultAsync(m => m.IdTrainRoute == id);
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
            if (_context.TrainRoutes == null)
            {
                return Problem("Entity set 'DOANContext.TrainRoutes'  is null.");
            }
            var trainRoute = await _context.TrainRoutes.FindAsync(id);
            if (trainRoute != null)
            {
                _context.TrainRoutes.Remove(trainRoute);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainRouteExists(int id)
        {
          return (_context.TrainRoutes?.Any(e => e.IdTrainRoute == id)).GetValueOrDefault();
        }
    }
}
