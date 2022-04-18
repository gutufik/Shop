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
    /// Логика взаимодействия для IntakesPage.xaml
    /// </summary>
    public partial class IntakesPage : Page
    {
        public List<ProductIntake> Intakes { get; set; }
        public IntakesPage()
        {
            InitializeComponent();
            Intakes = DataAccess.GetIntakes().ToList();
            SetUserRestrictions();
            this.DataContext = this;
            DataAccess.NewItemAddedEvent += DataAccess_NewItemAddedEvent;
        }

        private void DataAccess_NewItemAddedEvent()
        {
            Intakes = DataAccess.GetIntakes().ToList();
            dgIntakes.ItemsSource = Intakes;
            dgIntakes.Items.Refresh();
        }

        private void SetUserRestrictions()
        {
            if (App.User.RoleId == 1)
            {
                btnCreate.Visibility = Visibility.Hidden;
            }
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.IntakeProductsPage());
        }
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            var intake = dgIntakes.SelectedItem as ProductIntake;
            NavigationService.Navigate(new Pages.IntakeProductsPage(intake));
        }

        private void dgIntakes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnOpen.IsEnabled = dgIntakes.SelectedItem as ProductIntake != null;
        }
    }
}
