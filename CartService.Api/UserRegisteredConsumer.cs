using CartService.Api.Repository;
using MassTransit;
using UserRegisteredEntity.cs;

namespace CartService.Api
{
    public class UserRegisteredConsumer : IConsumer<User>
    {
        private readonly ICartRepository _cartRepository;
        public UserRegisteredConsumer(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task Consume(ConsumeContext<User> context)
        {
            var msg = context.Message;
            await  _cartRepository.AssignCart(msg);
            //await Console.Out.WriteLineAsync(msg.Email);
            //_userCartRepository.AssignCart(msg.Email);
        }
    }
}
