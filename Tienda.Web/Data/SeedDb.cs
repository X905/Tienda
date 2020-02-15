namespace Tienda.Web.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Identity;
    using Helpers;

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private Random random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            var user = await this.userHelper.GetUserByEmailAsync("dbocastillo@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Angel",
                    LastName = "Castillo",
                    Email = "dbocastillo@gmail.com",
                    UserName = "dbocastillo@gmail.com",
                    PhoneNumber = "60184063"
                };

                var result = await this.userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }
            if (!this.context.Products.Any())
            {
                this.AddProduct("iPad Air", user, "~/images/Products/ipad.png");
                this.AddProduct("Samsung se", user, "~/images/Products/samsung.webp");
                this.AddProduct("Office 365 License", user, "~/images/Products/office.png");
                this.AddProduct("Xiaomi Redmi", user, "~/images/Products/xiaomi-redmi-note.jpg");
                await this.context.SaveChangesAsync();
            }
        }


        private void AddProduct(string name, User user, string imageUrl)
        {
            this.context.Products.Add(new Product
            {
                Name = name,
                Price = this.random.Next(1000),
                IsAvailabe = true,
                Stock = this.random.Next(100),
                User = user,
                ImageUrl = imageUrl
            });
        }
    }

}
