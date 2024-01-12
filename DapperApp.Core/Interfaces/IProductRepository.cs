using DapperApp.Core.Entities;

namespace DapperApp.Core.Interfaces
{
    public interface IProductRepository : IRepository<Product, int>
    {
        Task<Product> FindByName(string productName);
    }
}
