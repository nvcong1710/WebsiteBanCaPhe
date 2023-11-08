using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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
            ViewBag.CartDetails = await websiteBanCaPheContext.ToListAsync();
            UserOrder userOrder;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("OrderDate,ReceiverName,PhoneNumber,Address,PaymentMethod,Note,ShippingFee,TotalValue,IsDone,AccountId")] UserOrder userOrder)
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
            ViewBag.CartDetails = await websiteBanCaPheContext.ToListAsync();
            if (ModelState.IsValid)
            {
                userOrder.OrderDate = DateTime.Now;
                userOrder.ShippingFee = 0;
                userOrder.IsDone = false;
                userOrder.AccountId = cart.AccountId;


                _context.Add(userOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "FullName", userOrder.AccountId);
            return View(userOrder);
        }
    }
}
