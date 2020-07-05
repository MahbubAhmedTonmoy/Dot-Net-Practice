using CartMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartMicroservice.Repository
{
    public class CartRepository : ICartRepository
    {
        public void DeleteCartItem(Guid userId, Guid cartItemId)
        {
            throw new NotImplementedException();
        }

        public List<CartItem> GetCartItems(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void InsertCartItem(Guid userId, CartItem cartItem)
        {
            throw new NotImplementedException();
        }

        public void UpdateCartItem(Guid userId, CartItem cartItem)
        {
            throw new NotImplementedException();
        }
    }
}
