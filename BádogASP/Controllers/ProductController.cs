using Microsoft.AspNetCore.Mvc;
using BádogASP.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BádogASP.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // GET: api/product
        [HttpGet]
        public IActionResult GetProducts()
        {
            using var context = new BadogContext();
            return Ok(context.Products.ToList());
        }

        // GET api/product/5
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            try
            {
                using var context = new BadogContext();

                var product = context.Products.FirstOrDefault(x => x.ProductId == id);

                if (product == null)
                {
                    return NotFound("No product found with the specified ID.");
                }

                return Ok(product);
            }
            catch (TimeoutException ex)
            {
                return StatusCode(500, "The request timed out. Please try again later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        public class ProductDTO
        {
            public string Name { get; set; } = null!;
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public decimal? DiscountPrice { get; set; }
            public int? CategoryId { get; set; }
            public int StockQuantity { get; set; }
            public string? ImageUrl { get; set; }
        }


        [HttpPost]
        public IActionResult AddProduct([FromBody] ProductDTO productDto)
        {
            using var context = new BadogContext();

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                DiscountPrice = productDto.DiscountPrice,
                CategoryId = productDto.CategoryId,
                StockQuantity = productDto.StockQuantity,
                ImageUrl = productDto.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Products.Add(product);
            context.SaveChanges();

            return Ok("Product added successfully.");
        }



        // PUT api/product/5
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDTO updatedProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var context = new BadogContext();

            try
            {
                var product = new Product
                {
                    ProductId = id,
                    Name = updatedProductDto.Name,
                    Description = updatedProductDto.Description,
                    Price = updatedProductDto.Price,
                    DiscountPrice = updatedProductDto.DiscountPrice,
                    CategoryId = updatedProductDto.CategoryId,
                    StockQuantity = updatedProductDto.StockQuantity,
                    ImageUrl = updatedProductDto.ImageUrl,
                    UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified) // Convert to Unspecified
                };

                context.Products.Attach(product);
                context.Entry(product).State = EntityState.Modified;

                context.SaveChanges();

                return Ok("Product updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }




        // DELETE api/product/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            using var context = new BadogContext();

            var product = context.Products
                .Include(p => p.OrderItems)
                .Include(p => p.Reviews)
                .Include(p => p.Wishlists)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound("No product found with the specified ID.");
            }

            // Remove related entities
            context.OrderItems.RemoveRange(product.OrderItems);
            context.Reviews.RemoveRange(product.Reviews);
            context.Wishlists.RemoveRange(product.Wishlists);

            // Remove the product
            context.Products.Remove(product);

            // Save changes
            context.SaveChanges();

            return Ok("Product deleted successfully.");
        }

    }
}
