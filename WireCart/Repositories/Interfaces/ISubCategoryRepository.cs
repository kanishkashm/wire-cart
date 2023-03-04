using System.Collections;
using System.Linq.Expressions;
using WireCart.Entities;

namespace WireCart.Repositories.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<int> GetTotalSubCatCount();
        Task<SubCategory> GetSubCategoryById(int id);
        Task<IEnumerable<SubCategory>> GetSubCategories();
        Task<IEnumerable<SubCategory>> GetSubCategories(Expression<Func<SubCategory, bool>> predict);
        Task<IEnumerable<SubCategory>> GetSubCategories(int page, int skip);
        Task<SubCategory> AddAsync(SubCategory category);
        Task UpdateAsync(SubCategory category);
        Task RemoveAsync(SubCategory category);
    }
}
