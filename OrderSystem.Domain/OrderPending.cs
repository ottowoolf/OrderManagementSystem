using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Domain
{
    public class OrderPending : OrderState
    {

        public OrderPending(OrderHeader orderHeader) : base(orderHeader) { }


        public override OrderStates State => OrderStates.Pending;

        public override void Complete(ref OrderState state)
        {
            state = new OrderComplete(_orderHeader); 
        }

        public override void Reject(ref OrderState state)
        {
            state = new OrderRejected(_orderHeader);
        }

        public override void Submit(ref OrderState state)
        {
           //NO-OP
        }
    }
}
