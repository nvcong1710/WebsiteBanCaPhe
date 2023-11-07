using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteBanCaPhe.Data;
using WebsiteBanCaPhe.Models;

namespace WebsiteBanCaPhe.Controllers
{
	public class CheckoutPageController : Controller
	{
		private readonly WebsiteBanCaPheContext _context;

		public CheckoutPageController(WebsiteBanCaPheContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			var accountId = HttpContext.Session.GetString("AccountId");
			var cart = await _context.Cart.FirstOrDefaultAsync(c => c.AccountId.ToString() == accountId);
			var cartId = cart.CartId;
			var websiteBanCaPheContext = _context.CartDetail
				.Include(c => c.Cart)
				.Include(c => c.Product)
				.Where(c => c.CartId == cartId);
			var cartTotalValue = websiteBanCaPheContext.Sum(c => c.TotalPrice);

			ViewBag.CartTotalValue = cartTotalValue;
			return View(await websiteBanCaPheContext.ToListAsync());
		}
	}
}
