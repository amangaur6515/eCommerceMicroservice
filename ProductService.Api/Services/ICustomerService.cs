using ProductDetailEntity;
using ProductService.Api.Models;

namespace ProductService.Api.Services
{
    public interface ICustomerService
    {
        List<CustomerProductViewModel> GetAllProducts();
        Task<ApiResponse> GetProductDetail(int id,string jwtToken);
        ProductStock GetStock(int id);
    }
}