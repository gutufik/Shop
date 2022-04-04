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
        public ProductsPage()
        {
            InitializeComponent();
            Products = DataAccess.GetProducts();
            ProductsForSearch = Products.ToList();
            Units = DataAccess.GetUnits();
            Units.Add(new Unit { Name = "Все"});
            this.DataContext = this;
        }

        private void cbUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var unit = cbUnits.SelectedItem as Unit;
            if (unit.Name == "Все")
                ProductsForSearch = Products.ToList();
            else
                ProductsForSearch = Products.Where(p => p.UnitId == unit.Id).ToList();

            dgProducts.ItemsSource = ProductsForSearch;
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = tbSearch.Text;
            var search = ProductsForSearch.Where(p => p.Name.ToLower().Contains(text.ToLower()) || p.Description.ToLower().Contains(text.ToLower())).ToList();
            dgProducts.ItemsSource = search;
        }
    }
}
