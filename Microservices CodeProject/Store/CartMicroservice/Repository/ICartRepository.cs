using CartMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartMicroservice.Repository
{
    public interface ICartRepository
    {
        List<CartItem> GetCartItems(Guid userId);
        void InsertCartItem(Guid userId, CartItem cartItem);
        void UpdateCartItem(Guid userId, CartItem cartItem);
        void DeleteCartItem(Guid userId, Guid cartItemId);
    }
}
