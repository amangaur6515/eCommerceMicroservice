using CartService.Api.Models;
using UserRegisteredEntity.cs;

namespace CartService.Api.Repository
{
    public class CartRepository:ICartRepository
    {
        private static Dictionary<string, List<(int ProductId, int Quantity)>> _userCarts = new Dictionary<string, List<(int ProductId, int Quantity)>>();
        private static bool alreadyAdded = false;
        public CartRepository()
        {
            if (!alreadyAdded) 
            AssignCart();
        }
        private  void AssignCart()
        {
            _userCarts.Add("amangaur6515@gmail.com",new List<(int,int)>());
            _userCarts.Add("test@gmail.com", new List<(int,int)>());
            alreadyAdded = true;
        }
        public async Task<bool> AssignCart(User obj)
        {
            _userCarts.Add(obj.Email,new List<(int,int)>());
            return true;
        }

        public List<(int,int)> GetUserCart(string email)
        {
            if (_userCarts.ContainsKey(email))
            {
                //return  cart list
                return _userCarts[email];
            }
            return null;
        }

        public bool AddToCart(AddToCart addToCartObj)
        {
            string user = addToCartObj.Email;
            int productId = addToCartObj.ProductId;
            int quantity = addToCartObj.Quantity;
            if (_userCarts.ContainsKey(user))
            {
                //check if item already present
                bool isItemPresent=IsItemPresent(addToCartObj);
                if(isItemPresent)
                {
                    //update cart
                    UpdateCart(addToCartObj);
                    return true;
                }
                //else add items to its cart
                _userCarts[user].Add((ProductId:productId, Quantity:quantity));
                return true;
            }
            return false;
        }

        private bool IsItemPresent(AddToCart addToCartObj)
        {
            int productId = addToCartObj.ProductId;
            string user = addToCartObj.Email;

            if (_userCarts.ContainsKey(user))
            {
                var cart = _userCarts[user];
                foreach(var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool UpdateCart(AddToCart addToCartObj)
        {
            int productId = addToCartObj.ProductId;
            string user = addToCartObj.Email;
            int quantity=addToCartObj.Quantity;

            if (_userCarts.ContainsKey(user))
            {
                var cart = _userCarts[user];
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        int currentQuantity = item.Quantity;
                        cart.Remove(item);
                        _userCarts[user].Add((ProductId: productId, Quantity: quantity+currentQuantity));
                        return true;
                    }
                }
            }
            return false;
        }


    }
}
