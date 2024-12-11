using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BadogASP_razor_pages_.Models;

namespace BadogASP_razor_pages_.Pages
{
	public class IndexModel : PageModel
	{
		private readonly BadogContext _context;

		public IndexModel(BadogContext context)
		{
			_context = context;
		}

		public List<Product> Products { get; set; }

		public async Task OnGetAsync()
		{
			Products = await _context.Products.ToListAsync();
		}
	}
}
