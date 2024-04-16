using CartService.Api.Models;
using CartService.Api.Repository;
using Steeltoe.Discovery;
using System.Text.Json;

namespace CartService.Api.Services
{
    public class CartsService : ICartsService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IDiscoveryClient _discoveryClient;
        public CartsService(ICartRepository cartRepository, IDiscoveryClient discoveryClient)
        {
            _cartRepository = cartRepository;
            _discoveryClient = discoveryClient;
        }
        public List<(int,int)> GetUserCart(string email)
        {
            var cart = _cartRepository.GetUserCart(email);

            return cart;
        }
        public async Task<bool> AddToCart(AddToCart addToCartObj, string jwtToken)
        {
            int quantity = addToCartObj.Quantity;

            int productId = addToCartObj.ProductId;
            int stock=0;
            bool productExist=true;
            //check if product is in stock
            if (productId != 0 && productId != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Discover the service using Steeltoe's IDiscoveryClient
                        string serviceUrl =  DiscoverServiceUrl("SERVICE.PRODUCTSERVICEAPI");
                        string apiUrl = $"{serviceUrl}/api/Customer/GetStock/{productId}";
                        //Console.WriteLine(apiUrl);
                        // Create HttpRequestMessage and add JWT token to the headers
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                        request.Headers.Add("Authorization", jwtToken);

                        HttpResponseMessage response = await client.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            // Parse and use the product details from the response
                            var responseData = await response.Content.ReadAsStringAsync();

                            ApiResponseStockCheck productDetails = JsonSerializer.Deserialize<ApiResponseStockCheck>(responseData);
                            stock = productDetails.stock;
                        }
                        else
                        {
                            productExist = false;
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }

            //if in stock pass to repository
            if(productExist && stock>0 && addToCartObj.Quantity!=0 && addToCartObj.Quantity<=stock)
            {
                //check the quantity of this product present in user cart
                int quantityPresent=CheckProductQuantity(addToCartObj);
                //if product already not present
                if (quantityPresent==-1)
                {
                    bool res = _cartRepository.AddToCart(addToCartObj);
                    return res;
                }
                if(quantityPresent!=-1 && quantityPresent+addToCartObj.Quantity<=stock)
                {
                    bool res = _cartRepository.AddToCart(addToCartObj);
                    //if items added
                    if (res)
                    {
                        return true;
                    }
                }
                
                //user does not exist
                return false;
            }
            return false;
        }

      
        private string DiscoverServiceUrl(string serviceName)
        {
            // Use Steeltoe's IDiscoveryClient to discover the location of the service
            var serviceInstances =  _discoveryClient.GetInstances(serviceName);
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

        private int CheckProductQuantity(AddToCart addToCartObj)
        {
            string email = addToCartObj.Email;
            var userCart = _cartRepository.GetUserCart(email);
            if (userCart!= null)
            {
                int productId = addToCartObj.ProductId;
                foreach(var item in userCart)
                {
                    int pId = item.Item1;
                    int quant=item.Item2;
                    if(pId==productId)
                    {
                        //return current quantity present in user cart
                        return quant;
                    }
                }
            }
            return -1;
        }
    }
}
