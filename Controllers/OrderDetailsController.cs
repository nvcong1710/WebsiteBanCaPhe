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
    public class OrderDetailsController : Controller
    {
        private readonly WebsiteBanCaPheContext _context;

        public OrderDetailsController(WebsiteBanCaPheContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            var websiteBanCaPheContext = _context.OrderDetail.Include(o => o.Product);
            return View(await websiteBanCaPheContext.ToListAsync());
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Branch");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDetailId,ProductId,Quantity,TotalPrice")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Branch", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Branch", orderDetail.ProductId);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderDetailId,ProductId,Quantity,TotalPrice")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderDetailId))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Branch", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderDetail == null)
            {
                return Problem("Entity set 'WebsiteBanCaPheContext.OrderDetail'  is null.");
            }
            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetail.Remove(orderDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
          return (_context.OrderDetail?.Any(e => e.OrderDetailId == id)).GetValueOrDefault();
        }
    }
}
