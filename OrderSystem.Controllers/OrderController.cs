using OrderSystem.DataAccess;
using OrderSystem.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace OrderSystem.Controllers
{
    public class OrderController
    {
        /// <summary>
        /// Fields with Repositories for Orders and Stock
        /// </summary>
        private readonly OrderRepository _orderRepo = new OrderRepository();
        private readonly StockRepository _stockRepo = new StockRepository();

        /// <summary>
        /// Implementing the singleton pattern
        /// </summary>
        public static OrderController _instance;

        public OrderController() { }

        public static OrderController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OrderController();
                }
                return _instance;
            }
        }


        public IEnumerable<OrderHeader> GetOrderHeaders()
        {
            return _orderRepo.GetOrderHeaders();
        }

        public OrderHeader CreateNewOrderHeader()
        {
            return _orderRepo.InsertOrderHeader();
        }

        public OrderHeader UpsertOrderItem(int orderHeaderId, int stockItemId, int quantity)
        {
            //find stock item by id
            var stockItem = _stockRepo.GetStockItem(stockItemId);

            //find the order header by id
            var order = _orderRepo.GetOrderHeader(orderHeaderId);

            //create or update the order item
            var orderItem = order.AddOrUpdateOrderItem(stockItemId, stockItem.Price, stockItem.Name, quantity);

            _orderRepo.UpsertOrderItem(orderItem);

            return order;
        }
        public OrderHeader SubmitOrder(int orderHeaderId)
        {
            //Create a new instance of the order object
            var order = _orderRepo.GetOrderHeader(orderHeaderId);

            //Update the state of the order to pending
            order.Submit();

            //Persist the changes to the database
            _orderRepo.UpdateOrderState(order);

            return order;
        }
        public OrderHeader ProcessOrder(int orderHeaderId)
        {
            var order = _orderRepo.GetOrderHeader(orderHeaderId);
            try
            {
                _stockRepo.UpdateStockItemAmount(order);
                order.Complete();
            }
            catch (SqlException ex)
            {
                int exceptionNumber = ex.Number; //547 
                //this means the stock item [InStock] check constraint threw an exception
                order.Reject();
            }
            _orderRepo.UpdateOrderState(order);
            return order;
        }
        public void DeleteOrderHeaderAndOrderItems(int orderHeaderId)
        {
            _orderRepo.DeleteOrderHeaderAndOrderItems(orderHeaderId);
        }
        public OrderHeader DeleteOrderItem(int orderHeaderId, int stockItemId)
        {
            _orderRepo.DeleteOrderItem(orderHeaderId, stockItemId);
            var order = _orderRepo.GetOrderHeader(orderHeaderId);
            return order;
        }

        public void ResetDatabase()
        {
            _orderRepo.ResetDatabase();
        }
    }
}
