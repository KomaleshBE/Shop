using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebAppCore.Models;


namespace WebAppCore.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly CateringContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public HomeController(ILogger<HomeController> logger,IHttpContextAccessor httpContextAccessor,CateringContext cateringContext)
        {
            _logger = logger;
            _contextAccessor = httpContextAccessor;
            _context = cateringContext;

        }
        
        public IActionResult Index() 
        {
            string userinfo = "";
            if (_contextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                userinfo = "Admin";
            }
            else
            { 
                userinfo = "none";
            }
            _contextAccessor.HttpContext.Session.SetString("mysession", userinfo);
            //_contextAccessor.HttpContext.Session.Get(userinfo);
            // int count=_context.carts.ToList().Count;
            ////TempData["Items"] = count;
            return View();
        }
        public IActionResult carts()
        {
            List<Cart> carts = _context.carts.ToList();
            return View(carts);
        }
        [Authorize]

        public IActionResult order()
        {
            var user = _contextAccessor.HttpContext.User;
            
                string xx = user.Identity.Name;
                ViewData["totalqtySold"] = _context.carts.Where(e => e.userId == xx).Sum(e => e.qty);
                List<Ordered> ca = _context.orderes.ToList();
                return View(ca);
            
                                 
        }
        public async Task<IActionResult> SelectCategory()
        {
            return View(await _context.categories.ToListAsync());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}