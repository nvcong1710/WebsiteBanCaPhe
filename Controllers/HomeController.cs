
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebsiteBanCaPhe.Data;
using WebsiteBanCaPhe.Models;

namespace WebsiteBanCaPhe.Controllers
{
    public class HomeController : Controller
    {
		private readonly WebsiteBanCaPheContext _context;

		public HomeController(WebsiteBanCaPheContext context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
            return View(_context.Product.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string searchString)
        {
            var product = _context.Product.Where(p => p.ProductName.Contains(searchString));
            return View(product.ToList());
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
