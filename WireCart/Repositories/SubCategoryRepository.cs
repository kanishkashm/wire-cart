using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WireCart.Data;
using WireCart.Entities;
using WireCart.Repositories.Interfaces;

namespace WireCart.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly WireCartDataContext _dbContext;

        public SubCategoryRepository(
            WireCartDataContext dbContext
            )
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetTotalSubCatCount()
        {
            return await _dbContext.SubCategories.CountAsync();
        }

        public async Task<SubCategory> GetSubCategoryById(int id)
        {
            return await _dbContext.SubCategories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategories()
        {
            return await _dbContext.SubCategories.ToListAsync();
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategories(Expression<Func<SubCategory, bool>> predict)
        {
            return await _dbContext.SubCategories.Where(predict).ToListAsync();
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategories(int page, int skip)
        {
            return await _dbContext.SubCategories.Skip((page - 1) * skip).Take(skip).ToListAsync();
        }

        public async Task<SubCategory> AddAsync(SubCategory subCategory)
        {
            _dbContext.SubCategories.Add(subCategory);
            await _dbContext.SaveChangesAsync();
            return subCategory;
        }

        public async Task UpdateAsync(SubCategory subCategory)
        {
            _dbContext.Entry(subCategory).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(SubCategory subCategory)
        {
            _dbContext.SubCategories.Remove(subCategory);
            await _dbContext.SaveChangesAsync();
        }
    }
}
