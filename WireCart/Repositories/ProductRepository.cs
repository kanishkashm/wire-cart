using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using WireCart.Data;
using WireCart.Entities;
using WireCart.Repositories.Interfaces;

namespace WireCart.Repositories
{
    public class ProductRepository : IProductRepository
    {
        protected readonly WireCartDataContext _dbContext;

        public ProductRepository(WireCartDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetTotalProductCount()
        {
            return await _dbContext.Products.CountAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts(Expression<Func<Product, bool>> predict)
        {
            return await _dbContext.Products.Where(predict).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts(int page, int skip)
        {
            return await _dbContext.Products.Skip((page - 1) * skip).Take(skip).ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _dbContext.Products
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            return await _dbContext.Products
                    .Include(p => p.SubCategory)
                    .Where(p => string.IsNullOrEmpty(name) || p.Name.ToLower().Contains(name.ToLower()))
                    .OrderBy(p => p.Name)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(int categoryId)
        {
            return await _dbContext.Products
                .Where(x => x.SubCategoryId == categoryId).ToListAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Product product)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
