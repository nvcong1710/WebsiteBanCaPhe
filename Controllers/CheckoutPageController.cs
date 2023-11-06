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

		// GET: UserOrders/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.UserOrder == null)
			{
				return NotFound();
			}

			var userOrder = await _context.UserOrder
				.Include(u => u.Account)
				.FirstOrDefaultAsync(m => m.OrderId == id);
			if (userOrder == null)
			{
				return NotFound();
			}

			return View(userOrder);
		}

		// GET: UserOrders/Create
		public IActionResult Create()
		{
			ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "FullName");
			return View();
		}

		// POST: UserOrders/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("OrderId,ReceiverName,PhoneNumber,Address,PaymentMethod,Note")] UserOrder userOrder)
		{
			if (ModelState.IsValid)
			{
				var accountId = HttpContext.Session.GetString("AccountId");
				var cart = await _context.Cart.FirstOrDefaultAsync(c => c.AccountId.ToString() == accountId);
				var cartId = cart.CartId;


				_context.Add(userOrder);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "FullName", userOrder.AccountId);
			return View(userOrder);
		}
	}
}
