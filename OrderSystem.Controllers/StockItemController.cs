using OrderSystem.DataAccess;
using OrderSystem.Domain;
using System;
using System.Collections.Generic;


namespace OrderSystem.Controllers
{
    public class StockItemController
    {
        private readonly StockRepository _stockRepo = new StockRepository();

        /// <summary>
        /// Implementing the singleton pattern
        /// </summary>
        private static StockItemController _instance;
        private StockItemController() { }

        public static StockItemController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StockItemController();
                }
                return _instance;
            }
        }


        public IEnumerable<StockItem> GetStockItems()
        {
            return _stockRepo.GetStockItems();
        }
    }
}
