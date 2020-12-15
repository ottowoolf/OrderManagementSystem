using System;
using System.Collections.Generic;
using System.Linq;


namespace OrderSystem.Domain
{
    public class OrderNew : OrderState
    {
        public OrderNew(OrderHeader orderHeader) : base(orderHeader) 
        {
        }

        public override OrderStates State => OrderStates.New; 

        public override void Complete(ref OrderState state)
        {
            throw new InvalidOperationException("Order must be submitted (pending) before it can be completed");
        }

        public override void Reject(ref OrderState state)
        {
            throw new InvalidOperationException("Order must be submitted (pending) before it can be rejected");
        }

        public override void Submit(ref OrderState state)
        {
           //business rule: an order must has at least one order item
           if(!_orderHeader.OrderItems.Any())
            {
                throw new InvalidOperationException("A new order must have at least one item before it can be submitted"); 
            }
            state = new OrderPending(_orderHeader);            
        }
    }
}
