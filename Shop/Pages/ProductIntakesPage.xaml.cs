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
using Shop;

namespace Shop.Pages
{
    /// <summary>
    /// Логика взаимодействия для IntakeProductsPage.xaml
    /// </summary>
    public partial class IntakeProductsPage : Page
    {
        public List<ProductIntake> Intakes { get; set; }
        public List<Product> Products { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<IntakeProduct> IntakeProducts { get; set; }


        public IntakeProductsPage()
        {
            InitializeComponent();
            Products = DataAccess.GetProducts().ToList();
            IntakeProducts = new List<IntakeProduct>();
            dpDate.SelectedDate = DateTime.Now;
            
            //gridProducts.ItemsSource = IntakeProducts;
            gridProducts.SelectionMode = DataGridSelectionMode.Extended;

            Suppliers = DataAccess.GetSuppliers().ToList();
            cbSupplier.SelectedIndex = 0;
            
            //cbColumn.DataPropertyName = "Table_ID";
            DataContext = this;
        }

        private void gridProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var t = (sender as Product);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            IntakeProducts.Add(new IntakeProduct() {ProductId = (cbProduct.SelectedItem as Product).Id, Product = (cbProduct.SelectedItem as Product) });
            gridProducts.Items.Refresh();
        }


        private void gridProducts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (this.gridProducts.SelectedItem != null)
            {
                (sender as DataGrid).RowEditEnding -= gridProducts_RowEditEnding;
                //(gridProducts.SelectedItem as IntakeProduct).ProductId = (gridProducts.SelectedItem as IntakeProduct).Product.Id;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();

                decimal sum = 0;
                foreach (IntakeProduct product in gridProducts.ItemsSource)
                {
                    sum += product.Sum;
                }
                tbSum.Text = sum.ToString();
                (sender as DataGrid).RowEditEnding += gridProducts_RowEditEnding;
            }

            //ComboBox ele = gridProducts.Columns[0].GetCellContent(gridProducts.Items[0]) as ComboBox;
            
            return;
        }
        private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            ComboBox ele = gridProducts.Columns[2].GetCellContent(gridProducts.Items[0]) as ComboBox;
            try
            {
                var selectedItem = ele.Text;
                MessageBox.Show(selectedItem.ToString());

            }
            catch { }
        }

        private void btnConduct_Click(object sender, RoutedEventArgs e)
        {
            ProductIntake intake = new ProductIntake {
                Id = 0,
                SupplierId = (cbSupplier.SelectedItem as Supplier).Id,
                TotalAmount = decimal.Parse(tbSum.Text),
                Data = (DateTime)dpDate.SelectedDate,
                StatusIntakeId = 2,
                IsDeleted = false
            };

            //foreach (IntakeProduct product in IntakeProducts)
            //{
            //    product.ProductId = product.Product.Id;
            //}

            //var products = gridProducts.ItemsSource.Cast<ProductIntakeProduct>().ToList();

            DataAccess.SaveProductIntake(intake);
            //DataAccess.SaveProductIntakeProducts(intake.Id, products);
        }
    }
}
