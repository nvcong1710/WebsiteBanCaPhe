using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
                .Where(u => u.AccountId.ToString() == accountId)
                .OrderByDescending(u => u.OrderDate);
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
                .Include(o => o.Product)
                .Include(o => o.UserOrder)
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
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,ReceiverName,PhoneNumber,Address,PaymentMethod,Note,ShippingFee,TotalValue,IsDone,AccountId,EmailAddress")] UserOrder userOrder)
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            var cart = await _context.Cart.FirstOrDefaultAsync(c => c.AccountId.ToString() == accountId);
            var cartId = cart.CartId;
            string htmlBody = $@"<html><head><style>body {{ font-family: 'Arial', sans-serif; }} table{{ width: 100%; border-collapse: collapse; margin-top: 15px; }}th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }} th {{ background-color: #f2f2f2; }} h2{{text-align: center; }}</style></head><body><h2>Thông tin đơn hàng</h2><p>Xin chào {userOrder.ReceiverName},</p><p>Dưới đây là thông tin chi tiết về đơn hàng của bạn tại LuaHanThu:</p><table><tr><th>Sản phẩm</th><th>Số lượng</th><th>Đơn giá</th><th>Thành tiền</th></tr>";

            var listCartDetail = _context.CartDetail
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .Where(c => c.CartId == cartId);
            foreach (var cartDetail in listCartDetail)
            {
                var product = await _context.Product.FindAsync(cartDetail.ProductId);
                htmlBody += $@"<tr><td>{product.ProductName}</td><td>{cartDetail.Quantity}</td><td>{product.Price}</td><td>{cartDetail.TotalPrice}</td></tr>";

                // Check if product quantity is less than order quantity
                if (product.Quantity < cartDetail.Quantity)
                {
                    // Return error message to view
                    String error = "Số lượng sản phẩm " + product.ProductName.ToString() + " không đủ để hoàn thành đơn hàng.";
                    ViewBag.ErrorMessage = error;
                    return View(userOrder);
                }


            }
            htmlBody += $@"</table><p>Tổng giá trị đơn hàng: {userOrder.TotalValue}</p><p>Phí vận chuyển: {userOrder.ShippingFee}</p><p>Tổng cộng: {userOrder.TotalValue + userOrder.ShippingFee}</p><p>Cảm ơn bạn đã mua sắm tại LuaHanThu!</p></body></html>";
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
            //==============================SEND EMAIL========================
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("cong171002@gmail.com", "ynpb hqik ilvc fybl");
                smtpClient.EnableSsl = true;

                // Create a MailMessage object
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("cong171002@gmail.com", "Lửa Hận Thù");
                mailMessage.To.Add(userOrder.EmailAddress);
                mailMessage.Subject = "Đơn hàng của bạn tại LuaHanThu";

                // Set the email body as HTML
                mailMessage.Body = htmlBody;
                mailMessage.IsBodyHtml = true;

                // Send the email
                smtpClient.Send(mailMessage);
            }
            //================================================================
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
