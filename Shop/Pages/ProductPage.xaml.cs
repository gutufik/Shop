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

namespace Shop.Pages
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public Product Product { get; set; }

        public ProductPage()
        {
            InitializeComponent();
            Product = new Product();
            this.DataContext = this;
        }


        public ProductPage(Product product)
        {
            InitializeComponent();
            Product = product;
            this.DataContext = this;
        }

        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            //Product.Name = tbName.Text;
            //Product.UnitId = (cbUnits.SelectedItem as Unit).Id;
            //Product.Description = tbDescription.Text;
            //DataAccess.AddOrEditProduct(Product);
        }
    }
}