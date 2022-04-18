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
        public delegate void NewItemAddedDelegate();

        public static event NewItemAddedDelegate NewItemAddedEvent;
        public static ObservableCollection<User> GetUsers()
        {
            return new ObservableCollection<User>(ShopMyasnikovEntities.GetContext().Users);
        }
        public static Client GetClient(User user)
        {
            return ShopMyasnikovEntities.GetContext().Clients.FirstOrDefault(cl => cl.UserId == user.Id);
        }
        public static Worker GetWorker(User user)
        {
            return ShopMyasnikovEntities.GetContext().Workers.FirstOrDefault(w => w.UserId == user.Id);
        }
        public static User GetUser(int id)
        {
            return GetUsers().Where(user => user.Id == id).FirstOrDefault();
        }

        public static User GetUser(string login, string password)
        {
            return GetUsers().Where(user => user.Login == login && user.Password == password).FirstOrDefault();
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
            return new ObservableCollection<Role>(ShopMyasnikovEntities.GetContext().Roles.Where(r => !r.IsDeleted));
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
            return new ObservableCollection<Product>(ShopMyasnikovEntities.GetContext().Products.Where(p => !p.IsDeleted));
        }
        public static void SaveProduct(Product product)
        {
            if (GetProducts().Where(p => p.Id == product.Id).Count() == 0)
            {
                product.AddDate = DateTime.Now;
                ShopMyasnikovEntities.GetContext().Products.Add(product);
            }
            else
                ShopMyasnikovEntities.GetContext().Products.SingleOrDefault(p => p.Id == product.Id);

            ShopMyasnikovEntities.GetContext().SaveChanges();
            NewItemAddedEvent?.Invoke();
        }
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

        public static bool RemoveProductCounrtry(int productId, int countryId)
        {
            ShopMyasnikovEntities.GetContext().ProductCountries.Remove(GetProductCountry(productId, countryId));
            return Convert.ToBoolean(ShopMyasnikovEntities.GetContext().SaveChanges());
        }

        public static ProductCountry GetProductCountry(int productId, int countryId)
        {
            return GetProductCountries().Where(p => p.ProductId == productId && p.CountryId == countryId).FirstOrDefault();
        }

        public static ObservableCollection<ProductCountry> GetProductCountries()
        {
            return new ObservableCollection<ProductCountry>(ShopMyasnikovEntities.GetContext().ProductCountries);
        }

        public static List<ProductCountry> GetProductCountries(Product product)
        {
            return GetProductCountries().Where(p => p.ProductId == product.Id).ToList();
        }

        public static bool DeleteProduct(Product product)
        {
            product.IsDeleted = true;
            ShopMyasnikovEntities.GetContext().Products.SingleOrDefault(p => p.Id == product.Id);
            return Convert.ToBoolean(ShopMyasnikovEntities.GetContext().SaveChanges());
        }

        public static ObservableCollection<Unit> GetUnits()
        {
            return new ObservableCollection<Unit>(ShopMyasnikovEntities.GetContext().Units.Where(u => !u.IsDeleted));
        }

        public static ObservableCollection<Country> GetCountries()
        {
            return new ObservableCollection<Country>(ShopMyasnikovEntities.GetContext().Countries.Where(c => !c.IsDeleted));
        }

        public static Country GetCountry(int id)
        {
            return GetCountries().Where(c => c.Id == id).FirstOrDefault();
        }

        public static bool CheckContent(string name, string description)
        {
            Regex regex = new Regex(@"^[А-Яа-яA-Za-z\s\-]+$");

            return regex.IsMatch(name) && regex.IsMatch(description);
        }

        public static ObservableCollection<ProductIntake> GetProductIntakes()
        {
            return new ObservableCollection<ProductIntake>(ShopMyasnikovEntities.GetContext().ProductIntakes.Where(c => !(bool)c.IsDeleted));
        }

        public static ObservableCollection<Supplier> GetSuppliers()
        {
            return new ObservableCollection<Supplier>(ShopMyasnikovEntities.GetContext().Suppliers.Where(c => !c.IsDeleted));
        }

        public static ObservableCollection<StatusIntake> GetStatusIntakes()
        {
            return new ObservableCollection<StatusIntake>(ShopMyasnikovEntities.GetContext().StatusIntakes.Where(c => !c.IsDeleted));
        }

        public static ObservableCollection<ProductIntakeProduct> GetProductIntakeProducts()
        {
            return new ObservableCollection<ProductIntakeProduct>(ShopMyasnikovEntities.GetContext().ProductIntakeProducts.Where(c => !c.IsDeleted));
        }

        public static ObservableCollection<Order> GetOrders()
        {
            return new ObservableCollection<Order>(ShopMyasnikovEntities.GetContext().Orders.Where(c => !c.IsDeleted));
        }

        public static ObservableCollection<ProductOrder> GetProductOrders()
        {
            return new ObservableCollection<ProductOrder>(ShopMyasnikovEntities.GetContext().ProductOrders.Where(c => !c.IsDeleted));
        }

        public static ObservableCollection<StatusOrder> GetStatusOrders()
        {
            return new ObservableCollection<StatusOrder>(ShopMyasnikovEntities.GetContext().StatusOrders.Where(c => !c.IsDeleted));
        }

        public static ObservableCollection<Worker> GetWorkers()
        {
            return new ObservableCollection<Worker>(ShopMyasnikovEntities.GetContext().Workers.Where(c => !c.IsDeleted));
        }

        public static void SaveProductIntake(ProductIntake productIntake)
        {
            if (GetProductIntakes().Where(p => p.Id == productIntake.Id).Count() == 0)
            {
                ShopMyasnikovEntities.GetContext().ProductIntakes.Add(productIntake);
            }
            else
                ShopMyasnikovEntities.GetContext().Products.SingleOrDefault(p => p.Id == productIntake.Id);

            ShopMyasnikovEntities.GetContext().SaveChanges();
            NewItemAddedEvent?.Invoke();
        }
        

        public static bool IsUserCorrect(string login, string password)
        {
            return GetUser(login, password) != null;
        }
        public static ObservableCollection<ProductIntakeProduct> GetProductIntakeroducts()
        {
            return new ObservableCollection<ProductIntakeProduct>(ShopMyasnikovEntities.GetContext().ProductIntakeProducts);
        }
        public static ObservableCollection<ProductIntake> GetIntakes()
        {
            return new ObservableCollection<ProductIntake>(ShopMyasnikovEntities.GetContext().ProductIntakes);
        }

        public static void SaveOrder(Order order)
        {
            if (GetOrders().Where(o => o.Id == order.Id).Count() == 0)
            {
                order.Date = DateTime.Now;
                ShopMyasnikovEntities.GetContext().Orders.Add(order);
            }
            else
                ShopMyasnikovEntities.GetContext().Orders.SingleOrDefault(p => p.Id == order.Id);

            ShopMyasnikovEntities.GetContext().SaveChanges();
            NewItemAddedEvent?.Invoke();
        }

        public static bool SaveProductOrder(int orderId, List<ProductOrder> productOrders)
        {
            foreach (var productOrder in productOrders)
            {
                productOrder.OrderId = orderId;

                if (GetProductOrders().Where(po => po.OrderId == productOrder.OrderId && po.ProductId == productOrder.ProductId).Count() == 0)
                {
                    ShopMyasnikovEntities.GetContext().ProductOrders.Add(productOrder);
                }
                else
                    ShopMyasnikovEntities.GetContext().ProductOrders.SingleOrDefault(po => po.OrderId == productOrder.OrderId && po.ProductId == productOrder.ProductId);
            }

            return Convert.ToBoolean(ShopMyasnikovEntities.GetContext().SaveChanges());
        }
        public static List<Order> GetUserOrders(User user)
        {
            var client = GetClient(user);
            return GetOrders().Where(o=> o.ClientId == client.Id).ToList();
        }
 
    }
}