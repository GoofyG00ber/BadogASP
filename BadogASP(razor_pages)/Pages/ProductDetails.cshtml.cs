using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BadogASP_razor_pages_.Models;
using Microsoft.AspNetCore.Mvc;

namespace BadogASP_razor_pages_.Pages
{
	public class ProductDetailsModel : PageModel
	{
		private readonly BadogContext _context;

		public ProductDetailsModel(BadogContext context)
		{
			_context = context;
		}

		public Product Product { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			Product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

			if (Product == null)
			{
				return NotFound(); // Return 404 if the product doesn't exist
			}

			return Page();
		}
	}
}
