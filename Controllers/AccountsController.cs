using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebsiteBanCaPhe.Data;
using WebsiteBanCaPhe.Models;

namespace WebsiteBanCaPhe.Controllers
{
    public class AccountsController : Controller
    {
        private readonly WebsiteBanCaPheContext _context;

        public AccountsController(WebsiteBanCaPheContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
              return _context.Account != null ? 
                          View(await _context.Account.ToListAsync()) :
                          Problem("Entity set 'WebsiteBanCaPheContext.Account'  is null.");
        }

        // GET: Accounts/Details
        public async Task<IActionResult> Details()
        {
			var accountId = HttpContext.Session.GetString("AccountId");
			var account = await _context.Account.FirstOrDefaultAsync(c => c.AccountId.ToString() == accountId);
            return View(account);
        }
		//===========================================================================
		// GET: Accounts/Create
		public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,PhoneNumber,Password,FullName,Gender")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                var cart = new Cart
                {
                    AccountId = account.AccountId,
                    TotalValue = 0
                };
                _context.Add(cart);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Accounts");
            }
			return RedirectToAction("Login", "Accounts");
		}

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit()
        {
            var id = HttpContext.Session.GetString("AccountId");
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FirstOrDefaultAsync(c => c.AccountId.ToString() == id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("AccountId,PhoneNumber,Password,FullName,Gender")] Account account)
        {
            var id = HttpContext.Session.GetString("AccountId");
            if (id != account.AccountId.ToString())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
				return RedirectToAction("Index", "Home");
			}
			return RedirectToAction("Index", "Home");
		}

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Account == null)
            {
                return Problem("Entity set 'WebsiteBanCaPheContext.Account'  is null.");
            }
            var account = await _context.Account.FindAsync(id);
            if (account != null)
            {
                _context.Account.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details));
        }

        private bool AccountExists(int id)
        {
          return (_context.Account?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }

        //==========================LOGIN========================================
        public IActionResult Login()
        {
            return View();
        }

        // POST: Accounts/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string phone, string password)
        {
            if (phone == null || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Phone number and password are required.");
                return View();
            }

            var account = await _context.Account.FirstOrDefaultAsync(a => a.PhoneNumber == phone && a.Password == password);
            if (account == null)
            {
                ModelState.AddModelError("", "Invalid phone number or password.");
                return View();
            }

            // Lưu thông tin tài khoản vào session
            HttpContext.Session.SetString("AccountId", account.AccountId.ToString());
			return RedirectToAction("Index", "Home");
        }

	}
}
