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
    public class UserOrdersController : Controller
    {
        private readonly WebsiteBanCaPheContext _context;

        public UserOrdersController(WebsiteBanCaPheContext context)
        {
            _context = context;
        }

        // GET: UserOrders
        public async Task<IActionResult> Index()
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            var websiteBanCaPheContext = _context.UserOrder.Include(u => u.Account)
                .Where(u => u.AccountId.ToString() == accountId);
            return View(await websiteBanCaPheContext.ToListAsync());
        }

        // GET: UserOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserOrder == null)
            {
                return NotFound();
            }

            var accountId = HttpContext.Session.GetString("AccountId");
            var cart = await _context.Cart.FirstOrDefaultAsync(c => c.AccountId.ToString() == accountId);

            var userOrder = await _context.UserOrder
                .Include(u => u.Account)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            var orderDetailList = _context.OrderDetail
                .Include(o=>o.Product)
                .Include(o=>o.UserOrder)
                .Where(o => o.OrderId == id);
            if (userOrder == null)
            {
                return NotFound();
            }

            ViewData["OrderDetailList"] = await orderDetailList.ToListAsync();
            return View(userOrder);
        }

        // GET: UserOrders/Create
        public async Task<IActionResult> Create()
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            var cart = await _context.Cart.FirstOrDefaultAsync(c => c.AccountId.ToString() == accountId);
            var cartId = cart.CartId;

            var listCartDetail = _context.CartDetail
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .Where(c => c.CartId == cartId);

            var cartTotalValue = listCartDetail.Sum(c => c.TotalPrice);

            ViewData["AccountId"] = accountId;
            ViewData["OrderDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["ShippingFee"] = 0;
            ViewData["ListCartDetail"] = await listCartDetail.ToListAsync();
            ViewData["TotalValue"] = ViewData["CartTotalValue"] = cartTotalValue;
            return View();
        }

        // POST: UserOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,ReceiverName,PhoneNumber,Address,PaymentMethod,Note,ShippingFee,TotalValue,IsDone,AccountId")] UserOrder userOrder)
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            var cart = await _context.Cart.FirstOrDefaultAsync(c => c.AccountId.ToString() == accountId);
            var cartId = cart.CartId;

            var listCartDetail = _context.CartDetail
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .Where(c => c.CartId == cartId);
            foreach (var cartDetail in listCartDetail)
            {
                var product = await _context.Product.FindAsync(cartDetail.ProductId);

                // Check if product quantity is less than order quantity
                if (product.Quantity < cartDetail.Quantity)
                {
                    // Return error message to view
                    ViewBag.ErrorMessage = "Số lượng sản phẩm không đủ để hoàn thành đơn hàng.";
                    return View(userOrder);
                }

                
            }
            _context.Add(userOrder);
            await _context.SaveChangesAsync();

            foreach (var cartDetail in listCartDetail)
            {
                var product = await _context.Product.FindAsync(cartDetail.ProductId);

                // Thêm sản phẩm vào OrderDetail
                var orderDetail = new OrderDetail
                {
                    OrderId = userOrder.OrderId,
                    ProductId = cartDetail.ProductId,
                    Quantity = cartDetail.Quantity,
                    TotalPrice = cartDetail.TotalPrice
                };
                //Cập nhật số lượng sản phẩm trong kho
                product.Quantity -= orderDetail.Quantity;
                _context.OrderDetail.Add(orderDetail);
            }
            //Xoá sản phẩm trong giỏ hàng
            _context.CartDetail.RemoveRange(listCartDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: UserOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserOrder == null)
            {
                return NotFound();
            }

            var userOrder = await _context.UserOrder.FindAsync(id);
            if (userOrder == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "FullName", userOrder.AccountId);
            return View(userOrder);
        }

        // POST: UserOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,ReceiverName,PhoneNumber,Address,PaymentMethod,Note,ShippingFee,TotalValue,IsDone,AccountId")] UserOrder userOrder)
        {
            if (id != userOrder.OrderId)
            {
                return NotFound();
            }
            try
            {
                _context.Update(userOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserOrderExists(userOrder.OrderId))
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

        // GET: UserOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: UserOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserOrder == null)
            {
                return Problem("Entity set 'WebsiteBanCaPheContext.UserOrder'  is null.");
            }
            var userOrder = await _context.UserOrder.FindAsync(id);
            if (userOrder != null)
            {
                _context.UserOrder.Remove(userOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserOrderExists(int id)
        {
            return (_context.UserOrder?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
