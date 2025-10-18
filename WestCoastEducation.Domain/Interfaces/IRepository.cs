using System.Linq.Expressions;

namespace WestcoastEducation.Domain.Interfaces;

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetAsync(string id);                  // <-- was Guid
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);                   // <-- was Guid
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
}
