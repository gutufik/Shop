using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core
{
    public static class DataAccess
    {
        public static ObservableCollection<User> GetUsers()
        {
            return new ObservableCollection<User>(ShopBozyaEntities.GetContext().Users);
        }

        public static User GetUser(int id)
        {
            return GetUsers().Where(user => user.Id == id).FirstOrDefault();
        }

        public static User GetUser(string login, string password)
        {
            return GetUsers().Where(user => user.Login == login && user.Password == password).FirstOrDefault();
        }

        public static bool TryLogin(string login, string password)
        {
            return GetUser(login, password) != null;
        }

        public static bool RegistartionUser(string login, string password)
        {
            User user = new User
            {
                Login = login,
                Password = password,
                RoleId = GetRole("Клиент").Id
            };

            ShopBozyaEntities.GetContext().Users.Add(user);
            return Convert.ToBoolean(ShopBozyaEntities.GetContext().SaveChanges());
        }



        public static bool CheckLogin(string login)
        {
            return GetUsers().Where(user => user.Login == login).Count() == 0;
        }

        public static bool CheckPassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^])(?=.*[^a-zA-Z0-9])\S{6,16}$");

            return regex.IsMatch(password);
        }



        public static ObservableCollection<Role> GetRoles()
        {
            return new ObservableCollection<Role>(ShopBozyaEntities.GetContext().Roles);
        }

        public static Role GetRole(int id)
        {
            return GetRoles().Where(role => role.Id == id).FirstOrDefault();
        }

        public static Role GetRole(string name)
        {
            return GetRoles().Where(role => role.Name == name).FirstOrDefault();
        }


        public static ObservableCollection<Product> GetProducts()
        {
            return new ObservableCollection<Product>(ShopBozyaEntities.GetContext().Products);
        }

        public static bool SaveProduct(Product product, List<ProductCountry> productCountries)
        {
            if (GetProducts().Where(p => p.Id == product.Id).Count() == 0)
            {
                product.AddDate = DateTime.Now;
                ShopBozyaEntities.GetContext().Products.Add(product);
            }
            else
                ShopBozyaEntities.GetContext().Products.SingleOrDefault(p => p.Id == product.Id);

            return Convert.ToBoolean(ShopBozyaEntities.GetContext().SaveChanges());
        }

        /*
        public static bool SaveProductCountries(int productId, int countryId)
        {
            if (GetProductCountries().Where(p => p.ProductId == productId && p.CountryId == countryId).Count() == 0)
            {
                ShopBozyaEntities.GetContext().ProductCountries.Add(new ProductCountry
                {
                    ProductId = productId,
                    CountryId = countryId
                });
            }
            else
            {
            }
            return true;
        }
        */
        public static ObservableCollection<ProductCountry> GetProductCountries()
        {
            return new ObservableCollection<ProductCountry>(ShopBozyaEntities.GetContext().ProductCountries);
        }

        public static bool DeleteProduct(Product product)
        {
            ShopBozyaEntities.GetContext().Products.Remove(product);
            return Convert.ToBoolean(ShopBozyaEntities.GetContext().SaveChanges());
        }

        public static ObservableCollection<Unit> GetUnits()
        {
            return new ObservableCollection<Unit>(ShopBozyaEntities.GetContext().Units);
        }

        public static ObservableCollection<Country> GetCountries()
        {
            return new ObservableCollection<Country>(ShopBozyaEntities.GetContext().Countries);
        }
        public static ObservableCollection<ProductIntake> GetIntakes()
        {
            return new ObservableCollection<ProductIntake>(ShopBozyaEntities.GetContext().ProductIntakes);
        }
        public static ObservableCollection<Supplier> GetSuppliers()
        {
            return new ObservableCollection<Supplier>(ShopBozyaEntities.GetContext().Suppliers);
        }

    }
}