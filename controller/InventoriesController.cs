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
    
    public class InventoriesController : Controller
    {
        private readonly CateringContext _context;

        public InventoriesController(CateringContext context)
        {
            _context = context;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            var cateringContext = _context.inventories.Include(i => i.cid).Include(i => i.product);
            return View(await cateringContext.ToListAsync());
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.inventories
                .Include(i => i.cid)
                .Include(i => i.product)
                .FirstOrDefaultAsync(m => m.id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }
        public JsonResult FilterProducts(int id)
        {
            var plist = _context.products.Where(e => e.categID == id).Select(e => new
            {
                id = e.id,
                name=e.name
            });
            return Json(plist);
        }
        public JsonResult GetProducts()
        {
            string[] products = _context.products.Select(e => e.name).ToArray();            
            string c=string.Join(',',products);            
            return Json(c);
        }


        // GET: Inventories/Create
        public IActionResult Create()
        {
            ViewData["categId"] = new SelectList(_context.categories, "id", "name");
            ViewData["pid"] = new SelectList(_context.products, "id", "name");
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,pid,categId,qty")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["categId"] = new SelectList(_context.categories, "id", "name", inventory.categId);
            ViewData["pid"] = new SelectList(_context.products, "id", "name", inventory.pid);
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            ViewData["categId"] = new SelectList(_context.categories, "id", "name", inventory.categId);
            ViewData["pid"] = new SelectList(_context.products, "id", "name", inventory.pid);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,pid,categId,qty")] Inventory inventory)
        {
            if (id != inventory.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.id))
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
            ViewData["categId"] = new SelectList(_context.categories, "id", "name", inventory.categId);
            ViewData["pid"] = new SelectList(_context.products, "id", "name", inventory.pid);
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.inventories
                .Include(i => i.cid)
                .Include(i => i.product)
                .FirstOrDefaultAsync(m => m.id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.inventories == null)
            {
                return Problem("Entity set 'CateringContext.inventories'  is null.");
            }
            var inventory = await _context.inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.inventories.Remove(inventory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
          return _context.inventories.Any(e => e.id == id);
        }
    }
}
