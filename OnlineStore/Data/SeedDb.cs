using Microsoft.AspNetCore.Identity;
using OnlineStore.Data.Entities;
using OnlineStore.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDB(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Costumer");
            await _userHelper.CheckRoleAsync("Employee");

            if (!_context.Categories.Any())
            {
                this.AddCategory("Monitor");
                this.AddCategory("Desktop");
                this.AddCategory("Portátil");
                this.AddCategory("Televisor");
                this.AddCategory("Software");
                this.AddCategory("Teclado");
                this.AddCategory("Rato");
                this.AddCategory("Headset");
                this.AddCategory("HeadPhone");
                this.AddCategory("Colunas");
                this.AddCategory("Smartphone");
                await _context.SaveChangesAsync();
            }

            if (!_context.Products.Any())
            {
                this.AddProduct("Headset SteelSeries Arctis 3 Console 2020 Edition Preto", "Headset");
                this.AddProduct("Smartphone Xiaomi Redmi Note 9 6.53 3GB / 64GB Dual SIM Preto", "Smartphone");
                this.AddProduct("Teclado Lenovo Legion K200 Gaming PT Preto", "Teclado");
                this.AddProduct("Rato Óptico SteelSeries Rival 3 Wireless 18000DPI Preto", "Rato");
                this.AddProduct("Portátil Lenovo IdeaPad L340-15IRH Gaming 15.6", "Portátil");
                await _context.SaveChangesAsync();
            }

            var user = await _userHelper.GetUserByEmailAsync("tiagotorres14516@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Tiago",
                    LastName = "Torres",
                    Email = "tiagotorres14516@gmail.com",
                    UserName = "tiagotorres14516@gmail.com",
                    PhoneNumber = "222222222",
                    Address = "Rua Imaginária A"
                };

                var result = await _userHelper.AddUserAsync(user, "~4YZQ-g(C{");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in Seeder");
                }

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

                var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
                if (!isInRole)
                {
                    await _userHelper.AddUserToRoleAsync(user, "Admin");
                }
            }
        }

        private void AddCategory(string name)
        {
            _context.Categories.Add(new Category
            {
                Name = name
            });
        }

        private void AddProduct(string name, string categoryName)
        {
            var category = _context.Categories.Single(c => c.Name == categoryName);

            _context.Products.Add(new Product
            {
                Name = name,
                Category = category
            });
        }
    }
}
