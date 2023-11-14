using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebsiteBanCaPhe.Data;
using WebsiteBanCaPhe.Models;

namespace WebsiteBanCaPhe.Controllers.Admin
{
    public class ReportController : Controller
    {
        private readonly WebsiteBanCaPheContext _context;

        public ReportController(WebsiteBanCaPheContext context)
        {
            _context = context;
        }

        public IActionResult HomeReport()
        {
            return View();
        }

        [HttpPost]
        public Task<IActionResult> HomeReport(int reportType, DateTime fromDate, DateTime toDate)
        {
            ViewData["reportType"] = reportType;
            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;
            if (reportType == 0)
            {
                return SalesReport(fromDate, toDate);
            }
            else return WarehouseReport(fromDate, toDate);
        }

        [HttpPost]
        public async Task<IActionResult> SalesReport(DateTime fromDate, DateTime toDate)
        {
            ViewData["reportType"] = 0;
            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;
            var listOrder = await _context.UserOrder.Include(u => u.Account).Where(u => u.OrderDate >= fromDate && u.OrderDate <= toDate).ToListAsync();
            decimal totalRevenue = 0;

            foreach (var order in listOrder)
            {
                totalRevenue += order.TotalValue;
            }

            ViewData["TotalRevenue"] = totalRevenue;
            return View("SalesReport", listOrder);
        }

        [HttpPost]
        public async Task<IActionResult> WarehouseReport(DateTime fromDate, DateTime toDate)
        {
            ViewData["reportType"] = 1;
            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;
            var productData = await _context.OrderDetail
                .Where(od => od.UserOrder.OrderDate >= fromDate && od.UserOrder.OrderDate <= toDate)
                .GroupBy(od => od.ProductId)
                .Select(g => new Product
                {
                    ProductId = g.Key,
                    QuantitySold = g.Sum(od => od.Quantity)
                }).ToListAsync();

            foreach (var product in productData)
            {
                var existingProduct = await _context.Product.FindAsync(product.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.QuantitySold = product.QuantitySold;
                }
            }

            await _context.SaveChangesAsync();
            return View("WarehouseReport", await _context.Product.ToListAsync());
        }
    }
}
