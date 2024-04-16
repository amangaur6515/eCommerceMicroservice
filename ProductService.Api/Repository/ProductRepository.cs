using ProductService.Api.Models;
using MassTransit;
using ProductDetailEntity;
using Entity;

namespace ProductService.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private static readonly List<Product> _products = new List<Product>();
        private static int _count = 0;
        private readonly IPublishEndpoint _publishEndpoint;
        public ProductRepository(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> AddProduct(Product product)
        {
            product.Id = _count + 1;
            ProductDetailMsg msg = new ProductDetailMsg()
            {
                ProductId = product.Id,
                Price = product.Price,
                Size = product.Size,
                Design=product.Design,
                ImageUrl = product.ImageUrl,
            };
            
            _count = _count + 1;
            _products.Add(product);
            await _publishEndpoint.Publish<ProductDetailMsg>(msg);
            return true;
        }
        public Product GetProductById(int id)
        {
            foreach(var product in _products)
            {
                if (product.Id == id)
                {
                    return product;
                }
            }
            return null;
        }
        public async Task<bool> RemoveProduct(int id)
        {

            foreach (var item in _products)
            {
                if (item.Id == id)
                {
                    _products.Remove(item);
                    //notify productDetail service that product is removed
                    ProductRemoved obj = new ProductRemoved()
                    {
                        ProductId = id
                    };
                    
                    await _publishEndpoint.Publish<ProductRemoved>(obj);
                    
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> UpdateProduct(int id, Product product)
        {
            //remove existing product 
            foreach (var item in _products)
            {
                if (item.Id == id)
                {
                    item.Id = id;
                    item.Name = product.Name;
                    item.Description = product.Description;
                    item.Price = product.Price;
                    item.Size = product.Size;
                    item.Quantity = product.Quantity;
                    item.Design= product.Design;
                     
                    //remove item
                    _products.Remove(item);
                    //add this item
                    _products.Add(item);
                    //publish message to detail service
                    ProductUpdated obj = new ProductUpdated()
                    {
                        ProductId = product.Id,
                        Price = product.Price,
                        Size = product.Size,
                        Design = product.Design,
                        ImageUrl = product.ImageUrl,
                    };
                    await _publishEndpoint.Publish<ProductUpdated>(obj);
                    return true;
                }
            }
            return false;
        }

        public List<Product> GetProducts()
        {
            return _products;
        }

        public ProductStock GetStock(int id)
        {
            
            foreach(var item in _products)
            {
                if(item.Id == id)
                {
                    ProductStock stock = new ProductStock()
                    {
                        ProductId = item.Id,
                        Stock = item.Quantity,
                    };
                    return stock;

                }

            }
            return null;
            
        }
    }
}
