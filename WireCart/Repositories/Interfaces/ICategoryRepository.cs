using System.Collections;
using System.Linq.Expressions;
using WireCart.Entities;

namespace WireCart.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<int> GetTotalCatCount();
        Task<Category> GetCategoryById(int id);
        Task<IEnumerable<Category>> GetCategories();
        Task<IEnumerable<Category>> GetCategories(Expression<Func<Category, bool>> predict);
        Task<IEnumerable<Category>> GetCategories(int page, int skip);
        Task<IEnumerable<Category>> GetCategoriesWithSubCategory(Expression<Func<Category, bool>> predict);
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task RemoveAsync(Category category);
    }
}
