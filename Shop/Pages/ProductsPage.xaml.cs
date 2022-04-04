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
        public ObservableCollection<Product> Products { get; set; }
        public List<Product> ProductsForSearch { get; set; }
        private int startIndex;
        private int countPerPage;
        public ProductsPage()
        {
            InitializeComponent();
            Products = DataAccess.GetProducts();
            ProductsForSearch = Products.ToList();
            Units = DataAccess.GetUnits();
            Units.Add(new Unit { Name = "Все"});
            startIndex = 0;
            cbCountPerPage.SelectedIndex = 0;
            countPerPage = Convert.ToInt32((cbCountPerPage.SelectedItem as ComboBoxItem).Content.ToString());
            GoPagination();
            this.DataContext = this;
        }
        private void ApplyFilters()
        { 
            
        }

        private void cbUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var unit = cbUnits.SelectedItem as Unit;
            if (unit.Name == "Все")
                ProductsForSearch = Products.ToList();
            else
                ProductsForSearch = ProductsForSearch.Where(p => p.UnitId == unit.Id).ToList();

            dgProducts.ItemsSource = ProductsForSearch;
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = tbSearch.Text;
            var search = ProductsForSearch.Where(p => p.Name.ToLower().Contains(text.ToLower()) || p.Description.ToLower().Contains(text.ToLower())).ToList();
            dgProducts.ItemsSource = search;
        }

        private void GoPagination()
        {
            int test = (Products.Count / (countPerPage + startIndex)) > 0 ? countPerPage : Products.Count % countPerPage;
            var search = ProductsForSearch.GetRange(startIndex, test);
            dgProducts.ItemsSource = ProductsForSearch;
            this.DataContext = this;
        }

        private void cbMonthFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedFilter = (cbMonthFilter.SelectedItem as ComboBoxItem).Content.ToString();
            if (selectedFilter == "Все")
            {
                dgProducts.ItemsSource = ProductsForSearch;
            }   
            else
            {
                var search = ProductsForSearch.Where(p => p.AddDate.Month == DateTime.Now.Month ).ToList();
                dgProducts.ItemsSource = search;
            }
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
    }
}
