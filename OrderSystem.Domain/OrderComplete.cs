using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Domain
{
    public class OrderComplete : OrderState
    {
        public OrderComplete(OrderHeader orderHeader) : base(orderHeader) { }

        public override OrderStates State => OrderStates.Complete;
        public override void Complete(ref OrderState state)
        {
            throw new InvalidOperationException("This order is already completed");
        }

        public override void Reject(ref OrderState state)
        {
            throw new InvalidOperationException("A completed can't be rejected");
        }

        public override void Submit(ref OrderState state)
        {
            throw new InvalidOperationException("A completed can't be submitted");
        }
    }
}
