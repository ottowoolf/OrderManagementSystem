using OrderSystem.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.DataAccess
{
    public class StockRepository
    {
        private string _connectionString;
        public StockRepository()
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["OrderManagementDb"];
            _connectionString = connectionString.ConnectionString;
        }
        public IEnumerable<StockItem> GetStockItems()
        {
            var items = new List<StockItem>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_SelectStockItems", connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    decimal price = reader.GetDecimal(2);
                    int inStock = reader.GetInt32(3);
                    var item = new StockItem(id, name, price, inStock);
                    items.Add(item);
                }
            }
            return items;
        }

        public StockItem GetStockItem(int id)
        {
            StockItem item = null;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_SelectStockItemById @id", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id", id);
                var reader = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                reader.Read();
                string name = reader.GetString(1);
                decimal price = reader.GetDecimal(2);
                int inStock = reader.GetInt32(3);
                item = new StockItem(id, name, price, inStock);
            }
            return item;
        }

        public void UpdateStockItemAmount(OrderHeader order)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_UpdateStockItemAmount @id, @amount", connection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    foreach (var orderItem in order.OrderItems)
                    {
                        command.Parameters.AddWithValue("@id", orderItem.StockItemId);
                        command.Parameters.AddWithValue("@amount", -orderItem.Quantity);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
