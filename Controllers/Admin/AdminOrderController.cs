using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteBanCaPhe.Data;
using WebsiteBanCaPhe.Models;

namespace WebsiteBanCaPhe.Controllers.Admin
{
    public class AdminOrdersController : Controller
    {
        private readonly WebsiteBanCaPheContext _context;

        public AdminOrdersController(WebsiteBanCaPheContext context)
        {
            _context = context;
        }

        // GET: AdminOrders
        public async Task<IActionResult> Index()
        {
            var websiteBanCaPheContext = _context.UserOrder.Include(u => u.Account);
            return View(await websiteBanCaPheContext.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, bool IsPaid)
        {
            var userOrder = await _context.UserOrder.FirstOrDefaultAsync(u => u.OrderId == id);
            userOrder.IsPaid = IsPaid;
            await _context.SaveChangesAsync();
            var websiteBanCaPheContext = _context.UserOrder.Include(u => u.Account);
            return View(await websiteBanCaPheContext.ToListAsync());
        }


        // GET: AdminOrders/Details/5
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

        // GET: AdminOrders/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "FullName");
            return View();
        }

        // POST: AdminOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,ReceiverName,PhoneNumber,Address,PaymentMethod,Note,ShippingFee,TotalValue,IsDone,AccountId")] UserOrder userOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "FullName", userOrder.AccountId);
            return View(userOrder);
        }

        // GET: AdminOrders/Edit/5
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

        // POST: AdminOrders/Edit/5
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

            if (ModelState.IsValid)
            {
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
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "FullName", userOrder.AccountId);
            return View(userOrder);
        }

        // GET: AdminOrders/Delete/5
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

        // POST: AdminOrders/Delete/5
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
