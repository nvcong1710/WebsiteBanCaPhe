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

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details()
        {
			var accountID = HttpContext.Session.GetString("AccountID");
			var account = GetAccountById(accountID);
			return View(account);
        }
		//==============================================================================
		public Account GetAccountById(string accountId)
		{
			Account account = null;
			using (var connection = new SqlConnection("your connection string"))
			{
				connection.Open();

				using (var command = new SqlCommand("SELECT * FROM Accounts WHERE AccountId = @AccountId", connection))
				{

					command.Parameters.AddWithValue("@AccountId", accountId);

					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							account = new Account();
							account.AccountId = (int)reader["AccountId"];
							account.PhoneNumber = reader["PhoneNumber"].ToString();
							account.Password = reader["Password"].ToString();
							account.FullName = reader["FullName"].ToString();
							account.Gender = reader["Gender"].ToString();
						}
					}
				}
			}
			return account;
		}
		//=============================================================================
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
                return RedirectToAction(nameof(Index));
            }
			return RedirectToAction("Index", "Home");
		}

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Account == null)
            {
                int n;
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,PhoneNumber,Password,FullName,Gender")] Account account)
        {
            if (id != account.AccountId)
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
