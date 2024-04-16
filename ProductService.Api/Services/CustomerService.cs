using MassTransit;
using ProductDetailEntity;
using ProductService.Api.Models;
using ProductService.Api.Repository;
using Steeltoe.Discovery;
using System.Text.Json;

namespace ProductService.Api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IProductRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IDiscoveryClient _discoveryClient;
        public CustomerService(IProductRepository productRepository,IDiscoveryClient discoveryClient)
        {
            _repository = productRepository;
            _discoveryClient = discoveryClient;
        }

        public List<CustomerProductViewModel> GetAllProducts()
        {
            List<CustomerProductViewModel> customerProducts = new List<CustomerProductViewModel>();
            List<Product> products = _repository.GetProducts();
            foreach (Product product in products)
            {
                customerProducts.Add(new CustomerProductViewModel()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl=product.ImageUrl,
                });
            }
            return customerProducts;
        }

        public async Task<ApiResponse> GetProductDetail(int id,string jwtToken)
        {
            if(id != 0 && id != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        //via apigateway
                        string serviceUrl =  DiscoverServiceUrl("SERVICE.PRODUCTDETAILAPI");
                        if (serviceUrl == null)
                        {
                            // Handle service not found
                            return null ;
                        }
                       
                        string apiUrl = $"{serviceUrl}/api/ProductDetail/GetProductDetail/{id}";
                        // Create HttpRequestMessage and add JWT token to the headers
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                        request.Headers.Add("Authorization", jwtToken);

                        HttpResponseMessage response = await client.SendAsync(request);

                        
                        if (response.IsSuccessStatusCode)
                        {
                            // Parse and use the product details from the response
                            var responseData = await response.Content.ReadAsStringAsync();
                            
                            ApiResponse productDetails = JsonSerializer.Deserialize<ApiResponse>(responseData);
                            var product=_repository.GetProductById(productDetails.productId);
                            productDetails.productName = product.Name;
                            productDetails.description = product.Description;
                            productDetails.ImageUrl = product.ImageUrl;
                            
                            return productDetails;
                        }
                        else
                        {
                            // Handle error
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            return null;

        }

        public ProductStock GetStock(int id)
        {
            if (id != 0 && id != null)
            {
                return _repository.GetStock(id);
            }
            return null;

        }

        private string DiscoverServiceUrl(string serviceName)
        {
            // Use Steeltoe's IDiscoveryClient to discover the location of the service
            var serviceInstances = _discoveryClient.GetInstances(serviceName);
            if (serviceInstances != null && serviceInstances.Count > 0)
            {
                // For simplicity, return the URL of the first instance
                var serviceInstance = serviceInstances[0];
                return $"https://{serviceInstance.Host}:{serviceInstance.Port}";
            }
            else
            {
                return null; // Service not found
            }
        }
    }
}
