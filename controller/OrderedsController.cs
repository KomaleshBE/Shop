using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppCore.Models;

namespace WebAppCore.Controllers
{
    public class OrderedsController : Controller
    {
        private readonly CateringContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrderedsController(CateringContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

       
        // GET: Ordereds
        public async Task<IActionResult> Index()
        {
            var user = _contextAccessor.HttpContext.User;

            string xx = user.Identity.Name;
            ViewData["total"] = _context.carts.Where(e => e.userId == xx).Sum(e => e.price);
            return View(await _context.orderes.ToListAsync());
        }

        // GET: Ordereds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.orderes == null)
            {
                return NotFound();
            }

            var ordered = await _context.orderes
                .FirstOrDefaultAsync(m => m.Oid == id);
            if (ordered == null)
            {
                return NotFound();
            }

            return View(ordered);
        }

        // GET: Ordereds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ordereds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Oid,name,qty,price,totalPrice,userid")] Ordered ordered)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordered);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ordered);
        }

        // GET: Ordereds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.orderes == null)
            {
                return NotFound();
            }

            var ordered = await _context.orderes.FindAsync(id);
            if (ordered == null)
            {
                return NotFound();
            }
            return View(ordered);
        }

        // POST: Ordereds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Oid,name,qty,price,totalPrice,userid")] Ordered ordered)
        {
            if (id != ordered.Oid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordered);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderedExists(ordered.Oid))
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
            return View(ordered);
        }

        // GET: Ordereds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.orderes == null)
            {
                return NotFound();
            }

            var ordered = await _context.orderes
                .FirstOrDefaultAsync(m => m.Oid == id);
            if (ordered == null)
            {
                return NotFound();
            }

            return View(ordered);
        }

        // POST: Ordereds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.orderes == null)
            {
                return Problem("Entity set 'CateringContext.orderes'  is null.");
            }
            var ordered = await _context.orderes.FindAsync(id);
            if (ordered != null)
            {
                _context.orderes.Remove(ordered);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderedExists(int id)
        {
          return _context.orderes.Any(e => e.Oid == id);
        }
    }
}
