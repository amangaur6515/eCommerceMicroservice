using MassTransit;
using ProductDetailEntity;
using ProductDetailService.Api.Repository;

namespace ProductDetailService.Api
{
    public class ProductDetailMsgConsumer : IConsumer<ProductDetailMsg>
    {
        private readonly IProductDetailRepository _productDetailRepository;
        public ProductDetailMsgConsumer(IProductDetailRepository productDetailRepository)
        {
            _productDetailRepository = productDetailRepository;
        }
        public async Task Consume(ConsumeContext<ProductDetailMsg> context)
        {
            var msg = context.Message;
            await _productDetailRepository.AddProductDetail(msg);
            //await Console.Out.WriteLineAsync(msg.Email);
            //_userCartRepository.AssignCart(msg.Email);
        }
       
    }
}
