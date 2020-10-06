using OnlineStore.Data.Entities;
using OnlineStore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Data.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrdersAsync(string userName);

        Task<IQueryable<OrderDetailsTemp>> GetDetailTempsAsync(string userName);

        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);

        Task DeleteDetailTempAsync(int id);

        Task<bool> ConfirmOrderAsync(string userName);

        Task<Order> GetOrderAsync(int id);
    }
}
