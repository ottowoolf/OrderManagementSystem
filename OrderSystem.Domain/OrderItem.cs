using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Domain
{
    public class OrderItem
    {
        public OrderItem(OrderHeader order, int stockItemId, decimal price, string description, int quantity)
        {
            OrderHeaderId = order.Id;
            OrderHeader = order;
            StockItemId = stockItemId;
            Price = price;
            Description = description;
            Quantity = quantity; 
        }

        public int OrderHeaderId { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public int StockItemId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get => Price * Quantity;  }

    }
}
