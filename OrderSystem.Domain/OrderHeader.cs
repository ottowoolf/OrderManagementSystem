using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Domain
{
    public class OrderHeader
    {
        /// <summary>
        /// Field that stores and instance of the OrderState
        /// </summary>
        private OrderState _state;

        /// <summary>
        /// Constructor that assigns values to the Id and DateTime and calls SetState passing the stateId as an argument
        /// </summary>
        /// <param name="id"></param>
        /// <param name="datetime"></param>
        /// <param name="stateId"></param>
        public OrderHeader(int id, DateTime datetime, int stateId)
        {
            Id = id;
            DateTime = datetime;
            setState(stateId);
        }

        public void GetOrderHeaders()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Property that stores a list of order items
        /// </summary>
        public List<OrderItem> OrderItems { get; } = new List<OrderItem>();

        /// <summary>
        /// Method for adding or updating an order item
        /// </summary>
        /// <param name="stockItemId"></param>
        /// <param name="price"></param>
        /// <param name="description"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public OrderItem AddOrUpdateOrderItem(int stockItemId, decimal price, string description, int quantity)
        {
            OrderItem item = null;
            //check if there an order item exists for the selected stock item
            foreach (var i in OrderItems)
            {
                if (i.StockItemId == stockItemId)
                {
                    item = i;
                }
            }
            //create a new instance and add it to the collection of order items if there isn't an existing one
            if (item == null)
            {
                item = new OrderItem(this, stockItemId, price, description, quantity);
                OrderItems.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
            return item;
        }


        //Properties
        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public int StateId => (int)_state.State;

        public OrderStates State => _state.State;

        public decimal Total { get => OrderItems.Sum(i => i.Total); }


        /// <summary>
        /// Method that sets the state of an order
        /// </summary>
        /// <param name="stateId"></param>
        public void setState(int stateId)
        {
            if (stateId == 1)
            {
                _state = new OrderNew(this);
            }
            else if (stateId == 2)
            {
                _state = new OrderPending(this);
            }
            else if (stateId == 3)
            {
                _state = new OrderRejected(this);
            }
            else if (stateId == 4)
            {
                _state = new OrderComplete(this);
            }
            else
            {
                throw new InvalidOperationException("Invalid StateId Detected");
            }

        }

        //update the state of the order i.e. submit [new > pending], Complete [pending > complete], Reject [pending > rejected]
        /// <summary>
        /// Method to change the state of the order
        /// </summary>
        public void Submit()
        {
            _state.Submit(ref _state);
        }

        public void Complete()
        {
            _state.Complete(ref _state);
        }

        public void Reject()
        {
            _state.Reject(ref _state);
        }
    }
}
