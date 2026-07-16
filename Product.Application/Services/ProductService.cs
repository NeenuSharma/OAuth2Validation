using Product.Domain.Entities;
using Product.Infrastructure.Repository;

namespace Product.Application.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository;

        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductData>> GetProductsAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
