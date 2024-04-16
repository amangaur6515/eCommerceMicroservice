using Entity;
using ProductDetailEntity;
using ProductDetailService.Api.Models;

namespace ProductDetailService.Api.Repository
{
    public interface IProductDetailRepository
    {
        Task<bool> AddProductDetail(ProductDetailMsg productDetail);
        ProductDetailMsg GetProductDetail(int id);
        List<ProductDetailMsg> GetProducts();
        Task<bool> RemoveProductDetail(ProductRemoved obj);
        Task<bool> UpdateProductDetail(ProductUpdated productUpdated);
    }
}