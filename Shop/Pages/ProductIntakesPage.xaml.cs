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
            IntakeProducts = new List<IntakeProduct> { new IntakeProduct {cbProduct = new ComboBox() } };
            //gridProducts.ItemsSource = IntakeProducts;
            gridProducts.SelectionMode = DataGridSelectionMode.Extended;

            Suppliers = DataAccess.GetSuppliers().ToList();
            cbColumn.ItemsSource = Products;
            DataContext = this;
        }

        private void gridProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var t = (sender as Product);
        }

        private void gridProducts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var t = (gridProducts.ItemsSource as List<IntakeProduct>);

            //tbSumm.Text =
            decimal sum = 0;
            for (int i = 0; i < gridProducts.Items.Count; i++) //taking care of each Row  
            {
                DataGridRow row = (DataGridRow)gridProducts.ItemContainerGenerator.ContainerFromIndex(i)
                //rowcount += 1;
                //sum += intake.PriceUnit * intake.Count;
            }

            tbSumm.Text = sum.ToString();
            //gridProducts.Dispatcher.BeginInvoke(new Action(() => gridProducts.Items.Refresh()), System.Windows.Threading.DispatcherPriority.Background);

            //CollectionViewSource.GetDefaultView(gridProducts.ItemsSource).Refresh();
        }

        private void gridProducts_CurrentCellChanged(object sender, EventArgs e)
        {
            var t = (sender as DataGridComboBoxColumn);
            //foreach ()
            //gridProducts.Items.Refresh();
        }

    }
}
