using OrderSystem.Controllers;
using OrderSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace OrderSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for AddOrderView.xaml
    /// </summary>
    public partial class AddOrderPage : Page
    {
        public AddOrderPage(OrderHeader order = null)
        {
            try
            {
                if (order == null)
                {
                    DataContext = OrderController.Instance.CreateNewOrderHeader();
                }
                else
                {
                    DataContext = order;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InitializeComponent();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var order = (OrderHeader)DataContext;
            NavigationService.Navigate(new AddOrderItemPage(order));
        }

        private void DeleteOrderItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = (Button)e.Source;
                var item = (OrderItem)button.DataContext;
                var order = (OrderHeader)DataContext;
                DataContext = OrderController.Instance.DeleteOrderItem(order.Id, item.StockItemId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                OrderController.Instance.SubmitOrder((DataContext as OrderHeader).Id);
                NavigationService.Navigate(new OrdersPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OrderController.Instance.DeleteOrderHeaderAndOrderItems((DataContext as OrderHeader).Id);
            NavigationService.Navigate(new OrdersPage());
        }
    }
}
