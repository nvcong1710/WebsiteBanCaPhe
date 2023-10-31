using Microsoft.AspNetCore.Mvc;
using WebsiteBanCaPhe.Data;

namespace WebsiteBanCaPhe.Models
{
	public class NavBar: ViewComponent
	{
		private readonly WebsiteBanCaPheContext _context;

		public NavBar(WebsiteBanCaPheContext context)
		{
			_context = context;
		}

		public IViewComponentResult Invoke()
		{
			return View(_context.Category.ToList());
		}
	}
}
