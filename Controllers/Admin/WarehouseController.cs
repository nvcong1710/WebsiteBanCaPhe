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
    public class WarehouseController : Controller
    {
        private readonly WebsiteBanCaPheContext _context;

        public WarehouseController(WebsiteBanCaPheContext context)
        {
            _context = context;
        }

        // GET: Warehouse
        public async Task<IActionResult> Index()
        {
            var websiteBanCaPheContext = _context.Product.Include(p => p.Category);
            return View(await websiteBanCaPheContext.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, int newQuantity)
        {
            var product = await _context.Product.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);

            if (product != null)
            {
                product.Quantity = newQuantity;
                await _context.SaveChangesAsync();
            }
            var websiteBanCaPheContext = _context.Product.Include(p => p.Category);
            return View(await websiteBanCaPheContext.ToListAsync());
        }
    }
}
