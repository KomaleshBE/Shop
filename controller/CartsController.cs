using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppCore.Models;

namespace WebAppCore.Controllers
{
    public class CartsController : Controller
    {
        private readonly CateringContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public CartsController(CateringContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
              return View(await _context.carts.ToListAsync());
        }

        
         public ActionResult placeorder() 
        {
            var user = _contextAccessor.HttpContext.User;
            
                string xx = user.Identity.Name;
                ViewData["total"] = _context.carts.Where(e => e.userId == xx).Sum(e => e.price);
                 return View(_context.carts.ToList()); 
        }
        [HttpPost]
        public ActionResult placeorder(List<Cart> plist)
        {
            var user = _contextAccessor.HttpContext.User;
            string xx = user.Identity.Name;
            
            float bill = 0;
            foreach (Cart p in plist)
            {
                if (p.check)
                {
                    Ordered p1 = new Ordered();
                    p1.userid = xx;
                    p1.qty = 1; 
                    p1.price = p.price;
                    p1.name = p.name;
                    bill += p.price * p1.qty;
                    p1.totalPrice = p1.qty*p1.price;
                    _context.orderes.Add(p1);
                }
            }

            ViewBag.items = _context.orderes.ToList().Count();
            _context.SaveChanges(); 
            return RedirectToAction("placeorder", "Carts", null);
        }



        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.carts == null)
            {
                return NotFound();
            }

            var cart = await _context.carts
                .FirstOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,userId,name,pid,qty,price,check")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.carts == null)
            {
                return NotFound();
            }

            var cart = await _context.carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,userId,name,pid,qty,price,check")] Cart cart)
        {
            if (id != cart.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.id))
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
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.carts == null)
            {
                return NotFound();
            }

            var cart = await _context.carts
                .FirstOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.carts == null)
            {
                return Problem("Entity set 'CateringContext.carts'  is null.");
            }
            var cart = await _context.carts.FindAsync(id);
            if (cart != null)
            {
                _context.carts.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
          return _context.carts.Any(e => e.id == id);
        }
    }
}
