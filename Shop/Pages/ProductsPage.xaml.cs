using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
namespace Shop.Pages
{
    /// <summary>
    /// Interaction logic for ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ObservableCollection<Unit> Units { get; set; }
        public List<Product> Products { get; set; }
        public List<Product> ProductsForSearch { get; set; }
        private Dictionary<string, Func<Product, object>> Sortings;

        private int startIndex;
        private int countPerPage;
        public ProductsPage()
        {
            InitializeComponent();
            Products = DataAccess.GetProducts().ToList();
            ProductsForSearch = Products.ToList();
            Units = DataAccess.GetUnits();

            Sortings = new Dictionary<string, Func<Product, object>>
            {
                { "Сначала старые", x => x.AddDate },//reverse
                { "Сначала новые", x => x.AddDate },
                { "А-Я", x => x.Name },
                { "Я-А", x => x.Name } //reverse
            };

            Units.Add(new Unit { Name = "Все"});
            startIndex = 0;
            cbCountPerPage.SelectedIndex = 0;

            countPerPage = Convert.ToInt32((cbCountPerPage.SelectedItem as ComboBoxItem).Content.ToString());
            cbMonthFilter.SelectedIndex = 0;
            DataAccess.NewItemAddedEvent += DataAccess_NewItemAddedEvent;
            cbUnits.SelectedIndex = Units.Count - 1;
            cbSort.SelectedIndex = 0;
            SetUserRestrictions();
            this.DataContext = this;
        }
        private void SetUserRestrictions()
        {
            if (App.User.RoleId == 3)
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                btnEdit.Visibility = Visibility.Hidden;

                btnInvoices.Visibility = Visibility.Hidden;
                //btnOrders.Visibility = Visibility.Hidden;
            }
            else if (App.User.RoleId == 2)
            {
                btnOrders.Visibility = Visibility.Hidden;
            }
        }
        private void ApplyFilters()
        {
            if (cbMonthFilter.SelectedItem != null & cbUnits.SelectedItem != null & cbSort.SelectedItem != null)
            {
                var text = tbSearch.Text;
                var selectedFilter = (cbMonthFilter.SelectedItem as ComboBoxItem).Content.ToString();
                
                var unit = cbUnits.SelectedItem as Unit;

                if (unit.Name == "Все")
                    ProductsForSearch = Products;
                else
                    ProductsForSearch = Products.Where(p => p.UnitId == unit.Id).ToList();

                if (selectedFilter == "Все")
                {
                    dgProducts.ItemsSource = ProductsForSearch;
                }
                else
                {
                    ProductsForSearch = ProductsForSearch.Where(p => p.AddDate.Month == DateTime.Now.Month).ToList();
                }
                
                var sort = (cbSort.SelectedItem as ComboBoxItem).Content.ToString();
                

                ProductsForSearch = ProductsForSearch.Where(p => p.Name.ToLower().Contains(text.ToLower()) || p.Description.ToLower().Contains(text.ToLower())).ToList();
                ProductsForSearch = ProductsForSearch.OrderBy(Sortings[sort]).ToList();
                if (sort == "Я-А" || sort == "Сначала новые")
                    ProductsForSearch.Reverse();
                
                dgProducts.ItemsSource = ProductsForSearch;
            }
        }
        private void DataAccess_NewItemAddedEvent()
        {
            Products = DataAccess.GetProducts().ToList();
            ApplyFilters();
            GoPagination();
            dgProducts.Items.Refresh();
        }
        private void cbUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters(); 
            GoPagination();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
            GoPagination();
        }

        private void GoPagination()
        {
            if (startIndex < ProductsForSearch.Count)
            {
                int test = (ProductsForSearch.Count / (countPerPage + startIndex)) > 0 ? countPerPage : ProductsForSearch.Count % countPerPage;
                var search = ProductsForSearch.GetRange(startIndex, test);
                dgProducts.ItemsSource = search;
            }
            lblWithdraw.Content = $"{dgProducts.ItemsSource.Cast<Product>().ToList().Count} из {Products.Count}";
        }

        private void cbMonthFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
            GoPagination();
        }

        private void btnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (startIndex != 0)
                startIndex -= Convert.ToInt32((cbCountPerPage.SelectedItem as ComboBoxItem).Content.ToString());
            GoPagination();
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (startIndex + Convert.ToInt32((cbCountPerPage.SelectedItem as ComboBoxItem).Content.ToString()) < Products.Count)
                startIndex += Convert.ToInt32((cbCountPerPage.SelectedItem as ComboBoxItem).Content.ToString());
            GoPagination();
        }

        private void cbCountPerPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cbCountPerPage.SelectedItem as ComboBoxItem).Content.ToString() != "Все")
            {
                countPerPage = Convert.ToInt32((cbCountPerPage.SelectedItem as ComboBoxItem).Content.ToString());
            }
            else 
            {
                countPerPage = Products.Count;
            }
            startIndex = 0;
            GoPagination();
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete.IsEnabled = dgProducts.SelectedItems.Count != 0;
            btnEdit.IsEnabled = dgProducts.SelectedItems.Count != 0;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductPage(dgProducts.SelectedItem as Product));
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
            
            var sort = (cbSort.SelectedItem as ComboBoxItem).Content.ToString();
            ProductsForSearch = ProductsForSearch.OrderBy(Sortings[sort]).ToList();
            if (sort == "Я-А" || sort == "Сначала новые")
                ProductsForSearch.Reverse();
            dgProducts.ItemsSource = ProductsForSearch;
            GoPagination();
        }

        private void btnInvoices_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.IntakesPage());
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var products = dgProducts.SelectedItems.Cast<Product>().ToList();

            foreach (var product in products)
            {
                var result = MessageBox.Show($"Вы точно хотите удалить {product.Name}?", "Предупреждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    DataAccess.DeleteProduct(product);
            }
            Products = DataAccess.GetProducts().ToList();
            ApplyFilters();
            GoPagination();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ProductPage());
        }
        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage());
        }
    }
}
