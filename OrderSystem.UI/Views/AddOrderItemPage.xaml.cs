using OrderSystem.Controllers;
using OrderSystem.Domain;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;


namespace OrderSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for AddOrderItemPage.xaml
    /// </summary>
    public partial class AddOrderItemPage : Page
    {
        private OrderHeader _order;

        public IEnumerable<StockItem> StockItems { get; private set; }
        public int Quantity { get; set; } = 1;

        public AddOrderItemPage(OrderHeader order)
        {
            InitializeComponent();
            try
            {
                _order = order;
                StockItems = StockItemController.Instance.GetStockItems();
                DataContext = this;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (StockItem)dgStockItems.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("Please select a stock item", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Quantity < 1)
            {
                MessageBox.Show("Quanity must be greater than zero (0)", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Quantity > item.InStock)
            {
                MessageBox.Show($"There is currently not enough items in stock. Requested: {Quantity} In stock: {item.InStock}\nThis order might be rejected if there is not enough stock when the order is being processed ", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            try
            {
                var order = OrderController.Instance.UpsertOrderItem(_order.Id, item.Id, Quantity);
                NavigationService.Navigate(new AddOrderPage(order));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
