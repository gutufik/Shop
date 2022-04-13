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
using Core;
using System.Collections.ObjectModel;

namespace Shop.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public ObservableCollection<Order> Orders { get; set; }

        public OrdersPage()
        {
            InitializeComponent();
            Orders = DataAccess.GetOrders();
            this.DataContext = this;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderPage());
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            var order = dgOrders.SelectedItem as Order;

            NavigationService.Navigate(new OrderPage(order));
        }
    }
}
