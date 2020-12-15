using OrderSystem.Controllers;
using OrderSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;


namespace OrderSystem.UI.Views
{
    /// <summary>
    /// OrderPage logic for navigating to the AddOrderPage or OrderDetailsPage
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            try
            {
                dgOrders.ItemsSource = OrderController.Instance.GetOrderHeaders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddOrderPage());
        }

        private void OrderDetails_Click(object sender, RoutedEventArgs e)
        {
            var order = (OrderHeader)((Button)e.Source).DataContext;
            NavigationService.Navigate(new OrderDetailsPage(order));
        }
    }
}
