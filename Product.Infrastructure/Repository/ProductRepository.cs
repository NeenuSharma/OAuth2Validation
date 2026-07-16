using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Infrastructure.Contexts;

namespace Product.Infrastructure.Repository
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductData>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
