using Microsoft.EntityFrameworkCore;
using OnlineStore.Data.Entities;
using OnlineStore.Helpers;
using OnlineStore.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Data.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<Order>> GetOrdersAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .OrderByDescending(o => o.OrderDate);
            }

            return _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User == user).OrderByDescending(o => o.OrderDate);
        }


        public async Task<IQueryable<OrderDetailsTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }



            return _context.OrderDetailsTemp
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.Product.Name);
        }


        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return;
            }

            var product = await _context.Products.FindAsync(model.ProductId);

            if (product == null)
            {
                return;
            }

            var orderDetailsTemp = await _context.OrderDetailsTemp
                .Where(odt => odt.User == user && odt.Product == product)
                .FirstOrDefaultAsync();

            if (orderDetailsTemp == null)
            {
                orderDetailsTemp = new OrderDetailsTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user,
                };

                _context.OrderDetailsTemp.Add(orderDetailsTemp);
            }
            else
            {
                orderDetailsTemp.Quantity += model.Quantity;
                _context.OrderDetailsTemp.Update(orderDetailsTemp);
            }

            await _context.SaveChangesAsync();
        }


        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await _context.OrderDetailsTemp.FindAsync(id);

            if (orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;

            if (orderDetailTemp.Quantity > 0)
            {
                _context.OrderDetailsTemp.Update(orderDetailTemp);
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailsTemp = await _context.OrderDetailsTemp.FindAsync(id);
            if (orderDetailsTemp == null)
            {
                return;
            }

            _context.OrderDetailsTemp.Remove(orderDetailsTemp);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return false;
            }

            var orderTemps = await _context.OrderDetailsTemp
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTemps == null || orderTemps.Count == 0)
            {
                return false;
            }

            var details = orderTemps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity
            }).ToList();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details,
            };

            _context.Orders.Add(order);
            _context.OrderDetailsTemp.RemoveRange(orderTemps);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
    }
}
