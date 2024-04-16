using ProductService.Api.Models;

namespace ProductService.Api.Services
{
    public interface IAdminService
    {
        Task<bool> AddProduct(Product product);
        List<Product> GetAllProducts();
        Task<bool> RemoveProduct(int id);
        Task<bool> UpdateProduct(int id, Product product);



    }
}