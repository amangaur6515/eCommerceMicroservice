using Entity;
using MassTransit;
using ProductDetailEntity;
using ProductDetailService.Api.Repository;

namespace ProductDetailService.Api
{
    public class ProductRemovedConsumer : IConsumer<ProductRemoved>
    {
        private readonly IProductDetailRepository _productDetailRepository;
        public ProductRemovedConsumer(IProductDetailRepository productDetailRepository)
        {
            _productDetailRepository = productDetailRepository;
        }
        public async Task Consume(ConsumeContext<ProductRemoved> context)
        {
            var msg = context.Message;
            await _productDetailRepository.RemoveProductDetail(msg);
             Console.WriteLine(msg);
            //_userCartRepository.AssignCart(msg.Email);
        }
    }
}
