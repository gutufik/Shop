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
            return new ObservableCollection<User>(ShopMyasnikovEntities.GetContext().Users);
        }

        public static User GetUser(int id)
        {
            return GetUsers().Where(user => user.Id == id).FirstOrDefault();
        }

        public static User GetUser(string login, string password)
        {
            return GetUsers().Where(user => user.Login == login && user.Password == password).FirstOrDefault();
        }

        public static bool IsUserCorrect(string login, string password)
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

            ShopMyasnikovEntities.GetContext().Users.Add(user);
            return Convert.ToBoolean(ShopMyasnikovEntities.GetContext().SaveChanges());
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
            return new ObservableCollection<Role>(ShopMyasnikovEntities.GetContext().Roles);
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
            return new ObservableCollection<Product>(ShopMyasnikovEntities.GetContext().Products);
        }

        public static bool SaveProduct(Product product, List<ProductCountry> productCountries)
        {
            if (GetProducts().Where(p => p.Id == product.Id).Count() == 0)
            {
                product.AddDate = DateTime.Now;
                ShopMyasnikovEntities.GetContext().Products.Add(product);
            }
            else
                ShopMyasnikovEntities.GetContext().Products.SingleOrDefault(p => p.Id == product.Id);

            return Convert.ToBoolean(ShopMyasnikovEntities.GetContext().SaveChanges());
        }
        public static bool SaveProductIntake(ProductIntake productIntake)
        {
            if (GetIntakes().Where(i => i.Id == productIntake.Id).Count() == 0)
            {
                ShopMyasnikovEntities.GetContext().ProductIntakes.Add(new ProductIntake());
            }
            else
                ShopMyasnikovEntities.GetContext().ProductIntakes.SingleOrDefault(i => i.Id == productIntake.Id);

            return Convert.ToBoolean(ShopMyasnikovEntities.GetContext().SaveChanges());
        }
        public static bool SaveProductIntakeProducts(int productIntakeId, List<ProductIntakeProduct> products)
        {
            foreach (var product in products)
            {
                product.ProductIntakeId = productIntakeId;

                if (GetProductIntakeroducts().Where(p => p.ProductIntakeId == productIntakeId).Count() == 0)
                {
                    ShopMyasnikovEntities.GetContext().ProductIntakeProducts.Add(product);
                }
            }

            return Convert.ToBoolean(ShopMyasnikovEntities.GetContext().SaveChanges());
        }
        public static ObservableCollection<ProductIntakeProduct> GetProductIntakeroducts()
        {
            return new ObservableCollection<ProductIntakeProduct>(ShopMyasnikovEntities.GetContext().ProductIntakeProducts);
        }

        /*

        public static bool SaveProductCountries(int productId, List<Country> countries)
        {
            foreach (var country in countries)
            {
                ProductCountry productCountry = new ProductCountry
                {
                    ProductId = productId,
                    CountryId = country.Id
                };

                if (GetProductCountries().Where(p => p.ProductId == productId && p.CountryId == country.Id).Count() == 0)
                {
                    ShopMyasnikovEntities.GetContext().ProductCountries.Add(productCountry);
                }
            }
            
            return Convert.ToBoolean(ShopMyasnikovEntities.GetContext().SaveChanges());
        }
        public static bool SaveProduct(Product product)
        {
            if (GetProducts().Where(p => p.Id == product.Id).Count() == 0)
            {
                product.AddDate = DateTime.Now;
                ShopMyasnikovEntities.GetContext().Products.Add(product);
            }
            else
                ShopMyasnikovEntities.GetContext().Products.SingleOrDefault(p => p.Id == product.Id);

            return Convert.ToBoolean(ShopMyasnikovEntities.GetContext().SaveChanges());
        }
        */
        public static ObservableCollection<ProductCountry> GetProductCountries()
        {
            return new ObservableCollection<ProductCountry>(ShopMyasnikovEntities.GetContext().ProductCountries);
        }

        public static bool DeleteProduct(Product product)
        {
            ShopMyasnikovEntities.GetContext().Products.Remove(product);
            return Convert.ToBoolean(ShopMyasnikovEntities.GetContext().SaveChanges());
        }

        public static ObservableCollection<Unit> GetUnits()
        {
            return new ObservableCollection<Unit>(ShopMyasnikovEntities.GetContext().Units);
        }

        public static ObservableCollection<Country> GetCountries()
        {
            return new ObservableCollection<Country>(ShopMyasnikovEntities.GetContext().Countries);
        }
        public static ObservableCollection<ProductIntake> GetIntakes()
        {
            return new ObservableCollection<ProductIntake>(ShopMyasnikovEntities.GetContext().ProductIntakes);
        }
        public static ObservableCollection<Supplier> GetSuppliers()
        {
            return new ObservableCollection<Supplier>(ShopMyasnikovEntities.GetContext().Suppliers);
        }

    }
}