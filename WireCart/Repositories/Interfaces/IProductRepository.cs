using System.Linq.Expressions;
using WireCart.Entities;

namespace WireCart.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<int> GetTotalProductCount();
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProducts(Expression<Func<Product, bool>> predict);
        Task<IEnumerable<Product>> GetProducts(int page, int skip);
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task<IEnumerable<Product>> GetProductByCategory(int categoryId);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task RemoveAsync(Product product);
    }
}
