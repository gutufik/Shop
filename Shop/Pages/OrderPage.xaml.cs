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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    /// public List<ProductIntake> Intakes { get; set; }
    public partial class OrderPage : Page
    {
        public List<Product> Products { get; set; }
        public Order Order { get; set; }
        public List<StatusOrder> StatusOrders { get; set; }
        public OrderPage()
        {
            InitializeComponent();
            Products = DataAccess.GetProducts().ToList();
            dpDate.SelectedDate = DateTime.Now;
            
            StatusOrders = DataAccess.GetStatusOrders().ToList();
            //gridProducts.ItemsSource = IntakeProducts;
            Order = new Order
            {
                StatusOrder = StatusOrders[0]
            };
            cbStatus.SelectedItem = Order.StatusOrder;
            gridProducts.SelectionMode = DataGridSelectionMode.Extended;

            //cbColumn.DataPropertyName = "Table_ID";
            this.DataContext = this;
        }

        public OrderPage(Order order)
        {
            InitializeComponent();
            Order = order;
            Products = DataAccess.GetProducts().ToList();
            dpDate.SelectedDate = DateTime.Now;

            StatusOrders = DataAccess.GetStatusOrders().ToList();
            cbStatus.SelectedItem = Order.StatusOrder;
            //gridProducts.ItemsSource = IntakeProducts;
            gridProducts.SelectionMode = DataGridSelectionMode.Extended;
            //cbColumn.DataPropertyName = "Table_ID";
            this.DataContext = this;
            SetEnable();
        }

        private void gridProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var t = (sender as Product);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var product = cbProduct.SelectedItem as Product;
            Order.ProductOrders.Add(new ProductOrder
            {
                Product = product,
                ProductId = product.Id
            });

            //IntakeProducts.Add(new IntakeProduct() { ProductId = (cbProduct.SelectedItem as Product).Id, Product = (cbProduct.SelectedItem as Product) });
            gridProducts.Items.Refresh();
            Products.Remove(product);
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


        private void btnConduct_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.SaveOrder(Order);

            //foreach (IntakeProduct product in IntakeProducts)
            //{
            //    product.ProductId = product.Product.Id;
            //}

            //var products = gridProducts.ItemsSource.Cast<ProductIntakeProduct>().ToList();

            //DataAccess.SaveProductIntakeProducts(intake.Id, products);
        }

        private void SetEnable()
        {
            if (Order.StatusOrder.Name != "Новый")
            {
                grid.IsEnabled = false;
            }
        }
    }
}
