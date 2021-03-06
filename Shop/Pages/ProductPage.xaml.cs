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
using System.IO;
using Microsoft.Win32;
using Core;

namespace Shop.Pages
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public Product Product { get; set; }
        public List<Country> ProductCountries { get; set; }
        public List<Country> Countries { get; set; }
        public List<Unit> Units { get; set; }

        public ProductPage(Product product)
        {
            InitializeComponent();
            Product = product;
            Units = DataAccess.GetUnits().ToList();
            Countries = DataAccess.GetCountries().ToList();
            FillCountries();
            cbCounties.ItemsSource = Countries;
            lvCountries.ItemsSource = ProductCountries;
            cbUnits.ItemsSource = Units;
            cbUnits.SelectedItem = product.Unit;


            this.DataContext = this;
        }
        public ProductPage()
        {
            InitializeComponent();
            Product = new Product();
            Units = DataAccess.GetUnits().ToList();
            Countries = DataAccess.GetCountries().ToList();
            FillCountries();
            cbCounties.ItemsSource = Countries;
            lvCountries.ItemsSource = ProductCountries;
            cbUnits.ItemsSource = Units;
            cbUnits.SelectedIndex = 0;


            this.DataContext = this;
        }
        private void FillCountries()
        {
            ProductCountries = new List<Country>();
            var productCountries = Product.ProductCountries.Where(p => p.ProductId == Product.Id).ToList();
            foreach (var country in productCountries)
            {
                ProductCountries.Add(Countries.Where(c => c.Id == country.CountryId).FirstOrDefault());
            }
        }

        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            Product.UnitId = (cbUnits.SelectedItem as Unit).Id;

            if (!DataAccess.CheckContent(Product.Name, Product.Description))
            {
                MessageBox.Show("Поля наименование и комментарий могут содержать в себе только буквы и следующие символы: пробел и дефис", "Ошибка");
                return;
            }

            DataAccess.SaveProduct(Product);
            DataAccess.SaveProductCountries(Product.Id, lvCountries.Items.Cast<Country>().ToList());
            
            NavigationService.GoBack();

        }

        private void btnChoicePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
            };

            if (fileDialog.ShowDialog().Value)
            {
                var photo = File.ReadAllBytes(fileDialog.FileName);
                if (photo.Length > 1024 * 150)  //Размер фотографии не должен превышать 150 Кбайт
                {
                    MessageBox.Show("Размер фотографии не должен превышать 150 КБ", "Ошибка");
                    return;
                }
                Product.Photo = photo;
                imageProduct.Source = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }

        private void lvCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var countries = lvCountries.Items.Cast<Country>().ToList();
            var productCountry = lvCountries.SelectedItem as Country;
            countries.Remove(productCountry);

            lvCountries.ItemsSource = countries;
        }

        private void cbCounties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var countries = lvCountries.Items.Cast<Country>().ToList();
            var productCountry = cbCounties.SelectedItem as Country;

            if (countries.Where(c => c.Name == productCountry.Name).Count() != 0)
                return;
            countries.Add(productCountry);

            lvCountries.ItemsSource = countries;
        }
    }
}