using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartMicroservice.Model
{
    public class CartItem
    {
        public Guid CatalogItemId { get; set; }
        public string CatalogItemName { get; set; }
        public decimal CatalogItemPrice { get; set; }
        public int Quantity { get; set; }
    }
}
