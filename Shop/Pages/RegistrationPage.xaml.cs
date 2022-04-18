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
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            var login = tbLogin.Text;
            var passwrod = pbPassword.Password;

            if (!DataAccess.CheckLogin(login))
            {
                MessageBox.Show("Данный логин уже занят", "Ошибка");
                return;
            }

            if (!DataAccess.CheckPassword(passwrod))
            {
                MessageBox.Show("Неверный формат пароля", "Ошибка");
                return;
            }

            if (DataAccess.RegistartionUser(login, passwrod))
            {
                var user = DataAccess.GetUser(login, passwrod);
                DataAccess.CreateClient(user);
                NavigationService.Navigate(new AuthorizationPage());
            }
        }
    }
}
