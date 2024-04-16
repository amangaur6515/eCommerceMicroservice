using ProductService.Api.Models;
using ProductService.Api.Repository;

namespace ProductService.Api.Services
{
    public class AdminService : IAdminService
    {
        private readonly IProductRepository _repository;
        public AdminService(IProductRepository productRepository)
        {
            _repository = productRepository;
        }

        public  async Task<bool> AddProduct(Product product)
        {
            bool res =await _repository.AddProduct(product);
            return res;
        }
        public async Task<bool> RemoveProduct(int id)
        {
            if (id == 0 || id == null)
            {
                return false;
            }
            bool res = await _repository.RemoveProduct(id);
            return res;
        }

        public async Task<bool> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || product == null || id == 0 || id == null)
            {
                return false;
            }
            bool res =await  _repository.UpdateProduct(id, product);
            return res;
        }

        public List<Product> GetAllProducts()
        {
            return _repository.GetProducts();
        }
        
    }
}
