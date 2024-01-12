using DapperApp.Core.Entities;
using DapperApp.Core.Interfaces;
using System.Data;

namespace DapperApp.Infrastructure.Repositories
{
    public class ProductRepository : RepositoryBase, IProductRepository
    {
        public ProductRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public async Task<Product> FindByName(string productName)
        {
            return await QuerySingleOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @Name", new { Name = productName });
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await QueryAsync<Product>("SELECT * FROM Products");
        }

        public async Task<Product> GetByIdAsync(int key)
        {
            return await QuerySingleOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @ProductId", new { ProductId = key });
        }

        public async Task<int> AddAsync(Product product)
        {
            return await ExecuteAsync("INSERT INTO Products (Name, Price) VALUES (@Name, @Price); SELECT CAST(SCOPE_IDENTITY() as int)", product);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var rowsAffected = await ExecuteAsync("UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id", product);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var rowsAffected = await ExecuteAsync("DELETE FROM Products WHERE Id = @ProductId", new { ProductId = key });
            return rowsAffected > 0;
        }
    }
}
