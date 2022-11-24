using Microsoft.EntityFrameworkCore;
using System;

namespace ASP.NET_CORE_WEB_APP_MVC_SHOP.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;
                }

                var users = new List<User>
                {
                   new User()
                   {
                       FirstName="Alex",
                       LastName="Johns",
                       Login="alex_123",
                       Password="123456"
                   },
                   new User()
                   {
                       FirstName="Admin",
                       LastName="Admin",
                       Login="Admin",
                       Password="123456"
                   }
                };

                context.Users.AddRange(users);
                if (context.Products.Any())
                {
                    return;
                }
                var products = new List<Product>
                {
                    new Product()
                    {
                        ProductName="Milk"
                    },
                    new Product()
                    {
                        ProductName="Cake"
                    },
                    new Product()
                    {
                        ProductName="Water botle"
                    }
                };

                context.Products.AddRange(products);    
               

                context.SaveChanges();
            }

        }
    }
}
