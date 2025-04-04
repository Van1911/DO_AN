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
    public class OrdersController : Controller
    {
        private readonly DOANContext _context;

        public OrdersController(DOANContext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            var dOANContext = _context.Orders.Include(o => o.IdCusNavigation).Include(o => o.IdDiscountNavigation);
            return View(await dOANContext.ToListAsync());
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.IdCusNavigation)
                .Include(o => o.IdDiscountNavigation)
                .FirstOrDefaultAsync(m => m.IdOrder == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Admin/Orders/Create
        public IActionResult Create()
        {
            ViewData["IdCus"] = new SelectList(_context.Customers, "IdCus", "IdCus");
            ViewData["IdDiscount"] = new SelectList(_context.Discounts, "IdDiscount", "IdDiscount");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdOrder,UnitPrice,DateOrder,IdTicket,IdDiscount,NameCus,Phone,IdCus")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCus"] = new SelectList(_context.Customers, "IdCus", "IdCus", order.IdCus);
            ViewData["IdDiscount"] = new SelectList(_context.Discounts, "IdDiscount", "IdDiscount", order.IdDiscount);
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["IdCus"] = new SelectList(_context.Customers, "IdCus", "IdCus", order.IdCus);
            ViewData["IdDiscount"] = new SelectList(_context.Discounts, "IdDiscount", "IdDiscount", order.IdDiscount);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdOrder,UnitPrice,DateOrder,IdTicket,IdDiscount,NameCus,Phone,IdCus")] Order order)
        {
            if (id != order.IdOrder)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.IdOrder))
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
            ViewData["IdCus"] = new SelectList(_context.Customers, "IdCus", "IdCus", order.IdCus);
            ViewData["IdDiscount"] = new SelectList(_context.Discounts, "IdDiscount", "IdDiscount", order.IdDiscount);
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.IdCusNavigation)
                .Include(o => o.IdDiscountNavigation)
                .FirstOrDefaultAsync(m => m.IdOrder == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'DOANContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.IdOrder == id)).GetValueOrDefault();
        }
    }
}
