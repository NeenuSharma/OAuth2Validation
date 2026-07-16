using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new[]
            {
            new
            {
                Id = 2,
                Name = "Laptop",
                Price = 50000
            }
        });
        }

        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            // Logic to add the product
            return CreatedAtAction(nameof(Get), new { id = 2, name = value, price = 60000 });
        }
    }
}
