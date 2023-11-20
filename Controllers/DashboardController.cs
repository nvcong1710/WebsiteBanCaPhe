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
    public class DashboardController : Controller
    {
        private readonly WebsiteBanCaPheContext _context;

        public DashboardController(WebsiteBanCaPheContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var websiteBanCaPheContext = _context.OrderDetail.Include(o => o.Product).Include(o => o.UserOrder);
            var orderDetailList = _context.OrderDetail.Include(o=>o.Product).Include(o => o.UserOrder);
            long RevenueInstant = 0;
            long RevenueBean = 0;
            long RevenueRoaster = 0;
            long [] RevenueByMonth = new long[12];
            long[] RevenueByMonthInstant = new long[12];
            long[] RevenueByMonthRoaster = new long[12];
            long[] RevenueByMonthBean = new long[12];

            foreach (var item in orderDetailList)
            {
                if (item.UserOrder.OrderDate.Year == DateTime.Now.Year)
                {
                    if (item.Product.CategoryId == 1)
                    {
                        RevenueInstant += item.TotalPrice;
                        RevenueByMonthInstant[item.UserOrder.OrderDate.Month - 1] += item.TotalPrice;
                    }
                    else if (item.Product.CategoryId == 2)
                    {
                        RevenueRoaster += item.TotalPrice;
                        RevenueByMonthRoaster[item.UserOrder.OrderDate.Month - 1] += item.TotalPrice;
                    }
                    else
                    {
                        RevenueBean += item.TotalPrice;
                        RevenueByMonthBean[item.UserOrder.OrderDate.Month - 1] += item.TotalPrice;
                    }

                    RevenueByMonth[item.UserOrder.OrderDate.Month - 1] += item.TotalPrice;
                }

            }

            double InstantRate = (double)RevenueInstant / (RevenueInstant + RevenueBean + RevenueRoaster) * 100.0;
            double RoasterRate = (double)RevenueRoaster / (RevenueInstant + RevenueBean + RevenueRoaster) * 100.0;
            double BeanRate = (double)RevenueBean / (RevenueInstant + RevenueBean + RevenueRoaster) * 100.0;

            ViewData["InstantRate"] = InstantRate;
            ViewData["RoasterRate"] = RoasterRate;
            ViewData["BeanRate"] = BeanRate;

            ViewData["RevenueInstant"] = RevenueInstant;
            ViewData["RevenueRoaster"] = RevenueRoaster;
            ViewData["RevenueBean"] = RevenueBean;
            ViewData["RevenueByMonth"] = RevenueByMonth;
            ViewData["RevenueByMonthInstant"] = RevenueByMonthInstant;
            ViewData["RevenueByMonthRoaster"] = RevenueByMonthRoaster;
            ViewData["RevenueByMonthBean"] = RevenueByMonthBean;

            return View(await websiteBanCaPheContext.ToListAsync());
        }
    }
}
