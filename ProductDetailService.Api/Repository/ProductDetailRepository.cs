using Entity;
using ProductDetailEntity;
using ProductDetailService.Api.Models;

namespace ProductDetailService.Api.Repository
{
    public class ProductDetailRepository : IProductDetailRepository
    {
        private static  List<ProductDetailMsg> _productDetails = new List<ProductDetailMsg>();
        public ProductDetailRepository()
        {

        }

        public async Task<bool> AddProductDetail(ProductDetailMsg productDetail)
        {
            _productDetails.Add(productDetail);
            return true;
        }

        public ProductDetailMsg GetProductDetail(int id)
        {
            ProductDetailMsg p = null;
            foreach (var item in _productDetails)
            {
                if (id == item.ProductId)
                {
                    p = item;
                    break;
                }
            }
            return p;
        }

        public List<ProductDetailMsg> GetProducts()
        {
            return _productDetails;
        }

        public async Task<bool> RemoveProductDetail(ProductRemoved obj)
        {
            int id=obj.ProductId;
            
            foreach(var item in _productDetails)
            {
                if (id == item.ProductId)
                {
                    _productDetails.Remove(item);
                    break;
                }
            }
            return true;
        }

        public async Task<bool> UpdateProductDetail(ProductUpdated productUpdated)
        {
            
            var data = _productDetails.Where(obj => obj.ProductId == productUpdated.ProductId).FirstOrDefault();
            
            _productDetails.Remove(data);
            ProductDetailMsg productDetail = new ProductDetailMsg()
            {
                ProductId = productUpdated.ProductId ,
                Price = productUpdated.Price ,
                Size = productUpdated.Size ,
                Design=productUpdated.Design ,
                ImageUrl = productUpdated.ImageUrl ,   
            };
            _productDetails.Add(productDetail);
           
            return true;
        }


    }
}
