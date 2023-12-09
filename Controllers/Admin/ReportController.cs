using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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
            long totalRevenue = 0;

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

        public async Task<IActionResult> SalesDownloadExcel(DateTime fromDate, DateTime toDate)
        {
            var listOrder = await _context.UserOrder.Include(u => u.Account).Where(u => u.OrderDate >= fromDate && u.OrderDate <= toDate).ToListAsync();

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Orders");
                worksheet.Cells["A1"].Value = "Ngày đặt";
                worksheet.Cells["B1"].Value = "Người nhận";
                worksheet.Cells["C1"].Value = "Số điện thoại";
                worksheet.Cells["D1"].Value = "Địa chỉ";
                worksheet.Cells["E1"].Value = "Ship";
                worksheet.Cells["F1"].Value = "Giá trị";
                worksheet.Cells["G1"].Value = "Trạng thái";
                worksheet.Cells["H1"].Value = "Thanh toán";

                int row = 2;
                foreach (var order in listOrder)
                {
                    worksheet.Cells[row, 1].Value = order.OrderDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 2].Value = order.ReceiverName;
                    worksheet.Cells[row, 3].Value = order.PhoneNumber;
                    worksheet.Cells[row, 4].Value = order.Address;
                    worksheet.Cells[row, 5].Value = order.ShippingFee;
                    worksheet.Cells[row, 6].Value = order.TotalValue;
                    worksheet.Cells[row, 7].Value = order.IsDone ? "Đã nhận" : "Chưa nhận";
                    worksheet.Cells[row, 8].Value = order.IsPaid ? "Đã thanh toán" : "Chưa thanh toán";
                    row++;
                }
                // Thêm công thức tổng vào cuối worksheet
                worksheet.Cells[worksheet.Dimension.Rows + 1, 6].Value = "Tổng doanh số:";
                worksheet.Cells[worksheet.Dimension.Rows, 7].Formula = "=SUM(F2:F" + (worksheet.Dimension.Rows - 1) + ")";
                xlPackage.Save();
            }

            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"DoanhThu_{fromDate.ToString("yyyyMMdd")}-{toDate.ToString("yyyyMMdd")}.xlsx");
        }

        public async Task<IActionResult> WarehouseDownloadExcel(DateTime fromDate, DateTime toDate)
        {
            var productData = await _context.OrderDetail
                .Where(od => od.UserOrder.OrderDate >= fromDate && od.UserOrder.OrderDate <= toDate)
                .GroupBy(od => od.ProductId)
                .Select(g => new Product
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.ProductName,
                    QuantitySold = g.Sum(od => od.Quantity),
                    Quantity = g.First().Product.Quantity
                }).ToListAsync();

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Orders");
                worksheet.Cells["A1"].Value = "Sản phẩm";
                worksheet.Cells["B1"].Value = "Đã bán";
                worksheet.Cells["C1"].Value = "Còn lại";

                int row = 2;
                foreach (var product in productData)
                {
                    worksheet.Cells[row, 1].Value = product.ProductName;
                    worksheet.Cells[row, 2].Value = product.QuantitySold;
                    worksheet.Cells[row, 3].Value = product.Quantity;
                    row++;
                }
                xlPackage.Save();
            }

            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BaoCaoKho_{fromDate.ToString("yyyyMMdd")}-{toDate.ToString("yyyyMMdd")}.xlsx");
        }

    }
}
