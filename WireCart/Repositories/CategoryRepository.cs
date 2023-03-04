using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WireCart.Data;
using WireCart.Entities;
using WireCart.Repositories.Interfaces;

namespace WireCart.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly WireCartDataContext _dbContext;

        public CategoryRepository(
            WireCartDataContext dbContext
            )
        {
            _dbContext = dbContext;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetTotalCatCount()
        {
            return await _dbContext.Categories.CountAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories(Expression<Func<Category, bool>> predict)
        {
            
            return await _dbContext.Categories.Where(predict).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories(int page, int skip)
        {
            return await _dbContext.Categories.Skip((page - 1) * skip).Take(skip).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithSubCategory(Expression<Func<Category, bool>> predict)
        {

            return await _dbContext.Categories.Where(predict).Include(x => x.SubCategories).ToListAsync();
        }

        public async Task<Category> AddAsync(Category category)
        {
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            _dbContext.Entry(category).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Category category)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
