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
    /// Interaction logic for AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();

            tbLogin.Text = Properties.Settings.Default.Login;
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var login = tbLogin.Text;
            var password = pbPassword.Password;

            if (DataAccess.TryLogin(login, password))
            {
                if (cbRemember.IsChecked.GetValueOrDefault())
                    Properties.Settings.Default.Login = login;
                else
                    Properties.Settings.Default.Login = null;
                Properties.Settings.Default.Save();
                NavigationService.Navigate(new ProductsPage());
            }
            else
                MessageBox.Show("Неверный логин или пароль", "Ошибка");
        }
    }
}
