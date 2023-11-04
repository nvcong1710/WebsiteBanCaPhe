using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteBanCaPhe.Data;
using WebsiteBanCaPhe.Models;

namespace WebsiteBanCaPhe.Controllers
{
    public class CartDetailsController : Controller
    {
        private readonly WebsiteBanCaPheContext _context;

        public CartDetailsController(WebsiteBanCaPheContext context)
        {
            _context = context;
        }

        // GET: CartDetails
        public async Task<IActionResult> Index()
        {
			var accountId = HttpContext.Session.GetString("AccountId");
			var cart = await _context.Cart.FirstOrDefaultAsync(c => c.AccountId.ToString() == accountId);
			var cartId = cart.CartId;
			var websiteBanCaPheContext = _context.CartDetail
                .Include(c => c.Cart)
                .Include(c => c.Product)
				.Where(c => c.CartId == cartId);
            return View(await websiteBanCaPheContext.ToListAsync());
        }

        // GET: CartDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CartDetail == null)
            {
                return NotFound();
            }

            var cartDetail = await _context.CartDetail
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartDetailId == id);
            if (cartDetail == null)
            {
                return NotFound();
            }

            return View(cartDetail);
        }

        // GET: CartDetails/Create
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName");
            return View();
        }

        //Add to cart
        public async Task<IActionResult> AddToCart(int? id)
        {
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId");
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName", product.ProductId);
            ViewData["ProductName"] = product.ProductName;
            return View();
        }


        // POST: CartDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartDetailId,CartId,ProductId,Quantity")] CartDetail cartDetail)
        {
			var accountId = HttpContext.Session.GetString("AccountId");
			var cart = await _context.Cart.FirstOrDefaultAsync(c => c.AccountId.ToString() == accountId);
			var cartId = cart.CartId;
			cartDetail.CartId = cartId;

			if (ModelState.IsValid)
            {
				var product = await _context.Product.FindAsync(cartDetail.ProductId);
				cartDetail.TotalPrice = cartDetail.Quantity * product.Price;
				_context.Add(cartDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", cartDetail.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName", cartDetail.ProductId);
            return View(cartDetail);
        }

        // GET: CartDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CartDetail == null)
            {
                return NotFound();
            }

            var cartDetail = await _context.CartDetail.FindAsync(id);
            if (cartDetail == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", cartDetail.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName", cartDetail.ProductId);
            return View(cartDetail);
        }

        // POST: CartDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartDetailId,CartId,ProductId,Quantity,TotalPrice")] CartDetail cartDetail)
        {
            if (id != cartDetail.CartDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartDetailExists(cartDetail.CartDetailId))
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
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", cartDetail.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName", cartDetail.ProductId);
            return View(cartDetail);
        }

        // GET: CartDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CartDetail == null)
            {
                return NotFound();
            }

            var cartDetail = await _context.CartDetail
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartDetailId == id);
            if (cartDetail == null)
            {
                return NotFound();
            }

            return View(cartDetail);
        }

        // POST: CartDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CartDetail == null)
            {
                return Problem("Entity set 'WebsiteBanCaPheContext.CartDetail'  is null.");
            }
            var cartDetail = await _context.CartDetail.FindAsync(id);
            if (cartDetail != null)
            {
                _context.CartDetail.Remove(cartDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartDetailExists(int id)
        {
          return (_context.CartDetail?.Any(e => e.CartDetailId == id)).GetValueOrDefault();
        }
    }
}
