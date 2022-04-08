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
    /// Логика взаимодействия для IntakeProductsPage.xaml
    /// </summary>
    public partial class IntakeProductsPage : Page
    {
        public List<ProductIntake> Intakes { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<ProductIntakeProduct> IntakeProducts { get; set; }


        public IntakeProductsPage()
        {
            Suppliers = DataAccess.GetSuppliers().ToList();
            DataContext = this;
            InitializeComponent();
        }
    }
}
