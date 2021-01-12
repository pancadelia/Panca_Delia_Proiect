using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopModel.Data;
using ShopModel.Models;

namespace Panca_Delia_Proiect.Data
{
    public class DbInitializer
    {
        public static void Initialize(Shop context)
        {
            context.Database.EnsureCreated();
            if (context.Products.Any())
            {
                return; // BD a fost creata anterior
            }
            var products = new Product[]
            {
 new Product{Title="Bomboane cu Ciocolata neagra",Brand="ClujFactory",Price=Decimal.Parse("22"),Weight="50"},
 new Product{Title="Bomboane cu Ciocolata alba",Brand="ClujFactory",Price=Decimal.Parse("18"),Weight="30"},
 new Product{Title="ciocolata cu alune",Brand="MuresFactory",Price=Decimal.Parse("27"),Weight="100"},
 new Product{Title="Jeleuri frunctate",Brand="MuresFactory",Price=Decimal.Parse("27"),Weight="80"}
            };
            foreach (Product b in products)
            {
                context.Products.Add(b);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {

 new Customer{CustomerID=1050,Name="Panca Delia",PhoneNumber=int.Parse("0740506789")},
 new Customer{CustomerID=1045,Name="Panca Adina",PhoneNumber=int.Parse("0765783451")},

 };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
 new Order{ProductID=1,CustomerID=1050,OrderDate=DateTime.Parse("2021-01-01")},
 new Order{ProductID=3,CustomerID=1045,OrderDate=DateTime.Parse("2021-01-02")},
 new Order{ProductID=1,CustomerID=1045,OrderDate=DateTime.Parse("2021-01-03")},
 new Order{ProductID=2,CustomerID=1050,OrderDate=DateTime.Parse("2021-01-01")},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();
            var categories = new Category[]
 {

 new Category{CategoryName="Bomboane",Description="Aici vei gasi o gama variata de ciocolata"},
 new Category{CategoryName="Ciocolata",Description="Daca esti fan bomboane, ai ajuns in locul potrivit!"},
 new Category{CategoryName="Jeleuri",Description="Arunca o privire peste aceste jeluri delicioase!"},
 };
            foreach (Category p in categories)
            {
                context.Categories.Add(p);
            }
            context.SaveChanges();
            var publishedproducts = new PublishedProduct[]
            {
 new PublishedProduct { ProductID = products.Single(c => c.Title == "Bomboane cu Ciocolata neagra" ).ID, CategoryID = categories.Single(i => i.CategoryName =="Bomboane").ID},
 new PublishedProduct { ProductID = products.Single(c => c.Title == "Bomboane cu Ciocolata alba" ).ID,CategoryID = categories.Single(i => i.CategoryName =="Bomboane").ID },
 new PublishedProduct { ProductID = products.Single(c => c.Title == "Ciocolata cu alune" ).ID,CategoryID = categories.Single(i => i.CategoryName =="Ciocolata").ID },
new PublishedProduct { ProductID = products.Single(c => c.Title == "Jeleuri frunctate" ).ID,CategoryID = categories.Single(i => i.CategoryName =="Jeleuri").ID },

            };
            foreach (PublishedProduct pb in publishedproducts)
            {
                context.PublishedProducts.Add(pb);
            }
            context.SaveChanges();
        }
    }

}
    

