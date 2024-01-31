using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Mvc.Core;
using WebsiteBanCaPhe.Data;
using WebsiteBanCaPhe.Models;
using X.PagedList;

namespace WebsiteBanCaPhe.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebsiteBanCaPheContext _context;

        public ProductsController(WebsiteBanCaPheContext context)
        {
            _context = context;
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id, Account account)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            var feedbackList = await _context.Feedback
                .Include(f=>f.Product)
                .Include(f=>f.Account)
                .Where(f=>f.ProductId == id).ToListAsync();
            ViewData["FeedbackList"] = feedbackList;
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Index(int? page)
        {
            if(page ==  null)
            {
                page = 1;
            }
            var productByCategory = _context.Product.ToList();
            return View(productByCategory.ToPagedList((int)page, 9));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        //===================================
          [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFeedback([Bind("FeedbackId,Content,Star,FeedbackDate,AccountId,ProductId")] Feedback feedback)
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            if (accountId != null)
            {
                feedback.AccountId = int.Parse(accountId);
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "FullName", feedback.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Branch", feedback.ProductId);
            _context.Add(feedback);
            await _context.SaveChangesAsync();

            var feedbackList = await _context.Feedback
                .Include(f => f.Product)
                .Include(f => f.Account)
                .Where(f => f.ProductId == feedback.ProductId).ToListAsync();

            var product = await _context.Product.FirstOrDefaultAsync(p => p.ProductId == feedback.ProductId);

            int k = 0;
            foreach(var item in feedbackList)
            {
                k = (k + item.Star);
            }
            k = k / feedbackList.Count;

            product.Star = k;
            _context.SaveChangesAsync();

            return RedirectToAction("Details", "Products", new { id = feedback.ProductId });
        }
    }
}
