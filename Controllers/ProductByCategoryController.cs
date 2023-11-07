using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteBanCaPhe.Data;
using WebsiteBanCaPhe.Models;

namespace WebsiteBanCaPhe.Controllers
{
	public class ProductByCategoryController : Controller
	{
		private readonly WebsiteBanCaPheContext _context;

		public ProductByCategoryController(WebsiteBanCaPheContext context)
		{
			_context = context;
		}
		public IActionResult Index(int? id)
		{
			var productByCategory = _context.Product
				.Where(x => x.CategoryId == id);
			return View(productByCategory.ToList());
		}
	}
}
