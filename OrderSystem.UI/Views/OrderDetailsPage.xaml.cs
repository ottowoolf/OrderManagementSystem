using OrderSystem.Controllers;
using OrderSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for OrderDetailView.xaml
    /// </summary>
    public partial class OrderDetailsPage : Page
    {
        public OrderDetailsPage(OrderHeader order)
        {
            DataContext = order;
            InitializeComponent();
        }

        private void ProcessOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = (OrderHeader)DataContext;
                if (order.State == OrderStates.New)
                {
                    DataContext = OrderController.Instance.SubmitOrder(order.Id);
                }
                else if (order.State == OrderStates.Pending)
                {
                    DataContext = OrderController.Instance.ProcessOrder(order.Id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage());
        }
    }
}
