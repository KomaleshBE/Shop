using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppCore.Models;

namespace WebAppCore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CateringContext _context;
       private readonly IHttpContextAccessor _contextAccessor;

        public ProductsController(CateringContext context,IHttpContextAccessor contextAccessor)
        {
            _context = context;
           _contextAccessor = contextAccessor;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var cateringContext = _context.products.Include(p => p.categ);
            return View(await cateringContext.ToListAsync());
        }
        public async Task<IActionResult> SelectedProducts(int cid)
        {
            var cateringContext = _context.products.Where(e => e.categID == cid).Include(p => p.categ);
            return View(await cateringContext.ToListAsync());
        }
        // GET: Products/Details/5
        //[HttpPost, Authorize]
        [HttpPost,Authorize]
        public async Task<IActionResult> SelectedProducts(List<Product> plist)
        {
            //    try
            //    {

            //        foreach (Product p in plist)
            //        {

            //            if (p.check)
            //            {
            //                Cart c1 = new Cart();
            //                c1.pid = p.id;
            //                c1.price = p.price;
            //                c1.qty = 10;
            //                c1.name= p.name;
            //                c1.userId = "xx";

            //                //c1.name = _contextAccessor.HttpContext.User.Identity.Name;
            //                _context.carts.Add(c1);
            //            }

            //        }

            //        //ViewBag.items = _context.carts.ToList().Count();
            //        _context.SaveChanges();
            //        return RedirectToAction("SelectCategory", "Categories", null);
            //    }
            //    catch
            //    {
            //        return View(plist);
            //    }
            //}
            var user = _contextAccessor.HttpContext.User;
            string xx = user.Identity.Name;
            foreach (Product p in plist)
            {
                if (p.check)
                {
                    Cart c1 = new Cart();
                    c1.userId = xx;
                    c1.qty = 1;
                    c1.price = p.price;
                    c1.name = p.name;
                    _context.carts.Add(c1);
                }
            }
            ViewBag.items = _context.carts.ToList().Count();
            _context.SaveChanges();
            return RedirectToAction("SelectCategory", "Categories", null);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .Include(p => p.categ)
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["categID"] = new SelectList(_context.categories, "id", "name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("id,name,categID,imagePath,price,check")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["categID"] = new SelectList(_context.categories, "id", "name", product.categID);
        //    return View(product);
        //}
        public async Task<IActionResult> Create(Product product)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
            //create folder if not existif (!Directory.Exists(path))
            Directory.CreateDirectory(path);
            //get file extension
            FileInfo fileInfo = new FileInfo(product.photo.FileName);
            string fileName = product.photo.FileName;// + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                product.photo.CopyTo(stream);
            }



            Product p = new Product
            {
                imagePath = "~/Images/" + product.photo.FileName,
                id = product.id,
                name = product.name,
                categID = product.categID,
                price = product.price,
                check = product.check
            };
            if (ModelState.IsValid)
            {
                _context.Add(p);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["categID"] = new SelectList(_context.categories, "id", "id", product.categID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,categID,imagePath,price,check")] Product product)
        {
            if (id != product.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.id))
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
            ViewData["categID"] = new SelectList(_context.categories, "id", "id", product.categID);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .Include(p => p.categ)
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.products == null)
            {
                return Problem("Entity set 'CateringContext.products'  is null.");
            }
            var product = await _context.products.FindAsync(id);
            if (product != null)
            {
                _context.products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.products.Any(e => e.id == id);
        }
    }
}
