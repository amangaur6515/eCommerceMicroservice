using ProductService.Api.Models;

namespace ProductService.Api.Repository
{
    public interface IProductRepository
    {
         Task<bool> AddProduct(Product product);
        Product GetProductById(int id); 
        List<Product> GetProducts();
        Task<bool> RemoveProduct(int id);
        Task<bool> UpdateProduct(int id,Product product);
        ProductStock GetStock(int id);
    }
}