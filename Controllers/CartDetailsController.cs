﻿using System;
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
            var websiteBanCaPheContext = _context.CartDetail.Include(c => c.Product);
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Branch");
            return View();
        }

        // POST: CartDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartDetailId,ProductId,Quantity,TotalPrice")] CartDetail cartDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Branch", cartDetail.ProductId);
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Branch", cartDetail.ProductId);
            return View(cartDetail);
        }

        // POST: CartDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartDetailId,ProductId,Quantity,TotalPrice")] CartDetail cartDetail)
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Branch", cartDetail.ProductId);
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
