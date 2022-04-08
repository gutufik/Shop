﻿using System;
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
        private Dictionary<string, Func<Product, object>> Sortings;

        private int startIndex;
        private int countPerPage;
        public ProductsPage()
        {
            InitializeComponent();
            Products = DataAccess.GetProducts();
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

            cbUnits.SelectedIndex = Units.Count - 1;
            cbSort.SelectedIndex = 0;
            this.DataContext = this;
        }
        private void ApplyFilters()
        {
            if (cbMonthFilter.SelectedItem != null & cbUnits.SelectedItem != null & cbSort.SelectedItem != null)
            {
                var text = tbSearch.Text;
                var selectedFilter = (cbMonthFilter.SelectedItem as ComboBoxItem).Content.ToString();
                
                var unit = cbUnits.SelectedItem as Unit;

                if (unit.Name == "Все")
                    ProductsForSearch = Products.ToList();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.IntakesPage());
        }
    }
}
