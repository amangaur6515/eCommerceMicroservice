using Entity;
using MassTransit;
using ProductDetailEntity;
using ProductDetailService.Api.Repository;

namespace ProductDetailService.Api
{
    public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
    {
        private readonly IProductDetailRepository _productDetailRepository;
        public ProductUpdatedConsumer(IProductDetailRepository productDetailRepository)
        {
            _productDetailRepository = productDetailRepository;
        }
        public async Task Consume(ConsumeContext<ProductUpdated> context)
        {
            var msg = context.Message;
            await _productDetailRepository.UpdateProductDetail(msg);
            //Console.WriteLine(msg);
            //_userCartRepository.AssignCart(msg.Email);
        }
    }
}
